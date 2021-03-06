﻿using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_Between
    {
        [Test]
        public void ShouldPassBetween()
        {
            Assert.IsTrue(5.Validate().IsBetween(i => i, 4, 6, "5 is not between 4 and 6").IsValid);
            Assert.IsTrue(4.Validate().IsBetween(i => i, 4, 6, "4 is not between 4 and 6").IsValid);
            Assert.IsTrue(5.Validate().IsBetween(i => i, 4, 6, "6 is not between 4 and 6").IsValid);
        }

        [Test]
        public void ShouldFailBetween()
        {
            Assert.IsFalse(3.Validate().IsBetween(i => i, 4, 6, "3 is not between 4 and 6").IsValid);
            Assert.IsFalse(7.Validate().IsBetween(i => i, 4, 6, "7 is not between 4 and 6").IsValid);
        }

        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage1()
        {
            var person = new Person { Name = "Some Name", Age = 18, Goals = 50 };
            var validator = person.Validate(new ValidationOptions { StopOnFirstError = false })
                .IsBetween(v => v.Age, 25, 30)
                .IsBetween(v => v.Goals, 100, 150);
            Assert.That(validator.Errors[0].Message, Is.EqualTo("Person.Age should be between \"25\" and \"30\"."));
            Assert.That(validator.Errors[1].Message, Is.EqualTo("Person.Goals should be between \"100\" and \"150\"."));
        }
    }
}