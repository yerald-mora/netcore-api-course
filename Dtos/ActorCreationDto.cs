using Microsoft.AspNetCore.Http;
using NETCoreMoviesAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Dtos
{
    public class ActorCreationDto
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        [FileSizeValidation(maxSizeMB:4)]
        [FileTypeValidation(FileTypeGroup.Image)]
        public IFormFile Photo { get; set; }
    }
}
