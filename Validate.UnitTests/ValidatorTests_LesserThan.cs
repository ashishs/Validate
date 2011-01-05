﻿using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_LesserThan
    {
        [Test]
        public void ShouldPassForLesserThan()
        {
            Assert.IsTrue(5.Validate().IsLesserThan(i => i, 10, "Failed").IsValid);
        }

        [Test]
        public void ShouldFailForLesserThan()
        {
            Assert.IsFalse(5.Validate().IsLesserThan(i => i, 5, "Failed").IsValid);
            Assert.IsFalse(5.Validate().IsLesserThan(i => i, 0, "Failed").IsValid);
        }
        
        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage1()
        {
            var person = new Person { Name = "Some Name", Age = 18, Goals = 100 };
            var validator = person.Validate(new ValidationOptions { StopOnFirstError = false })
                .IsLesserThan(v => v.Age, 18)
                .IsLesserThan(v => v.Goals, 50);
            Assert.That(validator.Errors[0].Message, Is.EqualTo("Person.Age should be lesser than 18."));
            Assert.That(validator.Errors[1].Message, Is.EqualTo("Person.Goals should be lesser than 50."));
        }
    }
}