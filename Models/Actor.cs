using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NETCoreMoviesAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }

        public List<MoviesActors> Movies { get; set; }

    }
}
