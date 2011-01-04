using System.Collections.Generic;
using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_Contains
    {
        [Test]
        public void ShouldPassForContains()
        {
            var people = new List<Person> { new Person { Name = "Person1" }, new Person { Name = "Person2" }, new Person { Name = "Person3" } };
            
            var validator = people.Validate().Contains(p => p, "Could not find person", new Person{Name = "Person1"});
            Assert.IsTrue(validator.IsValid);

            validator = people.Validate().Contains(p => p, "Could not find person", new Person { Name = "Person1" }, new Person{Name = "Person3"});
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForContains()
        {
            var people = new List<Person> { new Person { Name = "Person1" }, new Person { Name = "Person2" }, new Person { Name = "Person3" } };
            
            var validator = people.Validate().Contains(p => p, "Could not find person", new Person { Name = "Person4" });
            Assert.IsFalse(validator.IsValid);

            validator = people.Validate().Contains(p => p, "Could not find person", new Person { Name = "Person4" }, new Person { Name = "Person5" });
            Assert.IsFalse(validator.IsValid);
        }
    }
}