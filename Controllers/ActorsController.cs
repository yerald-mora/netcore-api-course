using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Helpers;
using NETCoreMoviesAPI.Models;
using NETCoreMoviesAPI.Services;

namespace NETCoreMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly string _container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
            :base(context,mapper)
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> Get([FromQuery] PaginationDto paginationDto)
        {
            return await Get<Actor, ActorDto>(paginationDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> Get(int id)
        {
            return await Get<Actor, ActorDto>(id);
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

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ActorPatchDto> patchDocument)
        {
            return await Patch<Actor, ActorPatchDto>(id, patchDocument);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            return await Delete<Actor>(id);
        }

    }
}
