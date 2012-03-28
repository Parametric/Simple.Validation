using NSubstitute;
using NUnit.Framework;

namespace Simple.Validation.Tests
{
    [TestFixture]
    public class DefaultValidatorProviderTests
    {
        [Test]
        public void GetValidators()
        {
            // Arrange
            var mockValidator = Substitute.For<IValidator<object>>();

            var provider = new DefaultValidatorProvider();
            provider.RegisterValidator(mockValidator);


            // Act
            var validators = provider.GetValidators<object>();

            // Assert
            Assert.That(validators, Is.Not.Empty);
            Assert.That(validators, Has.Member(mockValidator));
        }

    }
}
