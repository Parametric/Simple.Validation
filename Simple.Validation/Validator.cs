using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        private static readonly MethodInfo GenericValidateMethodInfo = typeof(Validator)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.IsGenericMethod)
            .First(m => m.Name == "Validate");

        public static IEnumerable<ValidationResult> Validate(Type validatorType, object value, params string[] rulesSets)
        {
            if (!validatorType.IsInstanceOfType(value))
            {
                var msg = string.Format("Parameter 'value' must be convertable to '{0}'", validatorType);
                throw new ArgumentOutOfRangeException(msg);
            }

            var genericMethod = GenericValidateMethodInfo.MakeGenericMethod(validatorType);
            var methodParameters = new[] {value, rulesSets};
            var objResults = genericMethod.Invoke(null, methodParameters);
            var results = objResults as IEnumerable<ValidationResult>;
            return results;
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
