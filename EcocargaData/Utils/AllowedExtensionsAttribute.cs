using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Cl.Gob.Energia.Ecocarga.Data.Utils
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _Extensions;
        public AllowedExtensionsAttribute(string[] Extensions)
        {
            _Extensions = Extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GetErrorMessageRequerido());
            }

            var file = value as IFormFile;

            var extension = Path.GetExtension(file.FileName);
            if (!_Extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Esta extensión no es permitida!";
        }

        public string GetErrorMessageRequerido()
        {
            return $"Imagen requerida";
        }
    }
}
