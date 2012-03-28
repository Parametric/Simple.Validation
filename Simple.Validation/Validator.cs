using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Simple.Validation
{
    public static class Validator
    {
        private static IValidatorProvider _validatorProvider;
        public static IValidatorProvider ValidatorProvider
        {
            get { return _validatorProvider; }
        }

        public static void SetValidatorProvider(IValidatorProvider validatorProvider)
        {
            if (validatorProvider == null)
            {
                UseDefaultValidatorProvider();
                return;
            }

            _validatorProvider = validatorProvider;
        }

        public static IEnumerable<ValidationResult> Validate<T>(T value, params string[] rulesSets)
        {
            if (rulesSets == null || !rulesSets.Any())
                rulesSets = new[]{""};

            var validators = 
                    from validator in ValidatorProvider.GetValidators<T>()
                    from rulesSet in rulesSets
                    where validator.AppliesTo(rulesSet)
                    select validator;

            var validationResults = validators
                .SelectMany(v => v.Validate(value))
                .ToList()
                ;
            return validationResults;
        }

        public static IEnumerable<ValidationResult> Enforce<T>(T value, params string[] rulesSets)
        {
            var results = Validate(value, rulesSets);
            if (results.HasErrors())
                throw new ValidationException(results.ToArray(), value, rulesSets);
            return results;
        }

        static Validator()
        {
            UseDefaultValidatorProvider();
        }

        private static void UseDefaultValidatorProvider()
        {
            _validatorProvider = new DefaultValidatorProvider();
        }

    }
}
