using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation
{
    public class RangePropertyValidator<TContext, TProperty> : IValidator<TContext> where TProperty: struct, IComparable
    {
        private readonly Expression<Func<TContext, TProperty?>> _propertyExpression;
        private readonly RangeRequirements<TProperty> _rangeRequirements;
        private readonly string _propertyName;
        private string _message;

        public bool AppliesTo(string rulesSet)
        {
            return true;
        }

        public IEnumerable<ValidationResult> Validate(TContext value)
        {
            var propertyValue = _propertyExpression.Compile().Invoke(value);
            return RangeValidator.Validate(_rangeRequirements, propertyValue, _propertyName, value, _message);
        }

        public RangePropertyValidator(Expression<Func<TContext, TProperty?>> propertyExpression)
        {
            _propertyExpression = propertyExpression;
            this._rangeRequirements = new RangeRequirements<TProperty>();

            var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            _propertyName = propertyInfo.Name;
        }

        public RangePropertyValidator<TContext, TProperty> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        }

        public RangePropertyValidator<TContext, TProperty> MinValue(TProperty? minValue)
        {
            _rangeRequirements.MinValue = minValue;
            return this;
        }

        public RangePropertyValidator<TContext, TProperty> MaxValue(TProperty? maxValue)
        {
            _rangeRequirements.MaxValue = maxValue;
            return this;
        }

        public RangePropertyValidator<TContext, TProperty> GreaterThan(TProperty? minValue)
        {
            return MinValue(minValue).LowerExclusive();
        }

        public RangePropertyValidator<TContext, TProperty> GreaterThanOrEqualTo(TProperty minValue)
        {
            return MinValue(minValue).LowerInclusive();
        }

        public RangePropertyValidator<TContext, TProperty> LessThan(TProperty maxValue)
        {
            return MaxValue(maxValue).UpperExclusive();
        }

        public RangePropertyValidator<TContext, TProperty> LessThanOrEqualTo(TProperty? maxValue)
        {
            return MaxValue(maxValue).UpperInclusive();
        }

        public RangePropertyValidator<TContext, TProperty> LowerInclusive()
        {
            _rangeRequirements.LowerInclusive = true;
            return this;
        }

        public RangePropertyValidator<TContext, TProperty> LowerExclusive()
        {
            _rangeRequirements.LowerInclusive = false;
            return this;
        }

        public RangePropertyValidator<TContext, TProperty> UpperInclusive()
        {
            _rangeRequirements.UpperInclusive = true;
            return this;
        }

        public RangePropertyValidator<TContext, TProperty> UpperExclusive()
        {
            _rangeRequirements.UpperInclusive = false;
            return this;
        }
    }
}