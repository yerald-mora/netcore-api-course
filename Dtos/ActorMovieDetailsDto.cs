using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class ActorMovieDetailsDto 
    {
        public int ActorId { get; set; }
        public string Character { get; set; }
        public string Name { get; set; }
    }
}
