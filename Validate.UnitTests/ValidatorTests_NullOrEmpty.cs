using System.Collections.Generic;
using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_NullOrEmpty
    {
        [Test]
        public void ShouldPassForIsNullOrEmpty()
        {
            List<string> values = new List<string> { };
            var validator = values.Validate().IsNullOrEmpty(v => v, "Values canot be null or empty.");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForIsNullOrEmpty()
        {
            List<string> values = new List<string>{"One"};
            var validator = values.Validate().IsNullOrEmpty(v => v, "Values canot be null or empty");
            Assert.IsFalse(validator.IsValid);
        }
    }
}