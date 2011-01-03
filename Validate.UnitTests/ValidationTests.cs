using System.Diagnostics;
using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void ShouldBeAbleToSetUpAndRunASimpleValidation()
        {
            var validation = new Validation<Person>()
                             .Setup(v => v.IsNotNull(p => p.Name, "Name is mandatory.")
                                         .IsGreaterThan(p => p.Age, 18, "Age must be nore than 18.")
                                    );

            var personToValidate = new Person {Name = "Person's Name", Age = 25};
            var validator = validation.RunAgainst(personToValidate);
            Assert.IsTrue(validator.IsValid);

            var invalidPersonToValidate = new Person {Name = "Person's Name", Age = 15};
            validator = validation.RunAgainst(invalidPersonToValidate);
            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldBeAbleToSetUpAndRunAndValidation()
        {
            var person = new Person {Age = 30, Fouls = 200, Name = "Some Person"};

            var validation = new Validation<Person>()
                             .Setup(v => v.And("One of the conditions failed.",
                                                p => p.Age > 15,
                                                p => p.Fouls > 50
                                              )
                                   );
            var validator = validation.RunAgainst(person);
            Assert.IsTrue(validator.IsValid);

            var invalidPerson = new Person { Age = 15, Fouls = 200, Name = "Some Person" };
            validator = validation.RunAgainst(invalidPerson);
            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldBeAbleToSetUpAndRunNestedValidation()
        {
            var person = new Person { Age = 30, Fouls = 200, Name = "Some Person" };

            var validation = new Validation<Person>()
                             .Setup(v => v.And("One of the conditions failed.",
                                                p => p.Age > 15,
                                                p => p.Fouls > 50
                                              )
                                   );
            var validator = person.Validate().And("One of the validations failed",
                                                  p => validation.RunAgainst(p)
                                                 );
            Assert.IsTrue(validator.IsValid);

            var invalidPerson = new Person { Age = 15, Fouls = 200, Name = "Some Person" };
            validator = invalidPerson.Validate().And("One of the validations failed",
                                                      p => validation.RunAgainst(p)
                                                     );
            Assert.IsFalse(validator.IsValid);
        }
    }
}