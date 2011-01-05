﻿using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_NotEqualTo
    {
        [Test]
        public void ShouldPassForNotEqualTo()
        {
            var person = new Person { Name = "Some Name", Age = 25 };
            var validator = person.Validate().IsNotEqualTo(v => v.Age, 18, "Age should not be 18");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForNotEqualTo()
        {
            var person = new Person { Name = "Some Name", Age = 18 };
            var validator = person.Validate().IsNotEqualTo(v => v.Age, 18, "Age should not be 18");
            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage1()
        {
            var person = new Person { Name = "Some Name", Age = 18, HomeAddress = new Address { City = "Reading" } };
            var validator = person.Validate(new ValidationOptions { StopOnFirstError = false })
                .IsNotEqualTo(v => v.Age, 18)
                .IsNotEqualTo(v => v.HomeAddress.City, "Reading");
            Assert.That(validator.Errors[0].Message, Is.EqualTo("Person.Age should not be equal to 18."));
            Assert.That(validator.Errors[1].Message, Is.EqualTo("Address.City should not be equal to Reading."));
        }
    }
}