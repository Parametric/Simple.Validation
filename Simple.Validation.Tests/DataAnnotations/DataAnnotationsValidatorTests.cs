using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.DataModels;
using Simple.Validation.DataAnnotations;

namespace Simple.Validation.Tests.DataAnnotations
{
    [TestFixture]
    public class DataAnnotationsValidatorTests
    {

        [Test]
        public void ValidateAge()
        {
            // Arrange
            var employee = new Employee()
                               {
                                   FirstName = "Fred",
                                   LastName = "Flintstone",
                                   Age = 0,
                               };

            // Act
            var results = new DataAnnotationsValidator<Employee>()
                .Validate(employee);

            // Assert
            results.AssertInvalidFor("Age", typeof(ValidationAttribute));
            var result = results.Single(r => r.PropertyName == "Age");
            Assert.That(result.Context, Is.SameAs(employee));
        }
    }
}
