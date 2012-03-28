using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simple.Validation.Validators
{
    public class StringPropertyValidator<T> : IValidator<T>
    {
        private readonly Expression<Func<T, string>> _propertyExpression;
        private readonly StringRequirements _stringRequirements;
        private readonly string _propertyName;
        private string _message;

        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public StringPropertyValidator(Expression<Func<T, string>> propertyExpression)
        {
            _propertyExpression = propertyExpression;
            this._stringRequirements = new StringRequirements();

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
            var results = StringValidator.Validate(this._stringRequirements, propertyValue, _propertyName, value, _message);
            return results;
        }

        public StringPropertyValidator<T> Length(int? minLength, int? maxLength = null)
        {
            this._stringRequirements.MinLength = minLength;
            this._stringRequirements.MaxLength = maxLength;
            return this;
        }

        public StringPropertyValidator<T> Required()
        {
            this._stringRequirements.Required = true;
            return this;
        }

        public StringPropertyValidator<T> NotRequired()
        {
            this._stringRequirements.Required = false;
            return this;
        }
    
        public StringPropertyValidator<T> IgnoreWhiteSpace()
        {
            this._stringRequirements.IgnoreWhiteSpace = true;
            return this;
        }

        public StringPropertyValidator<T> DoNotIgnoreWhiteSpace()
        {
            this._stringRequirements.IgnoreWhiteSpace = false;
            return this;
        }

        public StringPropertyValidator<T> Matches(string regularExpression)
        {
            this._stringRequirements.RegularExpression = regularExpression;
            return this;
        }

    }
}