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
    public class GenresController : CustomBaseController
    {
        public GenresController(ApplicationDbContext context, IMapper mapper)
            :base(context,mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get()
        {
            return await Get<Genre, GenreDto>();
        }

        [HttpGet("{id}",Name = "GetGenre")]
        public async Task<ActionResult<GenreDto>> Get(int id)
        {
            return await Get<Genre, GenreDto>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(GenreCreationDto genreCreationDto)
        {
            return await Post<GenreCreationDto, Genre, GenreDto>(genreCreationDto, "Get");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, GenreCreationDto genreCreationDto)
        {
            return await Put<GenreCreationDto, Genre>(id, genreCreationDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Genre, GenreDto>(id);
        }
    }
}
