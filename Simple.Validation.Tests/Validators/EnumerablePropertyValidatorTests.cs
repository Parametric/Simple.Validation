using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.Comparers;
using Personnel.Sample.Validators;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class EnumerablePropertyValidatorTests
    {
        [Test]
        public void Required()
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
        public void Message()
        {
            // Arrange
            const string message = "ContactInfo is required.";
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Message(message)
                .Required();

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = null,
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result.Message, Is.EqualTo(message));
        }

        [Test]
        public void Severity()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Severity(ValidationResultSeverity.Warning)
                .Required();

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = null,
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result.Severity, Is.EqualTo(ValidationResultSeverity.Warning));            
        }

        [Test]
        public void Type()
        {
            // Arrange
            const string customType = "ContactInfo is required.";
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Type(customType)
                .Required();

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = null,
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result.Type, Is.EqualTo(customType));           
        }

        [Test]
        public void Count_Minimum_NotRequired_ValueNotSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Count(1)
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
        public void Count_Minimum_NotRequired_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Count(1)
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
        public void Count_Minimum_Required_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Count(1)
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
        public void Count_Minimum_NotRequired_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Count(1)
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
        public void Count_Minimum_Required_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Count(1)
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
        public void Count_Maximum_NotRequired_ValueNotSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Count(1, 2)
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
        public void Count_Maximum_NotRequired_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Count(1, 2)
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
        public void Count_Maximum_Required_InvalidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Count(1, 2)
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
        public void Count_Maximum_NotRequired_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Count(1, 2)
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
        public void Count_Maximum_Required_ValidValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Count(1, 2)
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
        public void Unique_NotRequired_ValueNotSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Unique()
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
        public void Unique_Required_EmptyValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique()
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = new List<ContactInfo>(),
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Unique_SingleValueSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique()
                ;

            // Act
            var contactInfo = Builder<ContactInfo>.CreateListOfSize(1).Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Unique_TwoValuesSpecified()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique()
                ;

            // Act
            var contactInfo = Builder<ContactInfo>.CreateListOfSize(2).Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Unique_SameValueSpecifiedTwice()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique()
                ;

            // Act
            var contactInfo = Builder<ContactInfo>.CreateListOfSize(1).Build();
            contactInfo.Add(contactInfo.First());
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Unique_ValidValues_CustomEqualityComparer()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique(new ContactInfoTypeEqualityComparer())
                ;

            // Act
            var contactInfo = Builder<ContactInfo>.CreateListOfSize(2).Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Unique_InvalidValues_CustomEqualityComparer()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique(new ContactInfoTypeEqualityComparer())
                ;

            // Act
            var contactInfo = Builder<ContactInfo>
                .CreateListOfSize(2)
                .All().Do(c => c.Type = "Email")
                .Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            }).ToList();

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Unique_ValidValues_ByPredicate()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique<ContactInfo>((left, right) => left.Type == right.Type)
                ;

            // Act
            var contactInfo = Builder<ContactInfo>.CreateListOfSize(2).Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Unique_InvalidValues_ByPredicate()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique<ContactInfo>((left, right) => left.Type == right.Type)
                ;

            // Act
            var contactInfo = Builder<ContactInfo>
                .CreateListOfSize(2)
                .All().Do(c => c.Type = "Email")
                .Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            }).ToList();

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Unique_ValidValues_BySelector()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique<ContactInfo>(c => c.Type)
                ;

            // Act
            var contactInfo = Builder<ContactInfo>.CreateListOfSize(2).Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Unique_InvalidValues_BySelector()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .Unique<ContactInfo>(c => c.Type)
                ;

            // Act
            var contactInfo = Builder<ContactInfo>
                .CreateListOfSize(2)
                .All().Do(c => c.Type = "Email")
                .Build();
            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo,
            }).ToList();

            // Assert
            Assert.That(results, Is.Not.Empty);
            var result = results.First(vr => vr.PropertyName == "ContactInfo");
            Assert.That(result, Is.Not.Null);
        }


        [Test]
        public void Cascade_Enumerable_ValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveContactInfoValidator());
            validatorProvider.RegisterValidator(new EmailAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Cascade("Save")
                ;

            // Act
            var contactInfo = Builder<EmailAddress>
                .CreateListOfSize(1)
                .All()
                .Do(c => c.Type = "Email")
                .Do(c => c.Text = "email@email.com")
                .Build();

            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo.Cast<ContactInfo>().ToList(),
            }).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Cascade_Enumerable_InValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveContactInfoValidator());
            validatorProvider.RegisterValidator(new EmailAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Cascade("Save")
                ;

            // Act
            var contactInfo = Builder<EmailAddress>
                .CreateListOfSize(1)
                .All()
                .Do(c => c.Type = "Email")
                .Do(c => c.Text = "invalid.emailaddress")
                .Build();
            var employee = new Employee()
                               {
                                   ContactInfo = contactInfo.Cast<ContactInfo>().ToList(),
                               };
            var results = validator.Validate(employee).ToList();

            // Assert
            Assert.That(results, Is.Not.Empty);
            for (var i = 0; i < contactInfo.Count; i++)
            {
                var expectedPropertyName = string.Format("ContactInfo[{0}].Text", i);
                var expectedResults = results.Where(vr => vr.PropertyName == expectedPropertyName).ToList();
                Assert.That(expectedResults, Is.Not.Empty);

                foreach (var expectedResult in expectedResults)
                {
                    Assert.That(expectedResult.Context == employee);
                }
            }
        }

        [Test]
        public void Cascade_WithGenericType_ValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveContactInfoValidator());
            validatorProvider.RegisterValidator(new EmailAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Cascade<ContactInfo>("Save")
                ;

            // Act
            var contactInfo = Builder<EmailAddress>
                .CreateListOfSize(1)
                .All()
                .Do(c => c.Type = "Email")
                .Do(c => c.Text = "email@email.com")
                .Build();

            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo.Cast<ContactInfo>().ToList(),
            }).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Cascade_WithGenericType_InvalidValuesForSubType()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveContactInfoValidator());
            validatorProvider.RegisterValidator(new EmailAddressValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Cascade<ContactInfo>("Save")
                ;

            // Act
            var contactInfo = Builder<EmailAddress>
                .CreateListOfSize(1)
                .All()
                .Do(c => c.Type = "Email")
                .Do(c => c.Text = "InvalidForEmail")
                .Build();

            var results = validator.Validate(new Employee()
            {
                ContactInfo = contactInfo.Cast<ContactInfo>().ToList(),
            }).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }
        
        [Test]
        public void Cascade_WithGenericType_InValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveContactInfoValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Cascade<ContactInfo>("Save")
                ;

            // Act
            var contactInfo = Builder<ContactInfo>
                .CreateListOfSize(2)
                .All().Do(c =>
                {
                    c.Type = "Email";
                    c.Text = string.Empty;
                })
                .Build();
            var employee = new Employee()
            {
                ContactInfo = contactInfo,
            };
            var results = validator.Validate(employee).ToList();

            // Assert
            Assert.That(results, Is.Not.Empty);
            for (var i = 0; i < contactInfo.Count; i++)
            {
                var expectedPropertyName = string.Format("ContactInfo[{0}].Text", i);
                var expectedResults = results.Where(vr => vr.PropertyName == expectedPropertyName).ToList();
                Assert.That(expectedResults, Is.Not.Empty);

                foreach (var expectedResult in expectedResults)
                {
                    Assert.That(expectedResult.Context == employee);
                }
            }
        }

        [Test]
        public void CascadeOverManager_Should_SetPropertyNamesCorrectlyForAllDescendants()
        {
            // Arrange
            var config = new ValidationConfiguration();
            config.Configure();

            var manager = new Manager()
                              {
                                  Reports = new List<Employee>()
                                      {
                                          new Employee
                                              {
                                                  ContactInfo = new[]
                                                                    {
                                                                        new ContactInfo(), 
                                                                    }
                                              },
                                      }
                              };

            // Act
            var results = Validator.Validate(manager, RulesSets.Crud.Save);

            // Assert
            var result = results.FirstOrDefault(vr => vr.PropertyName == "Reports[0].ContactInfo[0].Text");
            Assert.That(result, Is.Not.Null);
        }
    }
}