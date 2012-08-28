using System.Collections.Generic;
using System.Linq;

namespace Simple.Validation
{
    public static class ValidationEngineExtensions
    {
        public static IEnumerable<ValidationResult> Enforce<T>(this IValidationEngine self, T value, params string[] rulesSets)
        {
            var results = self.Validate(value, rulesSets);
            if (results.HasErrors())
                throw new ValidationException(results.ToArray(), value, rulesSets);
            return results;
        } 
    }
}