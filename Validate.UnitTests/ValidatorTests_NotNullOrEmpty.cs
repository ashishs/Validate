using System.Collections.Generic;
using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_NotNullOrEmpty
    {
        [Test]
        public void ShouldPassForIsNotNullOrEmpty()
        {
            List<string> values = new List<string> {"One"};
            var validator = values.Validate().IsNotNullOrEmpty(v => v, "Values canot be null or empty.");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForIsNotNullOrEmpty()
        {
            List<string> values = new List<string>();
            var validator = values.Validate().IsNotNullOrEmpty(v => v, "Values canot be null or empty");
            Assert.IsFalse(validator.IsValid);
        }
    }
}