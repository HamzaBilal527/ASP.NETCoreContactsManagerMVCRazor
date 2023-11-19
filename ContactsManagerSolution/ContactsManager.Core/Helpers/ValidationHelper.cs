using System;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
	public class ValidationHelper
    {
        internal static void ModeValidation(object obj)
		{
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
	}
}

