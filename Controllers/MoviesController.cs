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

namespace NETCoreMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly string _container ="movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
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
                    .Where(m => m.Genres
                                .Select(g => g.GenreId)
                                .Contains(movieFilterDto.GenreId)
                        );

            await HttpContext.InsertPaginationParameter(queryMovies, movieFilterDto.RecordPerPage);

            var movies = await queryMovies.Paginate(movieFilterDto.Pagination).ToListAsync();

            return _mapper.Map<List<MovieDto>>(movies);
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDto>> Get(int id)
        {
            var Movie = await _context.Movies.FindAsync(id);

            if (Movie == null)
                return NotFound();

            return _mapper.Map<MovieDto>(Movie);
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
                .Include(m => m.Actors)
                .Include(m => m.Genres)
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
            if (movie.Actors != null)
            {
                for (int i = 0; i < movie.Actors.Count; i++)
                {
                    movie.Actors[i].Order = i;
                }
            }
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<MoviePatchDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            var moviePatchDto = _mapper.Map<MoviePatchDto>(movie);

            patchDocument.ApplyTo(moviePatchDto, ModelState);

            var isValid = TryValidateModel(moviePatchDto);

            if (!isValid)
                return BadRequest(ModelState);

            _mapper.Map(moviePatchDto, movie);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!MovieExists(id))
                return NotFound();

            _context.Remove(new Movie() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
