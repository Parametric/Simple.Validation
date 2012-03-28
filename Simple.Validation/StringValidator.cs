using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation
{
    public static class StringValidator
    {
        public static IEnumerable<ValidationResult> Validate<T>(StringRequirements requirements, T context, Expression<Func<T, string>> propertyExpression, string message = "")
        {
            var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            var propertyValue = propertyExpression.Compile().Invoke(context);
            var result = Validate(requirements, propertyValue, propertyInfo.Name, context, message);
            return result;
        }

        public static IEnumerable<ValidationResult> Validate(StringRequirements requirements, string value, string propertyName, object context = null, string message = "")
        {

            var valueToValidate = requirements.GetValueToValidate(value);

            if (requirements.Required && string.IsNullOrWhiteSpace(valueToValidate))
                yield return new ValidationResult()
                                 {
                                     Context = context,
                                     Message = message,
                                     PropertyName = propertyName,
                                     Type = TextValidationResultType.RequiredValueNotFound,
                                 };

            if (value == null)
                yield break;

            if (requirements.MinLength.HasValue && valueToValidate.Length < requirements.MinLength)
                yield return new ValidationResult()
                                 {
                                     Context = context,
                                     Message = message,
                                     PropertyName = propertyName,
                                     Type = TextValidationResultType.TextLengthOutOfRange,
                                 };

            if (requirements.MaxLength.HasValue && valueToValidate.Length > requirements.MaxLength)
                yield return new ValidationResult()
                                 {
                                     Context = context,
                                     Message = message,
                                     PropertyName = propertyName,
                                     Type = TextValidationResultType.TextLengthOutOfRange,
                                 };

            if (!string.IsNullOrWhiteSpace(requirements.RegularExpression))
            {
                if (!requirements.IsRegExMatch(valueToValidate))
                {
                    yield return  new ValidationResult()
                                      {
                                          Context = context,
                                          Message = message,
                                          PropertyName = propertyName,
                                          Type = TextValidationResultType.RegularExpressionMismatch,
                                      };
                }
            }
        }
    }
}