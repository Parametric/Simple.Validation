using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Simple.Validation.DataAnnotations
{
    /// <summary>
    /// Returns a list of ValidationResults indicated by the DataAnnotations attached to the object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAnnotationsValidator<T> : IValidator<T>
    {
        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public IEnumerable<ValidationResult> Validate(T value)
        {
            var validationContext = new ValidationContext(value, null, null);

            var dataAnnotationsValidationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator
                .TryValidateObject(value, validationContext, dataAnnotationsValidationResults, validateAllProperties:true);

            var q = dataAnnotationsValidationResults
                .Select(vr => new ValidationResult()
                {
                    Context = value,
                    Message = vr.ErrorMessage,
                    PropertyName = string.Join(", ", vr.MemberNames),
                    Severity = ValidationResultSeverity.Error,
                    Type = typeof(ValidationAttribute),
                });

            return q;
        }
    }

}
