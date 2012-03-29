using System.Collections.Generic;

namespace Simple.Validation.Comparers
{
    public class EqualityComparerAdapter<T, TPropertyType>: IEqualityComparer<object>
    {
        private readonly IEqualityComparer<TPropertyType> _internalComparer;

        public new bool Equals(object x, object y)
        {
            return _internalComparer.Equals((TPropertyType)x, (TPropertyType)y);
        }

        public int GetHashCode(object obj)
        {
            return 0;
        }

        public EqualityComparerAdapter(IEqualityComparer<TPropertyType> internalComparer)
        {
            _internalComparer = internalComparer;
        }
    }
}