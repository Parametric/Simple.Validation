using System;
using System.Collections.Generic;

namespace Simple.Validation
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationResult> ValidationResults { get; private set; }
        public object Context { get; private set; }
        public string[] RulesSets { get; private set; }

        public ValidationException(IEnumerable<ValidationResult> validationResults, object context, string[] rulesSets)
        {
            ValidationResults = validationResults;
            Context = context;
            RulesSets = rulesSets;
        }
    }
}
