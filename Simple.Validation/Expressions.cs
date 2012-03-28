using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Validation
{
    public static class Expressions
    {
        public static MemberExpression GetMemberExpression<T>(Expression<Func<T, IComparable>> propertyExpression, bool throwError = true)
        {
            MemberExpression memberExpression = null;
            if (propertyExpression.Body is MemberExpression)
            {
                memberExpression = (MemberExpression)propertyExpression.Body;
            }
            else if (propertyExpression.Body is UnaryExpression)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                var operand = unaryExpression.Operand;
                if (operand is MemberExpression)
                {
                    memberExpression = (MemberExpression)operand;
                }
            }
            else if (throwError)
            {
                throw new ArgumentException("Could not convert expression to MemberExpression.");
            }

            return memberExpression;
        }

        public static MemberExpression GetMemberExpression<TContext, TProperty>(Expression<Func<TContext, TProperty>> propertyExpression, bool throwError = true)
        {
            MemberExpression memberExpression = null;
            if (propertyExpression.Body is MemberExpression)
            {
                memberExpression = (MemberExpression)propertyExpression.Body;
            }
            else if (propertyExpression.Body is UnaryExpression)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                var operand = unaryExpression.Operand;
                if (operand is MemberExpression)
                {
                    memberExpression = (MemberExpression)operand;
                }
            }
            else if (throwError)
            {
                throw new ArgumentException("Could not convert expression to MemberExpression.");
            }

            return memberExpression;
        }

        public static PropertyInfo GetPropertyInfoFromExpression<T>(Expression<Func<T, IComparable>> propertyExpression)
        {
            var memberExpression = Expressions.GetMemberExpression(propertyExpression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }
            return propertyInfo;
        }

        public static PropertyInfo GetPropertyInfoFromExpression<T>(Expression<Func<T, string>> propertyExpression)
        {
            var memberExpression = Expressions.GetMemberExpression(propertyExpression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }
            return propertyInfo;
        }

        public static PropertyInfo GetPropertyInfoFromExpression<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberExpression = Expressions.GetMemberExpression(propertyExpression);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }
            return propertyInfo;
        }
    }
}
