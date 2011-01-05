﻿using System.Collections.Generic;
using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTests_IsNull
    {
        [Test]
        public void ShouldPassForIsNull()
        {
            List<string> values = null;
            var validator = values.Validate().IsNull(v => v, "Values should be null");
            Assert.IsTrue(validator.IsValid);
        }
        
        [Test]
        public void ShouldFailForIsNull()
        {
            List<string> values = new List<string>();
            var validator = values.Validate().IsNull(v => v, "Values should be null");
            Assert.IsFalse(validator.IsValid);
            Assert.That(validator.Errors[0].Message, Is.EqualTo("Values should be null"));
        }
    }
}