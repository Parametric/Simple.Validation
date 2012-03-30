using Ninject;
using NSubstitute;
using NUnit.Framework;

namespace Simple.Validation.Ninject.Tests
{
    [TestFixture]
    public class NinjectValidatorProviderTests
    {
        [Test]
        public void GetValidators()
        {
            // Arrange
            var mockValidator = Substitute.For<IValidator<object>>();
            mockValidator.AppliesTo("").Returns(true);

            var kernel = new StandardKernel();
            kernel.Bind<IValidator<object>>().ToConstant(mockValidator);
            var provider = new NinjectValidatorProvider(kernel);

            

            // Act
            var validators = provider.GetValidators<object>();

            // Assert
            Assert.That(validators, Is.Not.Empty);
            Assert.That(validators, Has.Member(mockValidator));
        }

    }
}
