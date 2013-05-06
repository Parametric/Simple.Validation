using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace Simple.Validation.Tests
{
    [TestFixture]
    public class AsyncValidationEngineTests
    {
       
        [Test]
        [TestCase(42)]
        [TestCase(null)]
        public void When_validating_then_the_item_is_validated(int? itemToValidate)
        {
            // Arrange
            var validator1 = Substitute.For<IValidator<int?>>();
            validator1.AppliesTo(null).ReturnsForAnyArgs(true);
            var validationResult1 = new ValidationResult { Message = "Message one" };
            validator1.Validate(itemToValidate).Returns(new[] { validationResult1 });

            var validator2 = Substitute.For<IValidator<int?>>();
            validator2.AppliesTo(null).ReturnsForAnyArgs(true);
            var validationResult2 = new ValidationResult { Message = "Message two" };
            validator2.Validate(itemToValidate).Returns(new[] { validationResult2 });

            var provider = Substitute.For<IValidatorProvider>();
            provider.GetValidators<int?>().Returns(new[] { validator1, validator2 });
            var engine = new AsyncValidationEngine(provider);
            
            // Act
            var results = engine.Validate(itemToValidate).ToArray();

            // Assert
            Assert.That(results,Is.EquivalentTo(new[]{validationResult1,validationResult2}));
        }

        [Test]
        [TestCase(42)]
        public void When_validating_with_type_then_the_item_is_validated(int? itemToValidate)
        {
            // Arrange
            var validator1 = Substitute.For<IValidator<int?>>();
            validator1.AppliesTo(null).ReturnsForAnyArgs(true);
            var validationResult1 = new ValidationResult { Message = "Message one" };
            validator1.Validate(itemToValidate).Returns(new[] { validationResult1 });

            var validator2 = Substitute.For<IValidator<int?>>();
            validator2.AppliesTo(null).ReturnsForAnyArgs(true);
            var validationResult2 = new ValidationResult { Message = "Message two" };
            validator2.Validate(itemToValidate).Returns(new[] { validationResult2 });

            var provider = Substitute.For<IValidatorProvider>();
            provider.GetValidators<int?>().Returns(new[] { validator1, validator2 });
            var engine = new AsyncValidationEngine(provider);

            // Act
            var results = engine.Validate(typeof(int?), itemToValidate).ToArray();

            // Assert
            Assert.That(results, Is.EquivalentTo(new[] { validationResult1, validationResult2 }));
        } 

    }
}