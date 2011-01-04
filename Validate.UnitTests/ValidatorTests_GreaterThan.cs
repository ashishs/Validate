using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_GreaterThan
    {
        [Test]
        public void ShouldPassForGreaterThan()
        {
            Assert.IsTrue(5.Validate().IsGreaterThan(i => i, 0, "Failed").IsValid);
        }

        [Test]
        public void ShouldFailForGreaterThan()
        {
            Assert.IsFalse(5.Validate().IsGreaterThan(i => i, 5, "Failed").IsValid);
            Assert.IsFalse(5.Validate().IsGreaterThan(i => i, 10, "Failed").IsValid);
        }
    }
}