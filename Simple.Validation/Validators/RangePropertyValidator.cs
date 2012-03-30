using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation.Validators
{
    public class RangePropertyValidator<TContext> : PropertyValidatorBase<TContext> 
    {
        private string _message;
        private IComparable _minValue;
        private IComparable _maxValue;
        private bool _lowerInclusive = true;
        private bool _upperInclusive = true;
        private ValidationResultSeverity _severity;

        public RangePropertyValidator(Expression<Func<TContext, IComparable>> propertyExpression) : base(propertyExpression)
        {
            Severity(ValidationResultSeverity.Error);
        }

        public override bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public override IEnumerable<ValidationResult> Validate(TContext value)
        {
            var propertyValue = base.GetPropertyValue<IComparable>(value); 
            return Validate(propertyValue, value, _message);
        }

        private IEnumerable<ValidationResult> Validate(IComparable valueToValidate, object context = null, string message = "")
        {
            if (!IsValidMin(valueToValidate))
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = PropertyInfo.Name,
                    Type = RangeValidationResultType.ValueOutOfRange,
                    Severity = _severity,
                };

            if (!IsValidMax(valueToValidate))
                yield return new ValidationResult()
                {
                    Context = context,
                    Message = message,
                    PropertyName = PropertyInfo.Name,
                    Type = RangeValidationResultType.ValueOutOfRange,
                    Severity = _severity,
                };
        }

        private bool IsValidMin(IComparable valueToValidate)
        {
            if (_minValue == null)
                return true;

            var minCompareResult = _minValue.CompareTo(valueToValidate);
            if (minCompareResult > 0)
                return false;

            if (!_lowerInclusive && _minValue.Equals(valueToValidate))
                return false;

            return true;
        }

        private bool IsValidMax(IComparable valueToValidate)
        {
            if (_maxValue == null)
                return true;

            var maxCompareResult = _maxValue.CompareTo(valueToValidate);
            if (maxCompareResult < 0)
                return false;

            if (!_upperInclusive && _maxValue.Equals(valueToValidate))
                return false;

            return true;
        }

        public RangePropertyValidator<TContext> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        }

        public RangePropertyValidator<TContext> MinValue(IComparable minValue)
        {
            _minValue = minValue;
            return this;
        }

        public RangePropertyValidator<TContext> MaxValue(IComparable maxValue)
        {
            _maxValue = maxValue;
            return this;
        }

        public RangePropertyValidator<TContext> GreaterThan(IComparable minValue)
        {
            return MinValue(minValue).LowerExclusive();
        }

        public RangePropertyValidator<TContext> GreaterThanOrEqualTo(IComparable minValue)
        {
            return MinValue(minValue).LowerInclusive();
        }

        public RangePropertyValidator<TContext> LessThan(IComparable maxValue)
        {
            return MaxValue(maxValue).UpperExclusive();
        }

        public RangePropertyValidator<TContext> LessThanOrEqualTo(IComparable maxValue)
        {
            return MaxValue(maxValue).UpperInclusive();
        }

        public RangePropertyValidator<TContext> LowerInclusive()
        {
            _lowerInclusive = true;
            return this;
        }

        public RangePropertyValidator<TContext> LowerExclusive()
        {
            _lowerInclusive = false;
            return this;
        }

        public RangePropertyValidator<TContext> UpperInclusive()
        {
            _upperInclusive = true;
            return this;
        }

        public RangePropertyValidator<TContext> UpperExclusive()
        {
            _upperInclusive = false;
            return this;
        }

        public RangePropertyValidator<TContext> Severity(ValidationResultSeverity severity)
        {
            _severity = severity;
            return this;
        }
    }
}