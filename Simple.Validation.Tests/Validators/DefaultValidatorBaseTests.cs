using NUnit.Framework;
using Personnel.Sample;
using Personnel.Sample.Validators;

namespace Simple.Validation.Tests.Validators
{
    [TestFixture]
    public class DefaultValidatorBaseTests
    {
        [Test]
        public void AppliesTo_WhenRulesSetIsContained_ShouldReturnTrue()
        {
            // Arrange
            var validator = new RulesSetEmployeeValidator();
            validator.RulesSets = new string[]{"Test"};

            // Act
            var result = validator.AppliesTo("Test");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void AppliesTo_WhenRulesSetsAreNull_AndRulesSetIsNull_ShouldReturnTrue()
        {
            // Arrange
            var validator = new RulesSetEmployeeValidator();
            validator.RulesSets = null;

            // Act
            var result = validator.AppliesTo(null);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void AppliesTo_WhenRulesSetsAreNull_AndRulesSetIsEmpty_ShouldReturnTrue()
        {
            // Arrange
            var validator = new RulesSetEmployeeValidator();
            validator.RulesSets = null;

            // Act
            var result = validator.AppliesTo("");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void AppliesTo_WhenRulesSetsAreNull_AndRulesSetIsNotEmpty_ShouldReturnFalse()
        {
            // Arrange
            var validator = new RulesSetEmployeeValidator();
            validator.RulesSets = null;

            // Act
            var result = validator.AppliesTo("Test");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void AppliesTo_WhenRulesSetIsNotContained_ShouldReturnFalse()
        {
            // Arrange
            var validator = new RulesSetEmployeeValidator();
            validator.RulesSets = new string[] { "Test" };

            // Act
            var result = validator.AppliesTo("Testing");

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
