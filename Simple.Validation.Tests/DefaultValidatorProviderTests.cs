using NSubstitute;
using NUnit.Framework;

namespace Simple.Validation.Tests
{
    [TestFixture]
    public class DefaultValidatorProviderTests
    {
        [Test]
        public void When_appliesTo_is_true_then_validator_is_executed()
        {
            // Arrange
            var mockValidator = Substitute.For<IValidator<object>>();
            mockValidator.AppliesTo("").Returns(true);

            var provider = new DefaultValidatorProvider();
            provider.RegisterValidator(mockValidator);

            Validator.SetValidatorProvider(provider);

            // Act
            var objectToValidate = new object();
            Validator.Validate(objectToValidate);

            // Assert
            mockValidator.Received().Validate(objectToValidate);
        }

        [Test]
        public void When_appliesTo_is_false_then_validator_is_not_executed()
        {
            // Arrange
            var mockValidator = Substitute.For<IValidator<object>>();
            mockValidator.AppliesTo("").Returns(false);

            var provider = new DefaultValidatorProvider();
            provider.RegisterValidator(mockValidator);

            Validator.SetValidatorProvider(provider);

            // Act
            var objectToValidate = new object();
            Validator.Validate(objectToValidate);

            // Assert
            mockValidator.DidNotReceive().Validate(objectToValidate);
        }
    }
}
