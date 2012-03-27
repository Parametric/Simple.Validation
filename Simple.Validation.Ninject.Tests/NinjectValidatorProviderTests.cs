using NSubstitute;
using NUnit.Framework;
using Ninject;

namespace Simple.Validation.Ninject.Tests
{
    [TestFixture]
    public class NinjectValidatorProviderTests
    {
        [Test]
        public void When_appliesTo_is_true_then_validator_is_executed()
        {
            // Arrange
            var mockValidator = Substitute.For<IValidator<object>>();
            mockValidator.AppliesTo("").Returns(true);

            var kernel = new StandardKernel();
            kernel.Bind<IValidator<object>>().ToConstant(mockValidator);
            var provider = new NinjectValidatorProvider(kernel);

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

            var kernel = new StandardKernel();
            kernel.Bind<IValidator<object>>().ToConstant(mockValidator);
            var provider = new NinjectValidatorProvider(kernel);

            Validator.SetValidatorProvider(provider);

            // Act
            var objectToValidate = new object();
            Validator.Validate(objectToValidate);

            // Assert
            mockValidator.DidNotReceive().Validate(objectToValidate);
        }

    }
}
