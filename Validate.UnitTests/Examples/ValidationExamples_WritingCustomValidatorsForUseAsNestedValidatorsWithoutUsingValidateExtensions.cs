using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using Validate.Extensions;
using Validate.UnitTests.Examples.Contract;

namespace Validate.UnitTests.Examples
{
    [TestFixture]
    public class ValidationExamples_WritingCustomValidatorsForUseAsNestedValidatorsWithoutUsingValidateExtensions
    {
        // This is a custom validator which is used in nested validations like Or, And, IfThe, IfNotThen, etc.
        // Nested validators should implement at least IValidator
        private class MyCustomAddressValidatorForUSAddresses : IValidator
        {
            private readonly Contract.Address _address;
            private List<ValidationError> _errors = new List<ValidationError>();

            public MyCustomAddressValidatorForUSAddresses(Contract.Address address)
            {
                _address = address;
                ValidateAddress();
            }

            private void ValidateAddress()
            {
                if(_address != null && _address.Country.EqualsIgnoreCase("US"))
                    _errors.Add(new ValidationError("This is not a valid US Address.", _address, cause: "{ The address was null or the country was not US. }"));
            }

            public bool IsValid
            {
                get { return _errors.IsNullOrEmpty(); }
            }

            public ReadOnlyCollection<ValidationError> Errors
            {
                get { return _errors.AsReadOnly(); }
            }

            public void Throw()
            {
                throw new ValidationException(_errors);
            }
        }

        [Test]
        public void ShouldUseCustomValidatorForValidationInAnd()
        {
            var contact = new ContactBuilder().WithOtherAddress().WithBusinessAddress().GetContact();
            // Setting one address to be outside US
            contact.OtherAddress.Country = "US";
            contact.BusinessAddress.Country = "Germany";

            var validator = contact.Validate(new ValidationOptions {StopOnFirstError = false})
                .And("Some validation failed.", 
                     c => new MyCustomAddressValidatorForUSAddresses(c.OtherAddress),
                     c => new MyCustomAddressValidatorForUSAddresses(c.BusinessAddress)
                );
            Assert.IsFalse(validator.IsValid);
            Assert.AreEqual(validator.Errors[0].Message, "Some validation failed.");
            Assert.IsTrue(validator.Errors[0].Cause.Contains("{ The address was null or the country was not US. }"));
        }
    }
}