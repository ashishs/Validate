using System.Collections.Generic;
using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_NotNull
    {
        [Test]
        public void ShouldPassForIsNotNull()
        {
            List<string> values = new List<string>();
            var validator = values.Validate().IsNotNull(v => v, "Values canot be null");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForIsNotNull()
        {
            List<string> values = null;
            var validator = values.Validate().IsNotNull(v => v, "Values canot be null");
            Assert.IsFalse(validator.IsValid);
        }
    }
}