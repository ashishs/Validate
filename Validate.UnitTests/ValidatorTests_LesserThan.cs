using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_LesserThan
    {
        [Test]
        public void ShouldPassForLesserThan()
        {
            Assert.IsTrue(5.Validate().IsLesserThan(i => i, 10, "Failed").IsValid);
        }

        [Test]
        public void ShouldFailForLesserThan()
        {
            Assert.IsFalse(5.Validate().IsLesserThan(i => i, 5, "Failed").IsValid);
            Assert.IsFalse(5.Validate().IsLesserThan(i => i, 0, "Failed").IsValid);
        }
    }
}