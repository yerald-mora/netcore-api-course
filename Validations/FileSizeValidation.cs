using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Validations
{
    public class FileSizeValidation : ValidationAttribute
    {
        private readonly int _maxSizeMB;
        private readonly int _maxSizeKB;


        public FileSizeValidation(int maxSizeMB)
        {
            _maxSizeMB = maxSizeMB;
            _maxSizeKB = maxSizeMB * 1024 * 1024;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null) 
                return ValidationResult.Success;

            if(formFile.Length > _maxSizeKB)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {_maxSizeMB} MB");
            }

            return ValidationResult.Success;
        }
    }
}
