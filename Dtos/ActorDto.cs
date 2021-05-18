using System;
using System.ComponentModel.DataAnnotations;

namespace NETCoreMoviesAPI.Dtos
{
    public class ActorDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }

    }
}
