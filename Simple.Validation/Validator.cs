using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simple.Validation
{
    public static class Validator
    {
        public static IValidatorProvider ValidatorProvider { get; private set; }

        public static IValidationEngine ValidationEngine { get; private set; }

        public static void SetValidationEngine(IValidationEngine validationEngine)
        {
            if (validationEngine == null)
            {
                UseDefaultValidatorProvider();
                return;
            }

            ValidationEngine = validationEngine;
        }

        [Obsolete("Use SetValidationEngine(new DefaultValidationEngine(my custom validator provider)) instead.")]
        public static void SetValidatorProvider(IValidatorProvider validatorProvider)
        {
            if (validatorProvider == null)
            {
                UseDefaultValidatorProvider();
                return;
            }

            ValidatorProvider = validatorProvider;
            SetValidationEngine(new DefaultValidationEngine(validatorProvider));
        }

        public static IEnumerable<ValidationResult> Validate(Type validatorType, object value, params string[] rulesSets)
        {
            return ValidationEngine.Validate(validatorType, value, rulesSets);
        } 

        public static IEnumerable<ValidationResult> Validate<T>(T value, params string[] rulesSets)
        {
            return ValidationEngine.Validate(value, rulesSets);
        }

        public static IEnumerable<ValidationResult> Enforce<T>(T value, params string[] rulesSets)
        {
            return ValidationEngine.Enforce(value, rulesSets);
        }

        static Validator()
        {
            UseDefaultValidatorProvider();
        }

        private static void UseDefaultValidatorProvider()
        {
            ValidatorProvider = new DefaultValidatorProvider();
            SetValidationEngine(new DefaultValidationEngine(ValidatorProvider));
        }

    }
}
