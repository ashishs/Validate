using System;
using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidationRepositoryTests
    {
        [Test]
        public void ShouldBeAbleToSaveAndGetAValidation()
        {
            var validation = new Validation<Person>()
                .Setup(v => v.IsNotNull(p => p.Name, "Name is mandatory"));
                             
            var validationRepository = new ValidationRepository();
            validationRepository.Save(validation);
            var fetchedValidation = validationRepository.Get<Person>("Default_Validation");
            Assert.AreEqual(validation, fetchedValidation);
        }

        [Test]
        public void ShouldNotBeAbleToSaveMultipleValidationsForTheATypeWithTheSameName()
        {
            var validation1 = new Validation<Person>()
                .Setup(v => v.IsNotNull(p => p.Name, "Name is mandatory"));
            var validation2 = new Validation<Person>()
                .Setup(v => v.IsNotNull(p => p.Name, "Name is mandatory"));

            var validationRepository = new ValidationRepository();
            TestDelegate testCode = () =>
                                        {
                                            validationRepository.Save(validation1);
                                            validationRepository.Save(validation2);                                
                                        };
            Assert.Throws<ArgumentException>(testCode);

        }


        [Test]
        public void ShouldBeAbleToRunASavedValidation()
        {
            var validation = new Validation<Person>()
                .Setup(v => v.IsNotNull(p => p.Name, "Name is mandatory"));

            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            validationRepository.Save(validation);
            

            var invalidPerson = new Person();
            var validator = invalidPerson.ValidateUsing("Default_Validation");
            Assert.IsFalse(validator.IsValid);
        }
    }
}