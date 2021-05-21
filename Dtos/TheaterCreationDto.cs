using System.ComponentModel.DataAnnotations;

namespace NETCoreMoviesAPI.Dtos
{
    public class TheaterCreationDto
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
    }
}
