using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_NotBetween
    {
        [Test]
        public void ShouldPassNotBetween()
        {   
            Assert.IsTrue(3.Validate().IsNotBetween(i => i, 4, 6, "5 is between 4 and 6").IsValid);
            Assert.IsTrue(7.Validate().IsNotBetween(i => i, 4, 6, "5 is between 4 and 6").IsValid);
        }

        [Test]
        public void ShouldFailNotBetween()
        {
            Assert.IsFalse(5.Validate().IsNotBetween(i => i, 4, 6, "5 is between 4 and 6").IsValid);
            Assert.IsFalse(4.Validate().IsNotBetween(i => i, 4, 6, "4 is between 4 and 6").IsValid);
            Assert.IsFalse(6.Validate().IsNotBetween(i => i, 4, 6, "6 is between 4 and 6").IsValid);
        }
    }
}