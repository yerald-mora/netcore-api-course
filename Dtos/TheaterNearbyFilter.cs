using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class TheaterNearbyFilter
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }

        private int _maxDistanceInKm = 80;

        private int _distanceInKM = 30;

        public int DistanceInKM
        {
            get { return _distanceInKM; }
            set { _distanceInKM = (value > _maxDistanceInKm) ? _maxDistanceInKm : value; }
        }

        public int DistanceInMeters { get { return DistanceInKM * 1000; } }

    }
}
