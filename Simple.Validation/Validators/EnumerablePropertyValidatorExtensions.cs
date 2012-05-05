using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Validation.Comparers;

namespace Simple.Validation.Validators
{
    public static class EnumerablePropertyValidatorExtensions
    {
        public static EnumerablePropertyValidator<T> Count<T>(this EnumerablePropertyValidator<T> self,  int? minCount, int? maxCount = null)
        {
            self.Count<T, object>(minCount, maxCount);
            return self;
        }

        public static PropertyValidator<TContext, IEnumerable<TElement>> Count<TContext, TElement>(this PropertyValidator<TContext, IEnumerable<TElement>> self, int? minCount, int? maxCount = null)
        {
            if (minCount.HasValue)
                self.Assert((t, list) => list.Count() >= minCount);

            if (maxCount.HasValue)
                self.Assert((t, list) => list.Count() <= maxCount);

            return self;
        }

        public static PropertyValidator<TContext, IEnumerable<TElement>> Unique<TContext, TElement>(this PropertyValidator<TContext, IEnumerable<TElement>> self)
        {
            return self.Unique((left, right) => left.Equals(right));
        }

        public static PropertyValidator<TContext, IEnumerable<TElement>> Unique<TContext, TElement>(this PropertyValidator<TContext, IEnumerable<TElement>> self, IEqualityComparer<TElement> comparer)
        {
            self.Assert((t, list) =>
            {
                var result = IsDistinct(list, comparer);
                return result;
            });

            return self;
        }

        private static bool IsDistinct<TElement>(IEnumerable<TElement> enumerable, IEqualityComparer<TElement> comparer)
        {
            bool result;
            if (comparer == null)
                result = enumerable.Count() == enumerable.Distinct().Count();
            else
                result = enumerable.Count() == enumerable.Distinct(comparer).Count();
            return result;
        }


        public static PropertyValidator<TContext, IEnumerable<TElement>> Unique<TContext, TElement>(this PropertyValidator<TContext, IEnumerable<TElement>> self,  Func<TElement, TElement, bool> predicate)
        {
            var comparer = new PredicateEqualityComparer<TElement>(predicate);
            return self.Unique(comparer);
        }

        public static PropertyValidator<TContext, IEnumerable<TElement>> Unique<TContext, TElement>(this PropertyValidator<TContext, IEnumerable<TElement>> self, Func<TElement, IComparable> selector)
        {
            var comparer = new SelectorEqualityComparer<TElement>(selector);
            return self.Unique(comparer);
        }


    }
}