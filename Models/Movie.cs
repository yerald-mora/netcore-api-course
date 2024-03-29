﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Models
{
    public class Movie : IIdentifier
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesTheaters> MoviesTheaters { get; set; }

    }
}
