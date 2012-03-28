using System;

namespace Simple.Validation.Validators
{
    public class RangeRequirements
    {
        public bool LowerInclusive { get; set; }
        public IComparable MinValue { get; set; }   

        public IComparable MaxValue { get; set; }
        public bool UpperInclusive { get; set; }

        public RangeRequirements()
        {
            LowerInclusive = true;
            UpperInclusive = true;
        }

        internal bool IsValidMin(IComparable valueToValidate)
        {
            if (MinValue == null)
                return true;

            var minCompareResult = MinValue.CompareTo(valueToValidate);
            if (minCompareResult > 0)
                return false;
            
            if (!LowerInclusive && MinValue.Equals(valueToValidate))
                return false;

            return true;
        }

        internal bool IsValidMax(IComparable valueToValidate)
        {
            if (MaxValue == null)
                return true;
            
            var maxCompareResult = MaxValue.CompareTo(valueToValidate);
            if (maxCompareResult < 0)
                return false;

            if (!UpperInclusive && MaxValue.Equals(valueToValidate))
                return false;

            return true;
        }
    }
}
