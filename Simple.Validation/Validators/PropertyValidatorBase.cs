using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation.Validators
{
    public class PropertyValidatorBase<T>
    {
        public PropertyValidatorBase(Expression<Func<T, object>> propertyExpression)
        {
            PropertyExpression = propertyExpression;
            PropertyInfo = Expressions.GetPropertyInfoFromExpression(propertyExpression);
        }

        protected Expression<Func<T, object>> PropertyExpression { get; private set; }
        protected PropertyInfo PropertyInfo { get; private set; }
    }
}