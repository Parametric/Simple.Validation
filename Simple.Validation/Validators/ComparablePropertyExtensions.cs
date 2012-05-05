using System;

namespace Simple.Validation.Validators
{
    public static class ComparablePropertyExtensions
    {
        private static bool IsValidMin(IComparable valueToValidate, IComparable minValue, bool lowerInclusive = true)
        {
            if (minValue == null)
                return true;

            var minCompareResult = minValue.CompareTo(valueToValidate);
            if (minCompareResult > 0)
                return false;

            if (!lowerInclusive && minValue.Equals(valueToValidate))
                return false;

            return true;
        }

        private static bool IsValidMax(IComparable valueToValidate, IComparable maxValue, bool upperInclusive = true)
        {
            if (maxValue == null)
                return true;

            var maxCompareResult = maxValue.CompareTo(valueToValidate);
            if (maxCompareResult < 0)
                return false;

            if (!upperInclusive && maxValue.Equals(valueToValidate))
                return false;

            return true;
        }

        public static PropertyValidator<TContext, IComparable> MinValue<TContext>(this PropertyValidator<TContext, IComparable> self, IComparable minValue, bool lowerInclusive = true)
        {
            self.Assert((t, p) => IsValidMin(p, minValue, lowerInclusive));
            return self;
        }

        public static PropertyValidator<TContext, IComparable> MaxValue<TContext>(this PropertyValidator<TContext, IComparable> self, IComparable maxValue, bool upperInclusive = true)
        {
            self.Assert((t, p) => IsValidMax(p, maxValue, upperInclusive));
            return self;
        }

        public static PropertyValidator<TContext, IComparable> GreaterThan<TContext>(this PropertyValidator<TContext, IComparable> self, IComparable minValue)
        {
            return self.MinValue(minValue, lowerInclusive: false);
        }

        public static PropertyValidator<TContext, IComparable> GreaterThanOrEqualTo<TContext>(this PropertyValidator<TContext, IComparable> self, IComparable minValue)
        {
            return self.MinValue(minValue, lowerInclusive: true);
        }

        public static PropertyValidator<TContext, IComparable> LessThan<TContext>(this PropertyValidator<TContext, IComparable> self,  IComparable maxValue)
        {
            return self.MaxValue(maxValue, upperInclusive: false);
        }

        public static PropertyValidator<TContext, IComparable> LessThanOrEqualTo<TContext>(this PropertyValidator<TContext, IComparable> self, IComparable maxValue)
        {
            return self.MaxValue(maxValue, upperInclusive: true);
        }        
    }
}