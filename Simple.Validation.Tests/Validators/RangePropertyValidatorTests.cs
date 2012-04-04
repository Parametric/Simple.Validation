using System.Linq;
using NUnit.Framework;
using Personnel.Sample;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class RangePropertyValidatorTests
    {
        [Test]
        [TestCase(1.0, 0.0, true, false)] // <
        [TestCase(1.0, 1.0, true, true)] // =
        [TestCase(1.0, 1.0, false, false)] // ~=
        [TestCase(1.0, 1.01, false, true)] // >
        [TestCase(1.0, 2.0, true, true)]
        public void MinValue(double? minValue, double? valueToValidate, bool lowerInclusive, bool isValid)
        {
            // Arrange
            var propertyName = "Salary";
            var employee = new Employee()
                               {
                                   Salary = valueToValidate
                               };
            var validator = Properties<Employee>
                .For(e => e.Salary)
                ;

            if (minValue.HasValue)
                validator.MinValue(minValue.Value);

            if (lowerInclusive)
                validator.LowerInclusive();
            else
                validator.LowerExclusive();


            // Act
            var results = validator.Validate(employee);

            // Assert
            
            const RangeValidationResultType type = RangeValidationResultType.ValueOutOfRange;
            if (isValid)
            {
                results.AssertValidFor(propertyName, type);
            }
            else
            {
                results.AssertInvalidFor(propertyName, type);
            }
        }

        [Test]
        [TestCase(10.0, 9.0, true, true)]
        [TestCase(10.0, 10.0, true, true)]
        [TestCase(10.0, 10.0, false, false)]
        [TestCase(10.0, 10.01, true, false)]
        public void MaxValue(double? maxValue, double? valueToValidate, bool upperInclusive, bool isValid)
        {
            // Arrange
            var propertyName = "Salary";
            var employee = new Employee()
            {
                Salary = valueToValidate
            };
            var validator = Properties<Employee>
                .For(e => e.Salary)
                ;

            if (maxValue.HasValue)
                validator.MaxValue(maxValue.Value);

            if (upperInclusive)
                validator.UpperInclusive();
            else
                validator.UpperExclusive();


            // Act
            var results = validator.Validate(employee);
            // Assert
            const RangeValidationResultType type = RangeValidationResultType.ValueOutOfRange;
            if (isValid)
            {
                results.AssertValidFor(propertyName, type);
            }
            else
            {
                results.AssertInvalidFor(propertyName, type);
            }
        }

        [Test]
        public void Severity()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.Age)
                .GreaterThan(0)
                .Severity(ValidationResultSeverity.Warning);

            // Act
            var results = validator.Validate(new Employee());

            // Assert
            Assert.That(results, Is.Not.Empty);
            Assert.That(results.First().Severity, Is.EqualTo(ValidationResultSeverity.Warning));
        }

        [Test]
        public void Type()
        {
            // Arrange
            const string customType = "CustomType";
            var validator = Properties<Employee>
                .For(e => e.Age)
                .GreaterThanOrEqualTo(18)
                .Type(customType);

            // Act
            var results = validator.Validate(new Employee());

            // Assert
            Assert.That(results, Is.Not.Empty);
            Assert.That(results.First().Type, Is.EqualTo(customType));
        }

        [Test]
        public void Message()
        {
            // Arrange
            const string customMessage = "CustomMessage";
            var validator = Properties<Employee>
                .For(e => e.Age)
                .GreaterThanOrEqualTo(18)
                .Message(customMessage);

            // Act
            var results = validator.Validate(new Employee());

            // Assert
            Assert.That(results, Is.Not.Empty);
            Assert.That(results.First().Message, Is.EqualTo(customMessage));
        }

        [Test]
        public void If_PredicateIsTrue_ShouldValidate()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.Age)
                .GreaterThanOrEqualTo(18)
                .If(e => e.Age != -1)
                ;

            // Act
            var employee = new Employee()
            {
                Age = 15
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Not.Empty);
        }

        [Test]
        public void If_PredicateIsFalse_ShouldNotValidate()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.Age)
                .GreaterThanOrEqualTo(18)
                .If(e => e.Age != -1)
                ;

            // Act
            var employee = new Employee()
            {
                Age = -1
            };
            var results = validator.Validate(employee);

            // Assert
            Assert.That(results, Is.Empty);

        }

    }
}
