using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_RegexMatches
    {
        [Test]
        public void ShouldPassForMatchesRegex()
        {
            var person = new Person {Name = "Some Person"};
            var validator = person.Validate().MatchesRegex(p => p.Name, "Some\\sPerson",message: "Regex match failed.");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForMatchesRegex()
        {
            var person = new Person { Name = "Some Person" };
            var validator = person.Validate().MatchesRegex(p => p.Name, "Some\\sOther\\sPerson", message: "Regex match failed.");
            Assert.IsFalse(validator.IsValid);
        }
    }
}