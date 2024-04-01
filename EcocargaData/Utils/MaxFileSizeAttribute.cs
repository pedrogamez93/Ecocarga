using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cl.Gob.Energia.Ecocarga.Data.Utils
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GetErrorMessageRequerido());
            }

            var file = value as IFormFile;
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"El máximo tamaño del archivo es de { _maxFileSize} bytes.";
        }

        public string GetErrorMessageRequerido()
        {
            return $"Imagen requerida";
        }
    }
}
