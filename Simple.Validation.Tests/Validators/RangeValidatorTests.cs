using System.Linq;
using NUnit.Framework;
using Simple.Validation.Validators;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class RangeValidatorTests
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
            var requirements = new RangeRequirements()
                                    {
                                        LowerInclusive = lowerInclusive,
                                        MinValue = minValue,
                                    };

            // Act
            const string propertyName = "Test";
            var results = RangeValidator.Validate(requirements, valueToValidate, propertyName);

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
            var requirements = new RangeRequirements()
            {
                UpperInclusive = upperInclusive,
                MaxValue = maxValue,
            };

            // Act
            const string propertyName = "Test";
            var results = RangeValidator.Validate(requirements, valueToValidate, propertyName).ToList();

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

        private class TestClass
        {
            public int? Amount { get; set; }
            public int Value { get; set; }
        }

        [Test]
        public void ValidateUsingPropertyExpressionOnNullableType()
        {
            // Arrange
            var instance = new TestClass();

            // Act
            var validationResults = RangeValidator.Validate(new RangeRequirements()
            {
                MinValue = 5
            }, instance, _ => instance.Amount, "Required");

            // Assert
            Assert.That(validationResults.Any());
            var result = validationResults.First();
            Assert.That(result.PropertyName, Is.EqualTo("Amount"));
            Assert.That(result.Context, Is.SameAs(instance));
            Assert.That(result.Message, Is.EqualTo("Required"));
        }

        [Test]
        public void ValidateUsingPropertyExpression()
        {
            // Arrange
            var instance = new TestClass();

            // Act
            var validationResults = RangeValidator.Validate(new RangeRequirements()
            {
                MinValue = 5
            }, instance, _ => instance.Value, "Required");

            // Assert
            Assert.That(validationResults.Any());
            var result = validationResults.First();
            Assert.That(result.PropertyName, Is.EqualTo("Value"));
            Assert.That(result.Context, Is.SameAs(instance));
            Assert.That(result.Message, Is.EqualTo("Required"));
        }

    }
}
