using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Dtos;
using NETCoreMoviesAPI.Helpers;
using NETCoreMoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/[controller]")]
    [ServiceFilter(typeof(MovieExistsAttribute))]
    public class ReviewsController : CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> Get( int movieId, [FromQuery] PaginationDto paginationDto)
        {
            var query = _context.Reviews.Include(r => r.User)
                .Where(r => r.MovieId == movieId);

            return await Get<Review, ReviewDto>(paginationDto, query);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int movieId, ReviewCreationDto reviewCreatinDto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var reviewExists = await _context.Reviews.AnyAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (reviewExists)
                return BadRequest("The user has already make a review.");

            var review = _mapper.Map<Review>(reviewCreatinDto);
            review.UserId = userId;
            review.MovieId = movieId;

            _context.Add(review);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{reviewId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> Put(int movieId,int reviewId, ReviewCreationDto reviewCreatinDto)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
                return NotFound();

            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            if (review.UserId != userId)
                return BadRequest("You are not allowed to update this review because you did not created it.");

            review = _mapper.Map(reviewCreatinDto, review);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
                return NotFound();

            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            if (review.UserId != userId)
                return Forbid("You are not allowed to update this review because you did not created it.");

            _context.Remove(review);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
