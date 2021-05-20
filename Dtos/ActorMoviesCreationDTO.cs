using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class ActorMoviesCreationDTO
    {
        public int ActorId { get; set; }
        public string Character { get; set; }
    }
}
