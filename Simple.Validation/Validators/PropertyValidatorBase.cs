using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation.Validators
{
    public abstract class PropertyValidatorBase<T> : IValidator<T>
    {
        protected PropertyValidatorBase(LambdaExpression  propertyExpression)
        {
            PropertyExpression = propertyExpression;
            PropertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);
        }

        protected LambdaExpression  PropertyExpression { get; private set; }
        protected PropertyInfo PropertyInfo { get; private set; }
    
        protected TPropertyType GetPropertyValue<TPropertyType>(T context)
        {
            return (TPropertyType)PropertyExpression.Compile().DynamicInvoke(context);
        }

        public abstract bool AppliesTo(string rulesSet);
        public abstract IEnumerable<ValidationResult> Validate(T value);
    }
}