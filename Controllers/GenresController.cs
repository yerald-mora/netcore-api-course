using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Models;

namespace NETCoreMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get()
        {
            var genres = await _context.Genres.ToListAsync();
            var genreDtos = _mapper.Map<List<GenreDto>>(genres);

            return genreDtos;
        }

        [HttpGet("{id}",Name = "GetGenre")]
        public async Task<ActionResult<GenreDto>> Get(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
                return NotFound();

            return _mapper.Map<GenreDto>(genre);
        }

        [HttpPost]
        public async Task<ActionResult> Post(GenreCreationDto genreCreationDto)
        {
            var genre = _mapper.Map<Genre>(genreCreationDto);

            _context.Add(genre);
            await _context.SaveChangesAsync();

            var genreDto = _mapper.Map<GenreDto>(genre);

            return new CreatedAtRouteResult("GetGenre", new { id = genreDto.Id }, genreDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, GenreCreationDto genreCreationDto)
        {
            var genre = _mapper.Map<Genre>(genreCreationDto);
            genre.Id = id;

            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!GenreExists(id))
                return NotFound();

            _context.Remove(new Genre() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
