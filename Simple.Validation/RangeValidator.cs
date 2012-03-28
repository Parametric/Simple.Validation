using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation
{
    public static class RangeValidator
    {
        //public static IEnumerable<ValidationResult> Validate(RangeRequirements requirements, object context, Expression<Func<object, IComparable>> propertyExpression, string message = "")
        //{
        //    var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
        //    if (propertyInfo == null)
        //    {
        //        throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
        //    }

        //    var propertyValue = propertyExpression.Compile().Invoke(context);
        //    var result = Validate(requirements, propertyValue, propertyInfo.Name, context, message);
        //    return result;
        //}

        public static IEnumerable<ValidationResult> Validate(RangeRequirements requirements, object context, Expression<Func<object, IComparable>> propertyExpression, string message = "")
        {
            var propertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);
            var propertyValue = propertyExpression.Compile().Invoke(context);
            var result = Validate(requirements, propertyValue, propertyInfo.Name, context, message);
            return result;
        }

        public static IEnumerable<ValidationResult> Validate(RangeRequirements requirements, IComparable valueToValidate, string propertyName, object context = null, string message = "")
        {
            if (!requirements.IsValidMin(valueToValidate))
                yield return new ValidationResult()
                {
                    Context = context,
                    Message =  message,
                    PropertyName = propertyName,
                    Type = RangeValidationResultType.ValueOutOfRange,
                }; 

            if (!requirements.IsValidMax(valueToValidate))
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = propertyName,
                    Type = RangeValidationResultType.ValueOutOfRange,
                }; 
        }
    }
}