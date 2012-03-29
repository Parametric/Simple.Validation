using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Simple.Validation.Validators
{
    public class StringPropertyValidator<T> : IValidator<T>
    {
        private readonly Expression<Func<T, string>> _propertyExpression;
        private readonly StringRequirements _requirements;
        private readonly string _propertyName;
        private string _message;

        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public StringPropertyValidator(Expression<Func<T, string>> propertyExpression)
        {
            _propertyExpression = propertyExpression;
            this._requirements = new StringRequirements();

            var propertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);

            _propertyName = propertyInfo.Name;
        }

        public StringPropertyValidator<T> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        }

        public IEnumerable<ValidationResult> Validate(T value)
        {
            var propertyValue = _propertyExpression.Compile().Invoke(value);
            var results = Validate(this._requirements, propertyValue, _propertyName, value, _message);
            return results;
        }

        private IEnumerable<ValidationResult> Validate(StringRequirements requirements, string value, string propertyName, object context = null, string message = "")
        {
            var valueToValidate = requirements.GetValueToValidate(value);

            if (requirements.Required && string.IsNullOrWhiteSpace(valueToValidate))
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = propertyName,
                    Type = TextValidationResultType.RequiredValueNotFound,
                    Severity = requirements.Severity,
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
                    yield return new ValidationResult()
                    {
                        Context = context,
                        Message = message,
                        PropertyName = propertyName,
                        Type = TextValidationResultType.RegularExpressionMismatch,
                    };
                }
            }
        }

        public StringPropertyValidator<T> Length(int? minLength, int? maxLength = null)
        {
            this._requirements.MinLength = minLength;
            this._requirements.MaxLength = maxLength;
            return this;
        }

        public StringPropertyValidator<T> Required()
        {
            this._requirements.Required = true;
            return this;
        }

        public StringPropertyValidator<T> NotRequired()
        {
            this._requirements.Required = false;
            return this;
        }
    
        public StringPropertyValidator<T> IgnoreWhiteSpace()
        {
            this._requirements.IgnoreWhiteSpace = true;
            return this;
        }

        public StringPropertyValidator<T> DoNotIgnoreWhiteSpace()
        {
            this._requirements.IgnoreWhiteSpace = false;
            return this;
        }

        public StringPropertyValidator<T> Matches(string regularExpression)
        {
            this._requirements.RegularExpression = regularExpression;
            return this;
        }

        public StringPropertyValidator<T> Severity(ValidationResultSeverity validationResultSeverity)
        {
            this._requirements.Severity = validationResultSeverity;
            return this;
        }
    }
}