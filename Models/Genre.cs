﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Models
{
    public class Genre : IIdentifier
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public List<MoviesGenres> Movies { get; set; }
    }
}
