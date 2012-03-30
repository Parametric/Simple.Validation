using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation
{
    public static class ValidationResultExtensions
    {
        public static bool HasErrors(this IEnumerable<ValidationResult> self)
        {
            return self.Any(v => v.Severity == ValidationResultSeverity.Error);
        }

        public static bool HasWarnings(this IEnumerable<ValidationResult>  self)
        {
            return self.Any(v => v.Severity == ValidationResultSeverity.Warning);
        }
    }
}