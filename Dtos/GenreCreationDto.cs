using System.ComponentModel.DataAnnotations;

namespace NETCoreMoviesAPI.Dtos
{
    public class GenreCreationDto
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
