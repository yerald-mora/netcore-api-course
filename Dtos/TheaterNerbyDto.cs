using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class TheaterNerbyDto : TheaterDto
    {
        public double DistanceInMeters { get; set; }
    }
}
