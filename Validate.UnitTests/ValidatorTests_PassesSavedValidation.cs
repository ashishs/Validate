using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_PassesSavedValidation
    {
        [Test]
        public void ShouldPassForPassesSavedValidation()
        {
            var validation = new Validation<Person>("Person_Validation_Example")
                .Setup(v => v.IsNotNullOrEmpty(p => p.Name));
            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            validationRepository.Reset();
            validationRepository.Save(validation);

            var person = new Person {Name = "Some Name"};
            var validator = person.Validate().PassesSavedValidation(p => p, "Person_Validation_Example");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForPassesSavedValidation()
        {
            var validation = new Validation<Person>("Person_Validation_Example")
                .Setup(v => v.IsNotNullOrEmpty(p => p.Name));
            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            validationRepository.Reset();
            validationRepository.Save(validation);

            var person = new Person { Name = "" };
            var validator = person.Validate().PassesSavedValidation(p => p, "Person_Validation_Example");
            Assert.IsFalse(validator.IsValid);
        }
    }
}