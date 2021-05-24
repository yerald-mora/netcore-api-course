using System;
using System.ComponentModel.DataAnnotations;

namespace NETCoreMoviesAPI.Dtos
{
    public class ReviewCreationDto
    {
        public string Commentary { get; set; }

        [Range(1, 5)]
        public int Punctuation { get; set; }
    }
}
