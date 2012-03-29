using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Simple.Validation.Validators
{
    public class EnumerablePropertyValidator<T> : PropertyValidatorBase<T>
    {
        private bool _required;
        private ValidationResultSeverity _severity = ValidationResultSeverity.Error;
        private string _message;
        private object _type;
        private int? _minSize;
        private int? _maxSize;

        public EnumerablePropertyValidator(LambdaExpression propertyExpression) : base(propertyExpression)
        {
        }

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public override IEnumerable<ValidationResult> Validate(T value)
        {
            var enumerableValue = GetPropertyValue<IEnumerable<object>>(value);
            if (_required && enumerableValue == null)
                yield return NewValidationResult(value);

            if (enumerableValue == null)
                yield break;

            var size = enumerableValue.Count();
            if (_minSize.HasValue && _minSize > size) 
                yield return NewValidationResult(value);

            if (_maxSize.HasValue && _maxSize < size)
                yield return NewValidationResult(value);
        }

        private ValidationResult NewValidationResult(T value)
        {
            return new ValidationResult()
                       {
                           Severity = _severity,
                           Message = _message,
                           PropertyName = PropertyInfo.Name,
                           Context = value,
                           Type = _type
                       };
        }

        public EnumerablePropertyValidator<T> Required()
        {
            _required = true;
            return this;
        }

        public EnumerablePropertyValidator<T> NotRequired()
        {
            _required = false;
            return this;
        }

        public EnumerablePropertyValidator<T> Size(int? minSize, int? maxSize = null)
        {
            _minSize = minSize;
            _maxSize = maxSize;
            return this;
        }
    }
}
