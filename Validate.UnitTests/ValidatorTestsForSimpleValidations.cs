using System.Collections.Generic;
using NUnit.Framework;
using Validate;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTestsForSimpleValidations
    {
        [Test]
        public void ShouldFailForIsNotNull()
        {
            List<string> values = null;
            var validator = values.Validate().IsNotNull(v => v,"Values canot be null");
            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldPassForIsNull()
        {
            List<string> values = null;
            var validator = values.Validate().IsNull(v => v, "Values should be null");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldPassGreaterThanAndLessThan()
        {
            Assert.IsTrue(5.Validate().IsGreaterThan(i => i, 0, "Failed").IsValid);
            Assert.IsFalse(5.Validate().IsGreaterThan(i => i, 10, "Failed").IsValid);
            Assert.IsTrue(5.Validate().IsLessThan(i => i, 10, "Failed").IsValid);
            Assert.IsFalse(5.Validate().IsLessThan(i => i, 0, "Failed").IsValid);
        }

        [Test]
        public void ShouldPassBetween()
        {
            Assert.IsTrue(5.Validate().IsBetween(i => i, 4, 6, "5 is not between 4 and 6").IsValid);
            Assert.IsTrue(4.Validate().IsBetween(i => i, 4, 6, "4 is not between 4 and 6").IsValid);
            Assert.IsTrue(5.Validate().IsBetween(i => i, 4, 6, "6 is not between 4 and 6").IsValid);
            Assert.IsFalse(3.Validate().IsBetween(i => i, 4, 6, "3 is not between 4 and 6").IsValid);
            Assert.IsFalse(7.Validate().IsBetween(i => i, 4, 6, "7 is not between 4 and 6").IsValid);
        }

        [Test]
        public void ShouldPassNotBetween()
        {
            Assert.IsFalse(5.Validate().IsNotBetween(i => i, 4, 6, "5 is between 4 and 6").IsValid);
            Assert.IsFalse(4.Validate().IsNotBetween(i => i, 4, 6, "4 is between 4 and 6").IsValid);
            Assert.IsFalse(6.Validate().IsNotBetween(i => i, 4, 6, "6 is between 4 and 6").IsValid);
            Assert.IsTrue(3.Validate().IsNotBetween(i => i, 4, 6, "5 is between 4 and 6").IsValid);
            Assert.IsTrue(7.Validate().IsNotBetween(i => i, 4, 6, "5 is between 4 and 6").IsValid);
        }

        [Test] 
        public void ShouldPassChainedValidation()
        {
            var obj = new {Name = "Ashish", Goals = 15, Fouls = 100};
            var validator = obj.Validate()
                .IsNotNull(v => v, "Object should be null")
                .IsNotNull(v => v.Name, "Name cannot be null")
                .IsGreaterThan(v => v.Goals, 10, "Goals should be greater than 10")
                .IsLessThan(v => v.Fouls, 110, "Fould should be less than 110");
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
    }
}