using NUnit.Framework;
using Validate.Extensions;
using Validate.UnitTests.Examples.Contract;

namespace Validate.UnitTests.Examples
{
    [TestFixture]
    public class ValidationExamples_WritingCustomValidatorsForUseAsNestedValidators
    {
        // This is a custom validator which is used in nested validations like Or, And, IfThe, IfNotThen, etc.
        // Nested validators should implement at least IValidator
        private class MyCustomAddressValidatorForUSAddresses : AbstractValidator
        {
            public MyCustomAddressValidatorForUSAddresses(Contract.Address addesss)
            {
                var validator = addesss.Validate()
                    .IsNotNull(a => a)
                    .IsEqualTo(a => a.Country, "US");
                this.errors.AddRange(validator.Errors);
            }
        }

        [Test]
        public void ShouldUseCustomValidatorForValidationInAnd()
        {
            var contact = new ContactBuilder().WithOtherAddress().WithBusinessAddress().GetContact();
            // Setting one address to be outside US
            contact.OtherAddress.Country = "US";
            contact.BusinessAddress.Country = "Germany";

            var validator = contact.Validate(new ValidationOptions { StopOnFirstError = false })
                .And("Some validation failed.",
                     c => new MyCustomAddressValidatorForUSAddresses(c.OtherAddress),
                     c => new MyCustomAddressValidatorForUSAddresses(c.BusinessAddress)
                );
            Assert.IsFalse(validator.IsValid);
            Assert.AreEqual(validator.Errors[0].Message, "Some validation failed.");
            Assert.IsTrue(validator.Errors[0].Cause.Contains("{{{The target member Address.Country with value Germany was not equal to US.}}}"));
        }
    }
}