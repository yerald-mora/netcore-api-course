using Microsoft.AspNetCore.Http;
using NETCoreMoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class MovieCreationDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }

        [FileSizeValidation(4)]
        [FileTypeValidation(FileTypeGroup.Image)]
        public IFormFile Poster { get; set; }
    }
}
