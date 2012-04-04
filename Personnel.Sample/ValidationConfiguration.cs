using Personnel.Sample.Validators;
using Simple.Validation;

namespace Personnel.Sample
{
    public class ValidationConfiguration
    {
        public void Configure()
        {
            var validatorProvider = CreateValidatorProvider();
            RegisterValidators(validatorProvider);
            Validator.SetValidatorProvider(validatorProvider);
        }

        private static void RegisterValidators(DefaultValidatorProvider validatorProvider)
        {
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            validatorProvider.RegisterValidator(new SaveContactInfoValidator());
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveManagerValidator());
        }

        private static DefaultValidatorProvider CreateValidatorProvider()
        {
            var validatorProvider = new DefaultValidatorProvider();
            return validatorProvider;
        }
    }
}
