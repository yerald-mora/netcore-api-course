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
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> Get()
        {
            var actor = await _context.Actors.ToListAsync();
            var actorDtos = _mapper.Map<List<ActorDto>>(actor);

            return actorDtos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreationDto actorCreationDto)
        {
            var actor = _mapper.Map<Actor>(actorCreationDto);

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            var actorDto = _mapper.Map<ActorDto>(actor);

            return CreatedAtAction("Get", new { id = actor.Id }, actorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ActorCreationDto actorCreationDto)
        {
            var actor = _mapper.Map<Actor>(actorCreationDto);
            actor.Id = id;

            _context.Entry(actor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            if (!ActorExists(id))
                return NotFound();

            _context.Remove(new Actor() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
