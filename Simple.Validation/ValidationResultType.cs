using System;

namespace Simple.Validation
{
    [Flags]
    public enum RangeValidationResultType
    {
        RequiredValueNotFound = 1,
        ValueOutOfRange = 2,
    }
}
