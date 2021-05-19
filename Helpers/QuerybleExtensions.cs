using NETCoreMoviesAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Helpers
{
    public static class QuerybleExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationDto paginationDto)
        {
            return query
                .Skip((paginationDto.Page - 1) * paginationDto.RecordsPerPage)
                .Take(paginationDto.RecordsPerPage);
        }
    }
}
