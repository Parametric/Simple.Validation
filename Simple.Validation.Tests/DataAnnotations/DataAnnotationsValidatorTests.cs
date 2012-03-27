using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using Simple.Validation.DataAnnotations;
using Simple.Validation.Tests.TestDomain;

namespace Simple.Validation.Tests.DataAnnotations
{
    [TestFixture]
    public class DataAnnotationsValidatorTests
    {
        [SetUp]
        public void BeforeEachTest()
        {
            var provider = new DefaultValidatorProvider();
            provider.RegisterValidator(new DataAnnotationsValidator());

            Validator.SetValidatorProvider(provider);
        }

        [Test]
        public void ValidateAge()
        {
            // Arrange
            var employee = new Employee();

            // Act
            var results = Validator.Validate(employee).ToList();

            // Assert
            results.AssertInvalidFor("Age", "DataAnnotationError");
            var result = results.Single(r => r.PropertyName == "Age");
            Assert.That(result.Context, Is.SameAs(employee));
        }
    }
}
