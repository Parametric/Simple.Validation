using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Personnel.Sample.DataModels;
using Simple.Validation;

namespace Personnel.Sample.Integration.Tests
{
    [TestFixture]
    public class CascadeValidationTests
    {
        [SetUp]
        public void BeforeEachTestRuns()
        {
            this.ValidationEngine = Substitute.For<IValidationEngine>();

            this.ValidationEngine
                .When(svc => svc.Validate(typeof(Employee), Arg.Any<Employee>(), Arg.Any<string[]>()))
                .Do(callInfo =>
                {
                    var args = callInfo.Args().ToList();
                    var employee = args[1] as Employee;
                    Console.WriteLine("Validating: {0}, Data: {1}, {2}", args[0], employee.EmployeeId, args[2]);
                });
            ;
            Simple.Validation.Validator.SetValidationEngine(this.ValidationEngine);

            this.Validator = new CascadeValidator();
     
        }

        protected CascadeValidator Validator { get; set; }

        protected IValidationEngine ValidationEngine { get; set; }

        [Test]
        public void CascadeValidate()
        {
            // Arrange
            var manager = new Manager()
                              {
                                  Reports =
                                      {
                                          new Employee()
                                              {
                                                  EmployeeId = 1
                                              },
                                          new Employee()
                                              {
                                                  EmployeeId = 2
                                              },
                                          new Employee()
                                              {
                                                  EmployeeId = 3
                                              },
                                      }
                              };


            foreach (var dataRow in manager.Reports)
            {
                this.ValidationEngine.Validate(typeof(Employee), dataRow, Arg.Any<string[]>())
                    .Returns(new[]{new ValidationResult()
                                    {
                                        PropertyName = "Id",
                                        Context = dataRow,
                                        Message = string.Format("Item {0}", dataRow.EmployeeId)
                                    }, });


            }

            // Act
            var results = this.Validator.Validate(manager).ToList();

            // Assert
            Assert.That(results, Has.Count.EqualTo(3));

            foreach(var employee in manager.Reports)
            {
                this.ValidationEngine.Received().Validate(typeof(Employee), employee, Arg.Is<string[]>(arg => arg.Length == 0));
            }

            for (var i = 0; i < 3; i++)
            {
                var result = results[i];
                Assert.That(result.PropertyName, Is.EqualTo(string.Format("Reports[{0}].Id", i)));
            }

        }
    }
}
