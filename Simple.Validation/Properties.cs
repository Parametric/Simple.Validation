using System;
using System.Linq.Expressions;
using Simple.Validation.Validators;

namespace Simple.Validation
{
    public static class Properties
    {
        public static RangePropertyValidator<T> For<T>(Expression<Func<T, IComparable>> propertyExpression)
        {
            return new RangePropertyValidator<T>(propertyExpression);
        }

        public static StringPropertyValidator<T> For<T>(Expression<Func<T, string>> propertyExpression)
        {
            return new StringPropertyValidator<T>(propertyExpression);
        }
    }

    public static class Properties<T>
    {
        public static RangePropertyValidator<T> For(Expression<Func<T, IComparable>> propertyExpression)
        {
            return new RangePropertyValidator<T>(propertyExpression);
        }

        public static StringPropertyValidator<T> For(Expression<Func<T, string>> propertyExpression)
        {
            return new StringPropertyValidator<T>(propertyExpression);
        }

        public static ReferencePropertyValidator<T> For(Expression<Func<T, object>> propertyExpression)
        {
            return new ReferencePropertyValidator<T>(propertyExpression);
        }
    }
}