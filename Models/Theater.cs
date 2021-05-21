using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Models
{
    public class Theater : IIdentifier
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public List<MoviesTheaters> MoviesTheaters { get; set; }

    }
}
