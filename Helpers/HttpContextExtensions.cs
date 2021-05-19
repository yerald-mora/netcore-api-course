using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameter<T>(this HttpContext httpContext, IQueryable<T> querybale, int recordsPerPage)
        {
            double recordsNumber = await querybale.CountAsync();
            double pagesNumber = Math.Ceiling(recordsNumber / recordsPerPage);

            httpContext.Response.Headers.Add("pagesNumber", pagesNumber.ToString());

        }
    }
}
