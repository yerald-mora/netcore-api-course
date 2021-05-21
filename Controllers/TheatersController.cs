using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TheatersController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TheaterDto>>> Get()
        {
            return await Get<Theater, TheaterDto>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TheaterDto>> Get(int id)
        {
            return await Get<Theater, TheaterDto>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(TheaterCreationDto theaterCreationDto)
        {
            return await Post<TheaterCreationDto,Theater, TheaterDto>(theaterCreationDto,"Get");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, TheaterCreationDto theaterCreationDto)
        {
            return await Put<TheaterCreationDto, Theater>(id, theaterCreationDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Theater>(id);
        }
    }
}
