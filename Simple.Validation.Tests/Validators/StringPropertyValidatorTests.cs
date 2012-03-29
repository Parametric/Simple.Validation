﻿using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Personnel.Sample;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class StringPropertyValidatorTests
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
            var employee = new Employee();
            if (actualLength.HasValue)
                employee.LastName = new string('x', actualLength.Value);

            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Length(minLength);

            // Act
            var results = validator.Validate(employee);

            // Assert
            if (isValid)
            {
                results.AssertValidFor("LastName", TextValidationResultType.TextLengthOutOfRange);
            }
            else
            {
                results.AssertInvalidFor("LastName", TextValidationResultType.TextLengthOutOfRange);
            }
        }

        [Test]
        public void ValidateMinLength_IgnoreWhitespace()
        {
            // Arrange
            var employee = new Employee {LastName = "ABC  "};

            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Length(5)
                .IgnoreWhiteSpace()
                ;


            // Act
            var results = validator.Validate(employee);

            // Assert

            results.AssertInvalidFor("LastName", TextValidationResultType.TextLengthOutOfRange);

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
            // Arrange
            var employee = new Employee();
            if (actualLength.HasValue)
                employee.LastName = new string('x', actualLength.Value);

            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Length(null, maxLength);

            // Act
            var results = validator.Validate(employee);

            // Assert
            if (isValid)
            {
                results.AssertValidFor("LastName", TextValidationResultType.TextLengthOutOfRange);
            }
            else
            {
                results.AssertInvalidFor("LastName", TextValidationResultType.TextLengthOutOfRange);
            }
        }

        [Test]
        public void ValidateMaxLength_IgnoreWhitespace()
        {
            // Arrange
            var employee = new Employee { LastName = "ABCD  " };

            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Length(null, 3)
                .IgnoreWhiteSpace()
                ;


            // Act
            var results = validator.Validate(employee);

            // Assert

            results.AssertInvalidFor("LastName", TextValidationResultType.TextLengthOutOfRange);

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
            var employee = new Employee();
            var validator = Properties<Employee>
                .For(e => e.LastName)
                ;

            if (required)
                validator.Required();
            else
                validator.NotRequired();

            if (actualLength.HasValue)
                employee.LastName = new string('x', actualLength.Value);

            // Act
            var results = validator.Validate(employee);

            // Assert
            if (isValid)
            {
                results.AssertValidFor("LastName", TextValidationResultType.RequiredValueNotFound);
            }
            else
            {
                results.AssertInvalidFor("LastName", TextValidationResultType.RequiredValueNotFound);
            }
        }

        [Test]
        public void ValidateRequired_IgnoreWhitespace()
        {
            // Arrange
            var employee = new Employee();
            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Required()
                .IgnoreWhiteSpace()
                ;

            employee.LastName  = "     ";

            // Act
            var results = validator.Validate(employee);

            // Assert

            results.AssertInvalidFor("LastName", TextValidationResultType.RequiredValueNotFound);

        }

        [Test]
        [TestCase(@"\d{5}", "12345", true)]
        [TestCase(@"\d{5}", "ABCDE", false)]
        [TestCase(@"\d{5}", null, true)]
        public void ValidateRegEx(string regularExpression, string value, bool isValid)
        {
            // Arrange
            var employee = new Employee()
                               {
                                   LastName = value
                               };
            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Matches(regularExpression)
                ;

            // Act
            var results = validator.Validate(employee);

            // Assert
            if (isValid)
            {
                results.AssertValidFor("LastName", TextValidationResultType.RegularExpressionMismatch);
            }
            else
            {
                results.AssertInvalidFor("LastName", TextValidationResultType.RegularExpressionMismatch);
            }

        }

        [Test]
        [TestCase(@"\d{5}", "12345", RegexOptions.None,  true)]
        [TestCase(@" \d{5} ", "12345", RegexOptions.IgnorePatternWhitespace, true)]
        [TestCase(@" \d{5} ", "12345", RegexOptions.None, false)]
        public void RegularExpressionWithOptions(string regularExpression, string value,RegexOptions options, bool isValid)
        {
            // Arrange
            var employee = new Employee()
            {
                LastName = value
            };
            var validator = Properties<Employee>
                .For(e => e.LastName)
                .Matches(regularExpression, options)
                ;

            // Act
            var results = validator.Validate(employee);

            // Assert
            if (isValid)
            {
                results.AssertValidFor("LastName", TextValidationResultType.RegularExpressionMismatch);
            }
            else
            {
                results.AssertInvalidFor("LastName", TextValidationResultType.RegularExpressionMismatch);
            }
        }

        [Test]
        public void Severity()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.FirstName)
                .Required()
                .Severity(ValidationResultSeverity.Warning);

            // Act
            var results = validator.Validate(new Employee());

            // Assert
            Assert.That(results, Is.Not.Empty);
            Assert.That(results.First().Severity, Is.EqualTo(ValidationResultSeverity.Warning));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void IsTrue(bool result, bool isValid)
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.FirstName)
                .IsTrue(_ => result);

            // Act
            var results = validator.Validate(new Employee()
                                                 {
                                                     FirstName = "Some Non-null value"
                                                 });

            // Assert
            if (isValid)
                Assert.That(results, Is.Empty);
            else
                Assert.That(results, Is.Not.Empty);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IsFalse(bool result, bool isValid)
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.FirstName)
                .IsFalse(_ => result);

            // Act
            var results = validator.Validate(new Employee()
                                                 {
                                                     FirstName = "Some non-null value"
                                                 });

            // Assert
            if (isValid)
                Assert.That(results, Is.Empty);
            else
                Assert.That(results, Is.Not.Empty);
        }

    }
}
