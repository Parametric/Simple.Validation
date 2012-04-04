using System;
using System.Linq;
using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.Validators;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class ReferencePropertyValidatorTests
    {
        [Test]
        public void WhenRequiredAndPropertyNotSetShouldFail()
        {
            // Arrange
            const string message = "ReportsTo is required.";
            var validator = Properties<Employee>
                .For(e => e.Address  )
                .Required()
                .Message(message)
                ;

            // Act
            var employee = new Employee()
                               {
                                   Address = null,
                               };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First();
            Assert.That(result.Context, Is.EqualTo(employee));
            Assert.That(result.PropertyName, Is.EqualTo("Address"));
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
            Assert.That(result.Type, Is.Null);
            Assert.That(result.Message, Is.EqualTo(message));
        }

        [Test]
        public void Severity()
        {
            // Arrange
            const string message = "ReportsTo is required.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Severity(ValidationResultSeverity.Warning)
                ;

            // Act
            var employee = new Employee()
            {
                Address = null,
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(v => v.PropertyName == "Address");
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Warning));
        }

        [Test]
        public void Type()
        {
            // Arrange
            const string customValidationResultType = "CustomValidationResultType";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Type(customValidationResultType)
                ;

            // Act
            var employee = new Employee()
            {
                Address = null,
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(v => v.PropertyName == "Address");
            Assert.That(result.Type, Is.EqualTo(customValidationResultType));
        }

        [Test]
        public void WhenNotRequiredAndPropertyNotSetShouldSucceed()
        {
            // Arrange
            const string message = "ReportsTo is required.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .NotRequired()
                .Message(message)
                ;

            // Act
            var employee = new Employee()
            {
                Address = null,
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void WhenRequiredAndPropertySetShouldSucceed()
        {
            // Arrange
            const string message = "ReportsTo is required.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Message(message)
                ;

            // Act
            var employee = new Employee()
            {
                Address = new Address(),
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void CascadeRequiredProperty()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Cascade("Save")
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
                Address = new Address()
                              {
                                  Line1 = null,
                              }
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First();
            Assert.That(result.Context, Is.EqualTo(employee));
            Assert.That(result.PropertyName, Is.EqualTo("Address.Line1"));
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
            Assert.That(result.Type, Is.EqualTo(TextValidationResultType.RequiredValueNotFound));
        }

        [Test]
        public void WhenNotRequired_AndCascading_AndPropertyIsEmpty_ShouldSucceed()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.Address)
                .NotRequired()
                .Cascade("Save")
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
                Address = null
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Empty);
        }
        
        [Test]
        public void WhenCascading_Message_DoesNotOverrideMessagesFromPropertyValidation()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var message = "This is a test failure.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Cascade("Save")
                .Message(message)
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
                Address = new Address()
                {
                    Line1 = null,
                }
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "Address.Line1");
            Assert.That(result.Message, Is.Not.EqualTo(message));
        }

        [Test]
        public void CascadeUsingType()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var message = "This is a test failure.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required()
                .Cascade<object>("Save")
                .Message(message)
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
                Address = new Address()
                {
                    Line1 = null,
                }
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Cascade_WithIncompatibleType()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var message = "This is a test failure.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Required();

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => validator.Cascade<ContactInfo>("Save"));
        }

        [Test]
        public void If_WhenPredicateTrue_ShouldValidate()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var acceptableLine1 = "This is a test of the emergency broadcast system.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Cascade("Save")
                .If(e => e.Address.Line1 != acceptableLine1)
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
                Address = new Address()
                {
                    Line1 = null,
                }
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First();
            Assert.That(result.Context, Is.EqualTo(employee));
            Assert.That(result.PropertyName, Is.EqualTo("Address.Line1"));
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
            Assert.That(result.Type, Is.EqualTo(TextValidationResultType.RequiredValueNotFound));
        }

        [Test]
        public void If_WhenPredicateFalse_ShouldNotValidate()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var acceptableLine1 = "This is a test of the emergency broadcast system.";
            var validator = Properties<Employee>
                .For(e => e.Address)
                .Cascade("Save")
                .If(e => e.Address.Line1 != acceptableLine1)
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
                Address = new Address()
                {
                    Line1 = acceptableLine1,
                }
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Empty);
        }

    }
}
