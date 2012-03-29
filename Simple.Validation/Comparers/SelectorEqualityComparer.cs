using System;
using System.Collections.Generic;

namespace Simple.Validation.Comparers
{
    /// <summary>
    /// Equality comparer that accepts a selector delegate.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <remarks>
    /// This class was adapted from a blog by Paulo Morgado:
    /// http://weblogs.asp.net/paulomorgado/archive/2010/04/09/linq-enhancing-distinct-with-the-selectorequalitycomparer.aspx
    /// </remarks>
    public class SelectorEqualityComparer<TSource> : IEqualityComparer<TSource>
    {
        // ReSharper disable InconsistentNaming
        private readonly Func<TSource, IComparable> selector;
        // ReSharper restore InconsistentNaming

        public SelectorEqualityComparer(Func<TSource, IComparable> selector)
            : base()
        {
            this.selector = selector;
        }

        public bool Equals(TSource x, TSource y)
        {
            IComparable xKey = this.GetKey(x);
            IComparable yKey = this.GetKey(y);

            if (xKey != null)
            {
                return ((yKey != null) && xKey.CompareTo(yKey) == 0);
            }

            return (yKey == null);
        }

        public int GetHashCode(TSource obj)
        {
            return 0;
        }

        private IComparable GetKey(TSource obj)
        {
            return (obj == null) ? null : this.selector(obj);
        }
    }
}