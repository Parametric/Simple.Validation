using System;

namespace Simple.Validation
{
    public class RangeRequirements<T> where T: struct, IComparable
    {
        public bool LowerInclusive { get; set; }
        public T? MinValue { get; set; }

        public T? MaxValue { get; set; }
        public bool UpperInclusive { get; set; }

        public RangeRequirements()
        {
            LowerInclusive = true;
            UpperInclusive = true;
        }

        internal bool IsValidMin(T? valueToValidate)
        {
            if (!MinValue.HasValue)
                return true;

            var compareValue = valueToValidate.GetValueOrDefault();
            var minCompareResult = MinValue.Value.CompareTo(compareValue);
            if (minCompareResult > 0)
                return false;
            
            if (!LowerInclusive && MinValue.Value.Equals(compareValue))
                return false;

            return true;
        }

        internal bool IsValidMax(T? valueToValidate)
        {
            if (!MaxValue.HasValue)
                return true;

            var compareValue = valueToValidate.GetValueOrDefault();
            var maxCompareResult = MaxValue.Value.CompareTo(compareValue);
            if (maxCompareResult < 0)
                return false;

            if (!UpperInclusive && MaxValue.Value.Equals(compareValue))
                return false;

            return true;
        }
    }
}
