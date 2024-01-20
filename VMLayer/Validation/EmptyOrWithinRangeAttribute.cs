using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLayer.Validation
{
    public class EmptyOrWithinRangeAttribute: ValidationAttribute
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string valueAsString && (string.IsNullOrEmpty(valueAsString) || (valueAsString.Length >= MinLength && valueAsString.Length <= MaxLength)))
            {
                return ValidationResult.Success!;
            }
            else
            {
                return new ValidationResult($"Длина записи должна быть от { MinLength } до { MaxLength} символов или быть пустой.");
            }

        }
    }
}
