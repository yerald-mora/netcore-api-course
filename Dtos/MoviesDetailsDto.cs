using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class MoviesDetailsDto : MovieDto
    {
        public IEnumerable<GenreDto> Genres { get; set; }
        public IEnumerable<ActorMovieDetailsDto> Actors { get; set; }
    }
}
