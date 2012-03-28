using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simple.Validation.Validators
{
    public class RangePropertyValidator<TContext> : IValidator<TContext> 
    {
        private readonly Expression<Func<TContext, IComparable>> _propertyExpression;
        private readonly RangeRequirements _rangeRequirements;
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

        public RangePropertyValidator(Expression<Func<TContext, IComparable>> propertyExpression)
        {
            _propertyExpression = propertyExpression;
            this._rangeRequirements = new RangeRequirements();

            var propertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            _propertyName = propertyInfo.Name;
        }

        public RangePropertyValidator<TContext> Message(string format, params object[] arguments)
        {
            _message = string.Format(format, arguments);
            return this;
        }

        public RangePropertyValidator<TContext> MinValue(IComparable minValue)
        {
            _rangeRequirements.MinValue = minValue;
            return this;
        }

        public RangePropertyValidator<TContext> MaxValue(IComparable maxValue)
        {
            _rangeRequirements.MaxValue = maxValue;
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
            _rangeRequirements.LowerInclusive = true;
            return this;
        }

        public RangePropertyValidator<TContext> LowerExclusive()
        {
            _rangeRequirements.LowerInclusive = false;
            return this;
        }

        public RangePropertyValidator<TContext> UpperInclusive()
        {
            _rangeRequirements.UpperInclusive = true;
            return this;
        }

        public RangePropertyValidator<TContext> UpperExclusive()
        {
            _rangeRequirements.UpperInclusive = false;
            return this;
        }
    }
}