using System.Linq;
using NUnit.Framework;
using Personnel.Sample;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class ChildPropertyValidatorTests
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
        [Ignore("Needs a non-generic implementation of Validator.Validate(). Also raises questions about which type to use for validation. The type of the property? Or the sub-type of the property value.")]
        public void CascadeRequiredProperty()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            const string message = "ReportsTo is required.";
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
                                  
                              }
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First();
            Assert.That(result.Context, Is.EqualTo(employee));
            Assert.That(result.PropertyName, Is.EqualTo("ReportsTo"));
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
            Assert.That(result.Type, Is.Null);
            Assert.That(result.Message, Is.EqualTo(message));
        }
    }
}
