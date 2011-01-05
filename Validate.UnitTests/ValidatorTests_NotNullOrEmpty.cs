﻿using System.Collections.Generic;
using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_NotNullOrEmpty
    {
        [Test]
        public void ShouldPassForIsNotNullOrEmpty()
        {
            List<string> values = new List<string> {"One"};
            var validator = values.Validate().IsNotNullOrEmpty(v => v, "Values canot be null or empty.");
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailForIsNotNullOrEmpty()
        {
            List<string> values = new List<string>();
            var validator = values.Validate().IsNotNullOrEmpty(v => v, "Values canot be null or empty");
            Assert.IsFalse(validator.IsValid);
        }


        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage1()
        {
            List<string> values = null;
            var validator = values.Validate().IsNotNullOrEmpty(v => v);
            Assert.That(validator.Errors[0].Message, Is.EqualTo("List`1[String].Value should not be null or empty."));
        }

        [Test]
        public void ShouldBeAbleToVerifyAutoGeneratedValidationMessage2()
        {
            var person = new Person();
            var validator = person.Validate(new ValidationOptions { StopOnFirstError = false })
                            .IsNotNullOrEmpty(p => p.Name)
                            .IsNotNullOrEmpty(p => p.HomeAddress.AddressLine1)
                            .IsNotNullOrEmpty(p => p.EmailAddresses);
            Assert.That(validator.Errors[0].Message, Is.EqualTo("Person.Name should not be null or empty."));
            Assert.That(validator.Errors[1].Message, Is.EqualTo("Address.AddressLine1 should not be null or empty."));
            Assert.That(validator.Errors[2].Message, Is.EqualTo("Person.EmailAddresses should not be null or empty."));
        }
    }
}