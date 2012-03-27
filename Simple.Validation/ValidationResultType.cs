using System;

namespace Simple.Validation
{
    [Flags]
    public enum TextValidationResultType
    {
        TextLengthOutOfRange = 1,
        RequiredValueNotFound = 2,
        RegularExpressionMismatch = 4,
    }

    [Flags]
    public enum RangeValidationResultType
    {
        RequiredValueNotFound = 1,
        ValueOutOfRange = 2,
    }
}
