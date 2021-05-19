using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        public int MaxRecordsPerPage { get; } = 50;

        private int recordsPerpage = 10;

        public int RecordsPerPage
        {
            get { return recordsPerpage; }
            set 
            {
                recordsPerpage = (value > MaxRecordsPerPage) ? MaxRecordsPerPage : value;
            }
        }

    }
}
