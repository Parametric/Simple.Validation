using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.Validators;
using Simple.Validation.Validators;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class AncestorTypeValidatorTests
    {
        [Test]
        [TestCase("RulesSet1", "RulesSet1", true)]
        [TestCase("RulesSet1", "RulesSet2", false)]
        public void AppliesTo(string rulesSet, string appliesToRulesSet, bool expectedResult)
        {
            // Arrange
            var validator = new AncestorTypeValidator<Manager, Employee>(rulesSet);

            // Act
            var result = validator.AppliesTo(appliesToRulesSet);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Validate()
        {
            // Arrange
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.RegisterValidator(new SaveEmployeeValidator());
            Validator.SetValidatorProvider(validatorProvider);

            var manager = new Manager();

            // Act
            var validator = new AncestorTypeValidator<Manager, Employee>(RulesSets.Crud.Save);
            var results = validator.Validate(manager);

            // Assert
            Assert.That(results, Is.Not.Empty);
        }

    }
}
