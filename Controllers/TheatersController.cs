using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Models;
using NetTopologySuite.Geometries;
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
        private readonly GeometryFactory _geometryFactory;

        public TheatersController(ApplicationDbContext context, IMapper mapper,GeometryFactory geometryFactory) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _geometryFactory = geometryFactory;
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

        [HttpGet("Nearby")]
        public async Task<ActionResult<IEnumerable<TheaterNerbyDto>>> Nearby([FromQuery] TheaterNearbyFilter filter)
        {
            var userLocation = _geometryFactory.CreatePoint(new Coordinate(filter.Longitude, filter.Latitude));
            var theater = await _context.Theaters
                .OrderBy(t => t.Location.Distance(userLocation))
                .Where(t => t.Location.IsWithinDistance(userLocation, filter.DistanceInMeters))
                .Select(t => new TheaterNerbyDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Latitude = t.Location.Y,
                        Longitude = t.Location.X,
                        DistanceInMeters = Math.Round( t.Location.Distance(userLocation))
                    }
                )
                .ToListAsync();

            return theater;
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
