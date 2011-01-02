using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_Between
    {
        [Test]
        public void ShouldPassBetween()
        {
            Assert.IsTrue(5.Validate().IsBetween(i => i, 4, 6, "5 is not between 4 and 6").IsValid);
            Assert.IsTrue(4.Validate().IsBetween(i => i, 4, 6, "4 is not between 4 and 6").IsValid);
            Assert.IsTrue(5.Validate().IsBetween(i => i, 4, 6, "6 is not between 4 and 6").IsValid);
        }

        [Test]
        public void ShouldFailBetween()
        {
            Assert.IsFalse(3.Validate().IsBetween(i => i, 4, 6, "3 is not between 4 and 6").IsValid);
            Assert.IsFalse(7.Validate().IsBetween(i => i, 4, 6, "7 is not between 4 and 6").IsValid);
        }
    }
}