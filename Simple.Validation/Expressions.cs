using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation
{
    public static class Expressions
    {
        public static MemberExpression GetMemberExpression(LambdaExpression expression, bool throwError = true)
        {
            MemberExpression result = null;
            if (expression.Body is MemberExpression)
            {
                result = (MemberExpression)expression.Body;
            }
            else if (expression.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression.Body ;
                var operand = unaryExpression.Operand;
                result = operand as MemberExpression;                    
            }

            if (result == null && throwError)
                throw new ArgumentException("Could not convert expression to MemberExpression.");

            return result;
        }

        public static PropertyInfo GetPropertyInfoFromExpression(LambdaExpression propertyExpression)
        {
            var memberExpression = GetMemberExpression(propertyExpression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            return propertyInfo;
        }

    }
}
