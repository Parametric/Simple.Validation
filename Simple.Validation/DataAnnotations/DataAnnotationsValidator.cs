using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

            var classLevelValidationResults = GetClassLevelValidationResults(value, validationContext);
            var propertyLevelValidationResults = GetPropertyLevelValidationResults(value, validationContext);
            var allResults = classLevelValidationResults.Union(propertyLevelValidationResults);
            return allResults;
        }

        private IEnumerable<ValidationResult> GetPropertyLevelValidationResults(T value, ValidationContext validationContext)
        {

            var query = from propertyInfo in value.GetType().GetProperties()
                        from attr in propertyInfo.GetCustomAttributes<ValidationAttribute>(inherit: true)
                        let propertyValue = propertyInfo.GetValue(value, null)
                        where !attr.IsValid(propertyValue)
                        let validationAttributeResult = attr.GetValidationResult(propertyValue, validationContext)
                        select new ValidationResult()
                        {
                            Context = value,
                            Message = validationAttributeResult.ErrorMessage,
                            PropertyName = propertyInfo.Name,
                            Type = "DataAnnotationError",
                        };

            return query;
        }

        private static IEnumerable<ValidationResult> GetClassLevelValidationResults(T value, ValidationContext validationContext)
        {
            var attributes = value.GetType().GetCustomAttributes<ValidationAttribute>(inherit: true);
            var invalidAttributes = attributes.Where(attr => !attr.IsValid(value));

            var attributeValidationResults =
                from attr in invalidAttributes
                let attrValidationResult = attr.GetValidationResult(value, validationContext)
                select new ValidationResult()
                           {
                               Context = value,
                               Message = attrValidationResult.ErrorMessage,
                               PropertyName = attrValidationResult.MemberNames.FirstOrDefault(),
                               Type = "DataAnnotationError",
                           };
            return attributeValidationResults;
        }
    }

    public class DataAnnotationsValidator : DataAnnotationsValidator<object>
    {
        
    }
}
