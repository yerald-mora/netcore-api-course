using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class MovieFilterDto
    {
        public int Page { get; set; } = 1;
        public int RecordPerPage { get; set; } = 10;
        public PaginationDto Pagination 
        { 
            get
            {
                return new PaginationDto() { Page = Page, RecordsPerPage = RecordPerPage };
            }
         }

        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool NextReleases { get; set; }
    }
}
