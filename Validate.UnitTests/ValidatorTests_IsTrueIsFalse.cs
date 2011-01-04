﻿using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_IsTrueIsFalse
    {
        [Test]
        public void ShouldPassForIsTrueAndFailForIsFalse()
        {
            var person = new Person {Goals = 101, Fouls = 9};
            
            var validator = person.Validate().IsTrue(v => v.Goals > 100 && v.Fouls < 10, "Goals should be greater than 100 and fouls should be less than 10.");
            Assert.IsTrue(validator.IsValid);

            validator = person.Validate().IsFalse(v => v.Goals > 100 && v.Fouls < 10, "Goals should be greater than 100 and fouls should be less than 10.");
            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldFailForIsTrueAndPassForIsFalse()
        {
            var person = new Person { Goals = 99, Fouls = 11 };

            var validator = person.Validate().IsTrue(v => v.Goals > 100 && v.Fouls < 10, "Goals should be greater than 100 and fouls should be less than 10.");
            Assert.IsFalse(validator.IsValid);

            validator = person.Validate().IsFalse(v => v.Goals > 100 && v.Fouls < 10, "Goals should be greater than 100 and fouls should be less than 10.");
            Assert.IsTrue(validator.IsValid);
        }
    }
}