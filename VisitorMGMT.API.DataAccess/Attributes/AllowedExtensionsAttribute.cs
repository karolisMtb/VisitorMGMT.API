using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VisitorMGMT.API.DataAccess.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        public AllowedExtensionsAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult("This file extension isn't supported");
                }
            }

            return ValidationResult.Success;
        }
    }
}
