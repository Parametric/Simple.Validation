using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Simple.Validation.Validators
{
    public abstract class CompositeValidator<T> : IValidator<T>
    {
        public abstract bool AppliesTo(string rulesSet);

        public PropertyValidator<T, bool> For(Expression<Func<T, bool>> propertyExpression)
        {
            return Properties<T>.For(propertyExpression);
        }

        public PropertyValidator<T, IComparable> For(Expression<Func<T, IComparable>> propertyExpression)
        {
            return Properties<T>.For(propertyExpression);
        }

        public StringPropertyValidator<T> For(Expression<Func<T, string>> propertyExpression)
        {
            return Properties<T>.For(propertyExpression);
        }

        public ReferencePropertyValidator<T> For(Expression<Func<T, object>> propertyExpression)
        {
            return Properties<T>.For(propertyExpression);
        }

        public EnumerablePropertyValidator<T> For(Expression<Func<T, IEnumerable>> propertyExpression)
        {
            return Properties<T>.For(propertyExpression);
        } 

        public IEnumerable<ValidationResult> Validate(T value)
        {
            var propertyValidators = GetInternalValidators();
            var results = propertyValidators.SelectMany(v => v.Validate(value));
            return results;
        }

        protected abstract IEnumerable<IValidator<T>> GetInternalValidators();
    }
}
