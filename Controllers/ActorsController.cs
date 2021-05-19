using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Models;
using NETCoreMoviesAPI.Services;

namespace NETCoreMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly string _container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
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
        public async Task<ActionResult> Post([FromForm] ActorCreationDto actorCreationDto)
        {
            var actor = _mapper.Map<Actor>(actorCreationDto);

            if (actorCreationDto.Photo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await actorCreationDto.Photo.CopyToAsync(ms);
                    var content = ms.ToArray();
                    var extension = Path.GetExtension(actorCreationDto.Photo.FileName);

                    actor.Photo = await _fileStorage.SaveFile(content, extension, _container, actorCreationDto.Photo.ContentType);
                }
            }

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            var actorDto = _mapper.Map<ActorDto>(actor);

            return CreatedAtAction("Get", new { id = actor.Id }, actorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] ActorCreationDto actorCreationDto)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
            
            if (actor == null)
                return NotFound();

            actor = _mapper.Map(actorCreationDto, actor);

            if (actorCreationDto.Photo != null)
            {
                using (var ms = new MemoryStream())
                {
                    await actorCreationDto.Photo.CopyToAsync(ms);
                    var content = ms.ToArray();
                    var extension = Path.GetExtension(actorCreationDto.Photo.FileName);

                    actor.Photo = await _fileStorage.EditFile(content, extension, _container, actor.Photo,actorCreationDto.Photo.ContentType);
                }
            }

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
