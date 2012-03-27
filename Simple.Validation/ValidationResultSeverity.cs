using System;

namespace Simple.Validation
{
    [Flags]
    public enum ValidationResultSeverity
    {
        Error = 1,
        Warning = 2,
        Informational = 4,
    }
}