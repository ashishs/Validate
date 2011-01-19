using System.Linq;
using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidateUsingAttributeTests
    {
        [ValidateUsing("MyPerson_Validation_Example")]
        [ValidateUsing(typeof(MyPersonValidator))]
        private class MyPerson
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class MyPersonValidator : AbstractClassValidator<MyPerson>
        {
            public MyPersonValidator(MyPerson target) : base(target)
            {
            }

            public override Validator<MyPerson> Validate()
            {
                return Target.Validate(new ValidationOptions { StopOnFirstError = false })
                    .IsNotNullOrEmpty(p => p.Name)
                    .IsGreaterThan(p => p.Age, 18);
            }
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            // Set up validation
            var validation = new Validation<MyPerson>("MyPerson_Validation_Example", new ValidationOptions { StopOnFirstError = false })
                .Setup(validator => validator
                                        .IsNotNullOrEmpty(person => person.Name)
                                        .IsGreaterThan(person => person.Age, 18)
                );

            // Save validation
            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            validationRepository.Save(validation);
        }

        [Test]
        public void ShouldPassValidationForValidPerson()
        {
            Assert.AreEqual(2, typeof(MyPerson).GetCustomAttributes(typeof(ValidateUsingAttribute), true).OfType<ValidateUsingAttribute>().Count());
            var person = new MyPerson {Name = "My Name", Age = 25};
            var validateUsingValidationAttribute = person.GetType().GetCustomAttributes(typeof (ValidateUsingAttribute), true).OfType<ValidateUsingAttribute>().First();
            var validator = validateUsingValidationAttribute.Validate(person, typeof(MyPerson));
            Assert.IsTrue(validator.IsValid);

            var validateUsingClassValidatorAttribute = person.GetType().GetCustomAttributes(typeof(ValidateUsingAttribute), true).OfType<ValidateUsingAttribute>().Last();
            validator = validateUsingClassValidatorAttribute.Validate(person, typeof(MyPerson));
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailValidationForInvalidPerson()
        {
            Assert.AreEqual(2, typeof(MyPerson).GetCustomAttributes(typeof(ValidateUsingAttribute), true).OfType<ValidateUsingAttribute>().Count());
            var inValidPerson = new MyPerson { Name = "My Name", Age = 15 };
            var validateUsingValidationAttribute = inValidPerson.GetType().GetCustomAttributes(typeof(ValidateUsingAttribute), true).OfType<ValidateUsingAttribute>().First();
            var validator = validateUsingValidationAttribute.Validate(inValidPerson, typeof(MyPerson));
            Assert.IsFalse(validator.IsValid);

            var validateUsingClassValidatorAttribute = inValidPerson.GetType().GetCustomAttributes(typeof(ValidateUsingAttribute), true).OfType<ValidateUsingAttribute>().Last();
            validator = validateUsingClassValidatorAttribute.Validate(inValidPerson, typeof(MyPerson));
            Assert.IsFalse(validator.IsValid);
        }
    }
}