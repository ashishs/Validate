using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_OneOf
    {
        [Test]
        public void ShouldPassForOneOf()
        {
            var person = new Person { Name = "Person2" };

            var validator = person.Validate().IsOneOf(p => p, "Person is not one of people.", new Person { Name = "Person1" }, new Person { Name = "Person2" }, new Person { Name = "Person3" });
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForOneOf()
        {
            var person = new Person { Name = "Person2" };

            var validator = person.Validate().IsOneOf(p => p, "Person is not one of people.", new Person { Name = "Person1" }, new Person { Name = "Person3" }, new Person { Name = "Person4" });
            Assert.IsFalse(validator.IsValid);
        }
    }
}