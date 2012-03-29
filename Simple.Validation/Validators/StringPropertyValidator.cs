using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Simple.Validation.Validators
{
    public class StringPropertyValidator<T> : PropertyValidatorBase<T> //, IValidator<T>
    {
        private string _message;
        private ValidationResultSeverity _severity;

        private bool _ignoreWhiteSpace;
        private bool _required;
        private int? _minLength;
        private int? _maxLength;
        private string _regularExpression;
        private Regex _regex;

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public StringPropertyValidator(Expression<Func<T, string>> propertyExpression) : base(propertyExpression)
        {
            Severity(ValidationResultSeverity.Error);
        }

        public StringPropertyValidator<T> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        }

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var propertyValue = base.GetPropertyValue<string>(value);
            var results = Validate(propertyValue, value, _message);
            return results;
        }

        internal string GetValueToValidate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                if (_ignoreWhiteSpace)
                    return null;

            if (_ignoreWhiteSpace)
                return value.Trim();

            return value;
        }

        private IEnumerable<ValidationResult> Validate(string value, object context = null, string message = "")
        {
            var valueToValidate = GetValueToValidate(value);

            if (_required && string.IsNullOrWhiteSpace(valueToValidate))
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = PropertyInfo.Name,
                    Type = TextValidationResultType.RequiredValueNotFound,
                    Severity = _severity,
                };

            if (value == null)
                yield break;

            if (_minLength.HasValue && valueToValidate.Length < _minLength)
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = PropertyInfo.Name,
                    Type = TextValidationResultType.TextLengthOutOfRange,
                };

            if (_maxLength.HasValue && valueToValidate.Length > _maxLength)
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = PropertyInfo.Name,
                    Type = TextValidationResultType.TextLengthOutOfRange,
                };

            if (!string.IsNullOrWhiteSpace(_regularExpression))
            {
                if (!IsRegExMatch(valueToValidate))
                {
                    yield return new ValidationResult()
                    {
                        Context = context,
                        Message = message,
                        PropertyName = PropertyInfo.Name,
                        Type = TextValidationResultType.RegularExpressionMismatch,
                    };
                }
            }
        }

        private bool IsRegExMatch(string value)
        {
            if (_regex == null)
                throw new InvalidOperationException();

            return _regex.IsMatch(value);
        }

        public StringPropertyValidator<T> Length(int? minLength, int? maxLength = null)
        {
            this._minLength = minLength;
            this._maxLength = maxLength;
            return this;
        }

        public StringPropertyValidator<T> Required()
        {
            this._required = true;
            return this;
        }

        public StringPropertyValidator<T> NotRequired()
        {
            this._required = false;
            return this;
        }
    
        public StringPropertyValidator<T> IgnoreWhiteSpace()
        {
            this._ignoreWhiteSpace = true;
            return this;
        }

        public StringPropertyValidator<T> DoNotIgnoreWhiteSpace()
        {
            this._ignoreWhiteSpace = false;
            return this;
        }

        public StringPropertyValidator<T> Matches(string regularExpression)
        {
            this._regularExpression = regularExpression;
            if (!string.IsNullOrWhiteSpace(regularExpression))
                _regex = new Regex(regularExpression);
            return this;
        }

        public StringPropertyValidator<T> Severity(ValidationResultSeverity validationResultSeverity)
        {
            this._severity = validationResultSeverity;
            return this;
        }
    }
}