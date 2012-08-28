using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simple.Validation
{
    public class DefaultValidationEngine : IValidationEngine
    {
        private readonly IValidatorProvider _validatorProvider;

        private static readonly MethodInfo GenericValidateMethodInfo = typeof(DefaultValidationEngine)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.IsGenericMethod)
            .First(m => m.Name == "Validate");

        public IEnumerable<ValidationResult> Validate(Type validatorType, object value, params string[] rulesSets)
        {
            if (!validatorType.IsInstanceOfType(value))
            {
                var msg = string.Format("Parameter 'value' must be convertable to '{0}'", validatorType);
                throw new ArgumentOutOfRangeException(msg);
            }

            var genericMethod = GenericValidateMethodInfo.MakeGenericMethod(validatorType);
            var methodParameters = new[] { value, rulesSets };
            var objResults = genericMethod.Invoke(this, methodParameters);
            var results = objResults as IEnumerable<ValidationResult>;
            return results;
        }

        public IEnumerable<ValidationResult> Validate<T>(T value, params string[] rulesSets)
        {
            if (rulesSets == null || !rulesSets.Any())
                rulesSets = new[] { "" };

            var allValidators = _validatorProvider.GetValidators<T>();

            var validators =
                from validator in allValidators
                from rulesSet in rulesSets
                where validator.AppliesTo(rulesSet)
                select validator;

            var validationResults = validators
                .SelectMany(v => v.Validate(value))
                .ToList()
                ;
            return validationResults;
        }

        public DefaultValidationEngine(IValidatorProvider validatorProvider)
        {
            _validatorProvider = validatorProvider;
        }
    }
}