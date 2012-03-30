using NUnit.Framework;
using Simple.Validation.Comparers;

namespace Simple.Validation.Tests.Comparers
{
    [TestFixture]
    public class SelectorEqualityComparerTests
    {

        private class Entity
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        [Test]
        public void CTor()
        {
            // Arrange: Declare any variables or set up any conditions
            //          required by your test.


            // Act:     Perform the activity under test.
            var comparer = new SelectorEqualityComparer<Entity>(e => e.Id);

            // Assert:  Verify that the activity under test had the
            //          expected results
        }

        [Test]
        public void Equals_WhenIdsMatch_ShouldReturnTrue()
        {
            // Arrange: Declare any variables or set up any conditions
            //          required by your test.
            var x = new Entity() { Id = 1, Description = "One" };
            var y = new Entity() { Id = 1, Description = "Uno" };

            var comparer = new SelectorEqualityComparer<Entity>(e => e.Id);


            // Act:     Perform the activity under test.
            var result = comparer.Equals(x, y);

            // Assert:  Verify that the activity under test had the
            //          expected results
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_WhenIdsDoNotMatch_ShouldReturnFalse()
        {
            // Arrange: Declare any variables or set up any conditions
            //          required by your test.
            var x = new Entity() { Id = 1, Description = "One" };
            var y = new Entity() { Id = 2, Description = "Two" };

            var comparer = new SelectorEqualityComparer<Entity>(e => e.Id);


            // Act:     Perform the activity under test.
            var result = comparer.Equals(x, y);

            // Assert:  Verify that the activity under test had the
            //          expected results
            Assert.That(result, Is.False);
        }

    }
}
