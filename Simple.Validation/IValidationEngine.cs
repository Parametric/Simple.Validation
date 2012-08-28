using System;
using System.Collections.Generic;

namespace Simple.Validation
{
    public interface IValidationEngine
    {
        IEnumerable<ValidationResult> Validate(Type validatorType, object value, params string[] rulesSets);
        IEnumerable<ValidationResult> Validate<T>(T value, params string[] rulesSets);
    }
}