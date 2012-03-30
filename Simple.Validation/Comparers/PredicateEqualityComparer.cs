using System;
using System.Collections.Generic;

namespace Simple.Validation.Comparers
{
    /// <summary>
    /// Equality comparer that accepts a function delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Adapted from this blog by Paulo Morado:
    /// http://msmvps.com/blogs/paulomorgado/archive/2010/04/08/linq-enhancing-distinct-with-the-predicateequalitycomparer.aspx
    /// </remarks>
    public class PredicateEqualityComparer<T> : IEqualityComparer<T>
    {
        // ReSharper disable InconsistentNaming
        private readonly Func<T, T, bool> predicate;
        // ReSharper restore InconsistentNaming

        public PredicateEqualityComparer(Func<T, T, bool> predicate)
            : base()
        {
            this.predicate = predicate;
        }

        public bool Equals(T x, T y)
        {
            if (x != null)
            {
                return ((y != null) && this.predicate(x, y));
            }

            if (y != null)
            {
                return false;
            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}
