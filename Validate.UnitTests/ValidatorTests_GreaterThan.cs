﻿using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_GreaterThan
    {
        [Test]
        public void ShouldPassForGreaterThan()
        {
            Assert.IsTrue(5.Validate().IsGreaterThan(i => i, 0, "Failed").IsValid);
        }

        [Test]
        public void ShouldFailForGreaterThan()
        {
            Assert.IsFalse(5.Validate().IsGreaterThan(i => i, 5, "Failed").IsValid);
            Assert.IsFalse(5.Validate().IsGreaterThan(i => i, 10, "Failed").IsValid);
        }

        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage1()
        {
            var person = new Person { Name = "Some Name", Age = 12, Goals = 22};
            var validator = person.Validate(new ValidationOptions { StopOnFirstError = false })
                .IsGreaterThan(v => v.Age, 18)
                .IsGreaterThan(v => v.Goals, 100);
            Assert.That(validator.Errors[0].Message, Is.EqualTo("Person.Age should be greater than 18."));
            Assert.That(validator.Errors[1].Message, Is.EqualTo("Person.Goals should be greater than 100."));
        }

        [Test]
        public void ShouldPassForGreaterThanOtherMember()
        {
            var person = new Person { Name = "Some Name", Age = 12, Goals = 22 };
            var validator = person.Validate()
                            .IsGreaterThan(p => p.Goals, p => p.Age, "Goals scored should be greater than age.");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForGreaterThanOtherMember()
        {
            var person = new Person { Name = "Some Name", Age = 12, Goals = 10 };
            var validator = person.Validate()
                            .IsGreaterThan(p => p.Goals, p => p.Age, "Goals scored should be greater than age.");
            Assert.IsFalse(validator.IsValid);
            Assert.AreEqual(validator.Errors[0].Message, "Goals scored should be greater than age.");
        }

        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage2()
        {
            var person = new Person { Name = "Some Name", Age = 12, Goals = 10 };
            var validator = person.Validate()
                            .IsGreaterThan(p => p.Goals, p => p.Age);
            Assert.IsFalse(validator.IsValid);
            Assert.AreEqual(validator.Errors[0].Message, "Person.Goals should be greater than Person.Age.");
        }
    }
}