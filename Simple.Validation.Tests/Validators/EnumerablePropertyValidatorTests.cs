using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Personnel.Sample;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class EnumerablePropertyValidatorTests
    {
        [Test]
        public void Requried()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required();

            // Act
            var results = validator.Validate(new Employee()
                                                 {
                                                     ContactInfo = null,
                                                 });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
        }

        [Test]
        public void Size_Minimum_NotRequired_ValueNotSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Size(1)
                ;

            // Act
            var results = validator.Validate(new Employee()
                                                 {
                                                     ContactInfo = null,
                                                 });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Size_Minimum_NotRequired_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Size(1)
                ;

            // Act
            var results = validator.Validate(new Employee()
                                                 {
                                                     ContactInfo = new List<ContactInfo>(),
                                                 });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
        }

        [Test]
        public void Size_Minimum_Required_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Size(1)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>(),
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
        }

        [Test]
        public void Size_Minimum_NotRequired_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Size(1)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>()
                                  {
                                      new ContactInfo()
                                  },
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Size_Minimum_Required_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Size(1)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>()
                                  {
                                      new ContactInfo()
                                  },
            });

            // Assert
            Assert.That(results, Is.Empty);
        }




        [Test]
        public void Size_Maximum_NotRequired_ValueNotSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Size(1, 2)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = null,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Size_Maximum_NotRequired_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Size(1, 2)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>()
                                  {
                                      new ContactInfo(),
                                      new ContactInfo(),
                                      new ContactInfo(),
                                  },
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
        }

        [Test]
        public void Size_Maximum_Required_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Size(1, 2)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>()
                                  {
                                      new ContactInfo(),
                                      new ContactInfo(),
                                      new ContactInfo(),
                                  },
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Error));
        }

        [Test]
        public void Size_Maximum_NotRequired_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Size(1, 2)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>()
                                  {
                                      new ContactInfo(),
                                      new ContactInfo(),
                                  },
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Size_Maximum_Required_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Size(1, 2)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>()
                                  {
                                      new ContactInfo(),
                                      new ContactInfo(),
                                  },
            });

            // Assert
            Assert.That(results, Is.Empty);
        }


    }
}