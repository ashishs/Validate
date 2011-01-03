using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidationRepositoryFactoryTests
    {
        [Test]
        public void ShouldGetTheSameValidationRepositoryOnSubsequentCalls()
        {
            var repository1 = new ValidationRepositoryFactory().GetValidationRepository();
            var repositorty2 = new ValidationRepositoryFactory().GetValidationRepository();
            Assert.AreEqual(repository1,repositorty2);
        }
    }
}