using System.Linq;
using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.DataModels;
using Simple.Validation.Validators;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class BooleanPropertyValidatorTests
    {
        [Test]
        [TestCase(true, true)] // <
        [TestCase(false, false)] // =
        public void IsTrue(bool value, bool isValid)
        {
            // Arrange
            var propertyName = "IsSalaried";
            var employee = new Employee()
                               {
                                   IsHourly = false,
                                   IsSalaried = value
                               };
            var validator = Properties<Employee>
                .For(e => e.IsSalaried)
                .If(e => !e.IsHourly)
                .IsTrue()
                ;


            // Act
            var results = validator.Validate(employee);
            // Assert
            
            if (isValid)
            {
                results.AssertValidFor(propertyName, null);
            }
            else
            {
                results.AssertInvalidFor(propertyName, null);
            }
        }

        [Test]
        [TestCase(true, false)] // <
        [TestCase(false, true)] // =
        public void IsFalse(bool value, bool isValid)
        {
            // Arrange
            var propertyName = "IsSalaried";
            var employee = new Employee()
            {
                IsHourly = true,
                IsSalaried = value
            };

            var validator = Properties<Employee>
                .For(e => e.IsSalaried)
                .If(e => e.IsHourly)
                .IsFalse()
                ;


            // Act
            var results = validator.Validate(employee);
            // Assert

            if (isValid)
            {
                results.AssertValidFor(propertyName, null);
            }
            else
            {
                results.AssertInvalidFor(propertyName, null);
            }
        }

    }
}