using NUnit.Framework;
using Simple.Validation.Tests.TestDomain;

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
            var validator = Properties
                .For<Employee>(e => e.Salary)
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
            var validator = Properties
                .For<Employee>(e => e.Salary)
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

    }
}
