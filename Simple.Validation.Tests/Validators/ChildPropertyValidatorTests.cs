using System.Linq;
using NUnit.Framework;
using Simple.Validation.Tests.TestDomain;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class ChildPropertyValidatorTests
    {
        [Test]
        public void Required()
        {
            // Arrange
            var message = "ReportsTo is required.";
            var validator = Properties<Employee>
                .For(e => e.ReportsTo)
                .Required()
                .Message(message)
                ;

            // Act
            var employee = new Employee()
                               {
                                   ReportsTo = null,
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

        [Test]
        [Ignore]
        public void Cascade()
        {
            const string message = "ReportsTo is required.";
            var validator = Properties<Employee>
                .For(e => e.ReportsTo)
                .Cascade("Manager.Can.Have.Reports")
                .Message(message)
                ;

            // Act
            var employee = new Employee()
            {
                ReportsTo = new Manager(),
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
