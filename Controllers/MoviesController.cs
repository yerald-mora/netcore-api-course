using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Helpers;
using NETCoreMoviesAPI.Models;
using NETCoreMoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace NETCoreMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController: CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly string _container ="movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage):
            base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }
        [HttpGet]
        public async Task<ActionResult<MoviesIndexDto>> Get()
        {
            int topMovies = 5;

            var nextReleases = await _context.Movies
                .Where(m => m.ReleaseDate > DateTime.Today)
                .OrderBy(m => m.ReleaseDate)
                .Take(topMovies)
                .ToListAsync();

            var inTheaters = await _context.Movies
                .Where(m => m.InTheaters)
                .Take(topMovies)
                .ToListAsync();

            var moviesIndexDto = new MoviesIndexDto()
            {
                NextReleases = _mapper.Map<IEnumerable<MovieDto>>(nextReleases),
                InTheaters = _mapper.Map<IEnumerable<MovieDto>>(inTheaters)
            };

            return moviesIndexDto;
        }
        
        
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> Filter([FromQuery] MovieFilterDto movieFilterDto)
        {
            var queryMovies = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(movieFilterDto.Title))
                queryMovies = queryMovies.Where(m => m.Title.Contains(movieFilterDto.Title));

            if (movieFilterDto.InTheaters)
                queryMovies = queryMovies.Where(m => m.InTheaters);

            if (movieFilterDto.NextReleases)
                queryMovies = queryMovies.Where(m => m.ReleaseDate > DateTime.Today);

            if (movieFilterDto.GenreId != 0)
                queryMovies = queryMovies
                    .Where(m => m.MoviesGenres
                                .Select(g => g.GenreId)
                                .Contains(movieFilterDto.GenreId)
                        );

            if(!string.IsNullOrEmpty(movieFilterDto.OrderField))
            {
                string orderType = movieFilterDto.AscOrder ? "Ascending" : "Descending";
                queryMovies = queryMovies.OrderBy($"{movieFilterDto.OrderField} {orderType}");
            }

            await HttpContext.InsertPaginationParameter(queryMovies, movieFilterDto.RecordPerPage);

            var movies = await queryMovies.Paginate(movieFilterDto.Pagination).ToListAsync();

            return _mapper.Map<List<MovieDto>>(movies);
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<ActionResult<MoviesDetailsDto>> Get(int id)
        {
            var Movie = await _context.Movies
                .Include(m=>m.MoviesGenres).ThenInclude(g=>g.Genre)
                .Include(m=>m.MoviesActors).ThenInclude(a=>a.Actor)
                .FirstOrDefaultAsync(m=>m.Id == id);

            if (Movie == null)
                return NotFound();

            Movie.MoviesActors = Movie.MoviesActors.OrderBy(a => a.Order).ToList();

            return _mapper.Map<MoviesDetailsDto>(Movie);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]MovieCreationDto movieCreationDto)
        {
            var movie = _mapper.Map<Movie>(movieCreationDto);

            if (movieCreationDto.Poster != null)
            {
                using (var ms = new MemoryStream())
                {
                    await movieCreationDto.Poster.CopyToAsync(ms);
                    var content = ms.ToArray();
                    var extension = Path.GetExtension(movieCreationDto.Poster.FileName);

                    movie.Poster = await _fileStorage.SaveFile(content, extension, _container, movieCreationDto.Poster.ContentType);
                }
            }

            AssignActorsOrder(movie);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var movieDto = _mapper.Map<MovieDto>(movie);

            return CreatedAtAction("Get", new { id = movie.Id }, movieDto);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm]MovieCreationDto movieCreationDto)
        {
            var movie = await _context.Movies
                .Include(m => m.MoviesActors)
                .Include(m => m.MoviesGenres)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (movie == null)
                return NotFound();

            movie = _mapper.Map(movieCreationDto, movie);

            if (movieCreationDto.Poster != null)
            {
                using (var ms = new MemoryStream())
                {
                    await movieCreationDto.Poster.CopyToAsync(ms);
                    var content = ms.ToArray();
                    var extension = Path.GetExtension(movieCreationDto.Poster.FileName);

                    movie.Poster = await _fileStorage.EditFile(content, extension, _container, movie.Poster, movieCreationDto.Poster.ContentType);
                }
            }

            AssignActorsOrder(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void AssignActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<MoviePatchDto> patchDocument)
        {
            return await Patch<Movie, MoviePatchDto>(id,patchDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Movie>(id);
        }

    }
}
