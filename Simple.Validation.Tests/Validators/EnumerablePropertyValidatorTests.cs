using System;
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
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveManagerValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Manager>
                .For(e => e.Reports)
                .Cascade("Save")
                ;

            // Act
            var manager = new Manager();
            var reports = Builder<Manager>
                .CreateListOfSize(1)
                .All()
                .Do(m => m.Age += 20)
                .Do(m => m.Address = Builder<Address>.CreateNew().Build())
                .Do(m => m.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                .Do(m => m.ReportsTo = manager)
                .Do(m => m.Reports = Builder<Employee>
                    .CreateListOfSize(10)
                    .All()
                    .Do(e => e.Address = Builder<Address>.CreateNew().Build())
                    .Do(e => e.ReportsTo = m)
                    .Do(e => e.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                    .Do(e => e.Age += 20)
                    .Build())
                .Build();

            manager.Reports = reports.Cast<Employee>().ToList();

            var results = validator.Validate(manager).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Cascade_Enumerable_InValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveManagerValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Manager>
                .For(e => e.Reports)
                .Cascade("Save")
                ;

            // Act
            var manager = new Manager();
            var reports = Builder<Manager>
                .CreateListOfSize(1)
                .All()
                //.Do(m => m.Age += 20)
                .Do(m => m.Address = Builder<Address>.CreateNew().Build())
                .Do(m => m.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                .Do(m => m.ReportsTo = manager)
                .Do(m => m.Reports = Builder<Employee>
                    .CreateListOfSize(10)
                    .All()
                    .Do(e => e.Address = Builder<Address>.CreateNew().Build())
                    .Do(e => e.ReportsTo = m)
                    .Do(e => e.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                    .Do(e => e.Age += 20)
                    .Build())
                .Build();

            manager.Reports = reports.Cast<Employee>().ToList();

            var results = validator.Validate(manager).ToList();

            // Assert

            // Assert
            Assert.That(results, Is.Not.Empty);
            for (var i = 0; i < manager.Reports.Count; i++)
            {
                var expectedPropertyName = string.Format("Reports[{0}].Age", i);
                var expectedResults = results.Where(vr => vr.PropertyName == expectedPropertyName).ToList();
                Assert.That(expectedResults, Is.Not.Empty);

                foreach (var expectedResult in expectedResults)
                {
                    Assert.That(expectedResult.Context == manager);
                }
            }
        }

        [Test]
        public void Cascade_WithGenericType_ValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveManagerValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Manager>
                .For(e => e.Reports)
                .Cascade<Employee>("Save")
                ;

            // Act
            var manager = new Manager();
            var reports = Builder<Manager>
                .CreateListOfSize(1)
                .All()
                .Do(m => m.Age += 20)
                .Do(m => m.Address = Builder<Address>.CreateNew().Build())
                .Do(m => m.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                .Do(m => m.ReportsTo = manager)
                .Do(m => m.Reports = Builder<Employee>.CreateListOfSize(10).Build())
                .Build();

            manager.Reports = reports.Cast<Employee>().ToList();

            var results = validator.Validate(manager).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Cascade_WithGenericType_InvalidValuesForSubType()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveManagerValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Manager>
                .For(e => e.Reports)
                .Cascade<Employee>("Save")
                ;

            // Act
            var manager = new Manager();
            var reports = Builder<Manager>
                .CreateListOfSize(1)
                .All()
                .Do(m => m.Age += 20)
                .Do(m => m.Address = Builder<Address>.CreateNew().Build())
                .Do(m => m.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                .Do(m => m.ReportsTo = manager)
                .Do(m => m.Reports = new List<Employee>())
                .Build();

            manager.Reports = reports.Cast<Employee>().ToList();

            var results = validator.Validate(manager).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }
        
        [Test]
        public void Cascade_WithGenericType_InValidValues()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            validatorProvider.RegisterValidator(new SaveManagerValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var validator = Properties<Manager>
                .For(e => e.Reports)
                .Cascade<Employee>("Save")
                ;

            // Act
            var manager = new Manager();
            var reports = Builder<Manager>
                .CreateListOfSize(1)
                .All()
                .Do(m => m.Age += 20)
                .Do(m => m.Address = Builder<Address>.CreateNew().Build())
                .Do(m => m.ContactInfo = Builder<ContactInfo>.CreateListOfSize(3).Build())
                .Do(m => m.ReportsTo = null)
                .Build();

            manager.Reports = reports.Cast<Employee>().ToList();

            var results = validator.Validate(manager).ToList();

            // Assert
            Assert.That(results, Is.Not.Empty);
            for (var i = 0; i < manager.Reports.Count; i++)
            {
                var expectedPropertyName = string.Format("Reports[{0}].ReportsTo", i);
                var expectedResults = results.Where(vr => vr.PropertyName == expectedPropertyName).ToList();
                Assert.That(expectedResults, Is.Not.Empty);

                foreach (var expectedResult in expectedResults)
                    Assert.That(expectedResult.Context == manager);
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


        [Test]
        public void If_WhenPredicateTrue_ShouldValidate()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .If(e => true)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = null,
            });

            // Assert
            Assert.That(results, Is.Not.Empty);
        }

        [Test]
        public void If_WhenPredicateFalse_ShouldNotValidate()
        {
            // Arrange
            var validator = Properties<Employee>
                .For(e => e.ContactInfo)
                .Required()
                .If(e => false)
                ;

            // Act
            var results = validator.Validate(new Employee()
            {
                ContactInfo = null,
            });

            // Assert
            Assert.That(results, Is.Empty);
        }
    }
}