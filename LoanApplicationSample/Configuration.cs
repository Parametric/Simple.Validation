using Simple.Validation;

namespace LoanApplicationSample
{
    public class Configuration
    {
        public void ConfigureValidation()
        {
            var validatorProvider = CreateValidatorProvider();
            RegisterValidators(validatorProvider);
            Validator.SetValidatorProvider(validatorProvider);
        }

        private static void RegisterValidators(DefaultValidatorProvider validatorProvider)
        {
            validatorProvider.RegisterValidator(new SaveLoanApplicationValidator());
            validatorProvider.RegisterValidator(new SubmitLoanApplicationValidator());
        }

        private static DefaultValidatorProvider CreateValidatorProvider()
        {
            var validatorProvider = new DefaultValidatorProvider();
            return validatorProvider;
        }
    }
}
