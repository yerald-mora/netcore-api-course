using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class MoviesIndexDto
    {
        public IEnumerable<MovieDto> NextReleases { get; set; }
        public IEnumerable<MovieDto> InTheaters { get; set; }
    }
}
