using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NETCoreMoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Helpers
{
    public class MovieExistsAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ApplicationDbContext _dbcontext;

        public MovieExistsAttribute(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var movieIdObject = context.HttpContext.Request.RouteValues["movieId"];

            if (movieIdObject == null)
                return;

            int movieId = int.Parse(movieIdObject.ToString());

            var movieExists = await _dbcontext.Movies.AnyAsync(m => m.Id == movieId);

            if (!movieExists)
                context.Result = new NotFoundResult();
            else
                await next();
        }
    }

}
