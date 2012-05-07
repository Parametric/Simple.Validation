using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.DataModels;
using Personnel.Sample.Validators;

namespace Simple.Validation.Tests
{
    [TestFixture]
    public class ValidatorTests
    {
        [SetUp]
        public void BeforeEachTest()
        {
            Validator.SetValidatorProvider(new DefaultValidatorProvider());
        }

        [Test]
        public void Validate_should_initialize_with_DefaultValidatorProvider()
        {
            // Arrange
            var provider = Validator.ValidatorProvider;

            // Act

            // Assert
            Assert.That(provider, Is.InstanceOf<DefaultValidatorProvider>());
        }

        [Test]
        public void When_validatorProvider_is_set_to_null_should_return_DefaultValidationProvider()
        {
            // Arrange
            Validator.SetValidatorProvider(null);

            // Act
            var provider = Validator.ValidatorProvider;

            // Assert
            Assert.That(provider, Is.InstanceOf<DefaultValidatorProvider>());
        }

        [Test]
        public void When_validatorProvider_is_set_should_return_correct_instance_of_validator_provider()
        {
            // Arrange
            var expectedProvider = Substitute.For<IValidatorProvider>();
            Validator.SetValidatorProvider(expectedProvider);

            // Act
            var provider = Validator.ValidatorProvider;

            // Assert
            Assert.That(provider, Is.SameAs(expectedProvider));
        }

        [Test]
        public void Validate_should_get_validators()
        {
            // Arrange
            var stubProvider = Substitute.For<IValidatorProvider>();

            Validator.SetValidatorProvider(stubProvider);

            // Act
            Validator.Validate(new object());

            // Assert
            stubProvider.GetValidators<object>().Received(1);
        }

        [Test]
        public void Validate_should_return_accumulated_validation_results()
        {
            // Arrange
            var stubProvider = Substitute.For<IValidatorProvider>();
            var validator = Substitute.For<IValidator<object>>();
            stubProvider.GetValidators<object>().Returns(new []{validator});
            validator.AppliesTo(Arg.Any<string>()).Returns(true);
            validator.Validate(Arg.Any<object>()).Returns(new[]{new ValidationResult()
                                                         {
                                                             Message = "Test"
                                                         }});

            Validator.SetValidatorProvider(stubProvider);

            // Act
            var results = Validator.Validate(new object());

            // Assert
            Assert.That(results, Is.Not.Empty);
            Assert.That(results.First().Message, Is.EqualTo("Test"));
        }

        [Test]
        public void Enforce_NoValidationResults_ShouldNotThrow()
        {
            // Arrange
            var stubProvider = Substitute.For<IValidatorProvider>();
            var validator = Substitute.For<IValidator<object>>();
            stubProvider.GetValidators<object>().Returns(new[] { validator });
            validator.AppliesTo(Arg.Any<string>()).Returns(true);
            validator.Validate(Arg.Any<object>()).Returns(Enumerable.Empty<ValidationResult>());

            Validator.SetValidatorProvider(stubProvider);

            // Act
            Assert.DoesNotThrow(() => Validator.Enforce(new object()));

            // Assert
        }

        [Test]
        public void Enforce_Warnings_ShouldNotThrow()
        {
            // Arrange
            var stubProvider = Substitute.For<IValidatorProvider>();
            var validator = Substitute.For<IValidator<object>>();
            stubProvider.GetValidators<object>().Returns(new[] { validator });
            validator.AppliesTo(Arg.Any<string>()).Returns(true);
            validator.Validate(Arg.Any<object>())
                .Returns(new []{new ValidationResult()
                        {
                            Severity = ValidationResultSeverity.Warning,
                        }, });

            Validator.SetValidatorProvider(stubProvider);

            // Act
            IEnumerable<ValidationResult> results = null;
            Assert.DoesNotThrow(() => { results = Validator.Enforce(new object()); });

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.Not.Empty);
        }

        [Test]
        public void Enforce_Informational_ShouldNotThrow()
        {
            // Arrange
            var stubProvider = Substitute.For<IValidatorProvider>();
            var validator = Substitute.For<IValidator<object>>();
            stubProvider.GetValidators<object>().Returns(new[] { validator });
            validator.AppliesTo(Arg.Any<string>()).Returns(true);
            validator.Validate(Arg.Any<object>())
                .Returns(new[]{new ValidationResult()
                        {
                            Severity = ValidationResultSeverity.Informational,
                        }, });

            Validator.SetValidatorProvider(stubProvider);

            // Act
            IEnumerable<ValidationResult> results = null;
            Assert.DoesNotThrow(() => { results = Validator.Enforce(new object()); });

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.Not.Empty);
        }

        [Test]
        public void Enforce_Errors_ShouldThrow()
        {
            // Arrange
            var stubProvider = Substitute.For<IValidatorProvider>();
            var validator = Substitute.For<IValidator<object>>();
            stubProvider.GetValidators<object>().Returns(new[] { validator });
            validator.AppliesTo(Arg.Any<string>()).Returns(true);
            validator.Validate(Arg.Any<object>())
                .Returns(new[]{new ValidationResult()
                        {
                            Severity = ValidationResultSeverity.Error,
                        }, });

            Validator.SetValidatorProvider(stubProvider);

            // Act
            Assert.Throws<ValidationException>(() => Validator.Enforce(new object()));

            // Assert
        }

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

        [Test]
        public void Validate_NonGeneric_SameType()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveAddressValidator());

            Validator.SetValidatorProvider(validatorProvider);

            // Act
            var results = Validator.Validate(typeof(Employee), new Employee(), RulesSets.Crud.Save);

            // Assert
            Assert.That(results, Is.Not.Empty);
        }

        [Test]
        public void Validate_NonGeneric_SubType()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());

            Validator.SetValidatorProvider(validatorProvider);

            // Act
            var results = Validator.Validate(typeof(Employee), new Manager(), RulesSets.Crud.Save);

            // Assert
            Assert.That(results, Is.Not.Empty);
        }

        [Test]
        public void Validate_NonGeneric_TypesDontMatch()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => Validator.Validate(typeof(Manager), new Employee()));

            // Assert
        }

    }
}
