using NUnit.Framework;
using Validate;
using System.Linq;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTestsForChainedValidations
    {
        [Test] 
        public void ShouldPassChainedValidation()
        {
            var obj = new {Name = "Ashish", Goals = 15, Fouls = 100};
            var validator = obj.Validate()
                .IsNotNull(v => v, "Object should be null")
                .IsNotNull(v => v.Name, "Name cannot be null")
                .IsGreaterThan(v => v.Goals, 10, "Goals should be greater than 10")
                .IsLesserThan(v => v.Fouls, 110, "Fould should be less than 110");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldPassChainedValidationWithContinuedValidation()
        {
            Person obj = null;
            var validator = obj.Validate(options: new ValidationOptions {StopOnFirstError = false})
                .IsNotNull(v => v, "Object should not be null")
                .IsNotNull(v => v.Name, "Name cannot be null")
                .IsGreaterThan(v => v.Goals, 10, "Goals should be greater than 10");
            Assert.IsFalse(validator.IsValid);
            Assert.AreEqual(3, validator.Errors.Count);
        }

        [Test]
        public void ShouldThrowValidationExceptionOnFirstFailedValidation()
        {
            var person = new Person {Age = 25, Fouls = 10, Goals = 100, Name = "Some dude."};
            TestDelegate testCode = () => person
                                          .Validate(new ValidationOptions { StopOnFirstError = true, ThrowValidationExceptionOnValidationError = true })
                                          .IsLesserThan(p => p.Age, 18, "You should be less than 18 years old.")
                                          .IsGreaterThan(p => p.Goals, 200, "You should have scored more than 200 goals.");
            var validationException = Assert.Throws<ValidationException>(testCode);
            Assert.That(validationException.Errors.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ShouldNotThrowValidationExceptionOnFirstFailedValidation()
        {
            var person = new Person { Age = 25, Fouls = 10, Goals = 100, Name = "Some dude." };
            TestDelegate testCode = () => person
                                          .Validate(new ValidationOptions { StopOnFirstError = false, ThrowValidationExceptionOnValidationError = true })
                                          .IsLesserThan(p => p.Age, 18, "You should be less than 18 years old.")
                                          .IsGreaterThan(p => p.Goals, 200, "You should have scored more than 200 goals.")
                                          .Throw();
            var validationException = Assert.Throws<ValidationException>(testCode);
            Assert.That(validationException.Errors.Count(), Is.EqualTo(2));
        }
    }
}