using System.Linq;
using NUnit.Framework;

namespace Simple.Validation.Tests
{
    [TestFixture]
    public class StringValidatorTests
    {

        [Test]
        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(1, 2, true)]
                              
        [TestCase(0, null, true)]
        [TestCase(1, null, true)]

        [TestCase(null, 0, true)]
        [TestCase(null, 1, true)]
        [TestCase(null, null, true)]
        public void ValidateMinLength(int? minLength, int? actualLength, bool isValid)
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                MinLength = minLength,
            };

            string stringToValidate = null;
            if (actualLength.HasValue)
                stringToValidate = new string('x', actualLength.Value);

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, stringToValidate, propertyName);

            // Assert
            if (isValid)
            {
                results.AssertValidFor(propertyName, TextValidationResultType.TextLengthOutOfRange);
            }
            else
            {
                results.AssertInvalidFor(propertyName, TextValidationResultType.TextLengthOutOfRange);
            }
        }

        [Test]
        public void ValidateMinLength_IgnoreWhitespace()
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                MinLength = 5,
                IgnoreWhiteSpace = true,
            };

            string stringToValidate = "ABC  ";

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, stringToValidate, propertyName);

            // Assert

            results.AssertInvalidFor(propertyName, TextValidationResultType.TextLengthOutOfRange);

        }

        [Test]
        [TestCase(50, 0, true)]
        [TestCase(50, 1, true)]
        [TestCase(50, 2, true)]
        [TestCase(50, 25, true)]
        [TestCase(50, 50, true)]
        [TestCase(50, 51, false)]
        [TestCase(50, null, true)]
        [TestCase(null, 50, true)]
        [TestCase(null, 51, true)]
        [TestCase(null, null, true)]
        public void ValidateMaxLength(int? maxLength, int? actualLength, bool isValid)
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                MaxLength = maxLength,
            };

            string stringToValidate = null;
            if (actualLength.HasValue)
                stringToValidate = new string('x', actualLength.Value);

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, stringToValidate, propertyName);

            // Assert
            if (isValid)
            {
                results.AssertValidFor(propertyName, TextValidationResultType.TextLengthOutOfRange);
            }
            else
            {
                results.AssertInvalidFor(propertyName, TextValidationResultType.TextLengthOutOfRange);
            }
        }

        [Test]
        public void ValidateMaxLength_IgnoreWhitespace()
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                MaxLength = 3,
                IgnoreWhiteSpace = true,
            };

            string stringToValidate = "ABC  ";

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, stringToValidate, propertyName);

            // Assert

            results.AssertValidFor(propertyName, TextValidationResultType.TextLengthOutOfRange);

        }

        [Test]
        [TestCase(0, true, false)]
        [TestCase(1, true, true)]
        [TestCase(2, true, true)]
        [TestCase(null, true, false)]

        [TestCase(0, false, true)]
        [TestCase(1, false, true)]
        [TestCase(2, false, true)]
        [TestCase(null, false, true)]
        public void ValidateRequired(int? actualLength, bool required, bool isValid)
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                Required = required,
            };

            string stringToValidate = null;
            if (actualLength.HasValue)
                stringToValidate = new string('x', actualLength.Value);

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, stringToValidate, propertyName);

            // Assert
            if (isValid)
            {
                results.AssertValidFor(propertyName, TextValidationResultType.RequiredValueNotFound);
            }
            else
            {
                results.AssertInvalidFor(propertyName, TextValidationResultType.RequiredValueNotFound);
            }
        }

        [Test]
        public void ValidateRequired_IgnoreWhitespace()
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                Required = true,
                IgnoreWhiteSpace = true,
            };

            string stringToValidate = "     ";

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, stringToValidate, propertyName);

            // Assert

            results.AssertInvalidFor(propertyName, TextValidationResultType.RequiredValueNotFound);

        }

        [Test]
        [TestCase(@"\d{5}", "12345", true)]
        [TestCase(@"\d{5}", "ABCDE", false)]
        [TestCase(@"\d{5}", null, true)]
        public void ValidateRegEx(string regularExpression, string value, bool isValid)
        {
            // Arrange
            var requirements = new StringRequirements()
            {
                RegularExpression = regularExpression,
            };

            // Act
            var propertyName = "Test";
            var results = StringValidator.Validate(requirements, value, propertyName);

            // Assert
            if (isValid)
            {
                results.AssertValidFor(propertyName, TextValidationResultType.RegularExpressionMismatch);
            }
            else
            {
                results.AssertInvalidFor(propertyName, TextValidationResultType.RegularExpressionMismatch);
            }

        }
        private class TestClass
        {

            public string Name { get; set; }
        }

        [Test]
        public void ValidateUsingPropertyExpression()
        {
            // Arrange
            var instance = new TestClass();

            // Act
            var validationResults = StringValidator.Validate(new StringRequirements()
                                                                 {
                                                                     Required = true,
                                                                 }, instance, _ => instance.Name, "Required");
            
            // Assert
            Assert.That(validationResults.Any());
            var result = validationResults.First();
            Assert.That(result.PropertyName, Is.EqualTo("Name"));
            Assert.That(result.Context, Is.SameAs(instance));
            Assert.That(result.Message, Is.EqualTo("Required"));
        }
    }
}
