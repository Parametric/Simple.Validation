using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation
{
    public class ValidationResult
    {
        public string PropertyName { get; set; }

        public object Type { get; set; }

        public ValidationResultSeverity Severity { get; set; }

        public string Message { get; set; }

        public object Context { get; set; }

        public ValidationResult()
        {
            this.Severity = ValidationResultSeverity.Error;
        }
    }

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
