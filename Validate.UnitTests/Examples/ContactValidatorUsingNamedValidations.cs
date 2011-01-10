using NUnit.Framework;
using Validate.Extensions;
using Validate.UnitTests.Examples.Contract;

namespace Validate.UnitTests.Examples
{
    [TestFixture]
    public class ContactValidatorUsingNamedValidations
    {
        [Test]
        public void ShouldBeAbleToValidateContactUsingNamedValidations()
        {
            var validation = new Validation<Contact>("Contact_Validation_Example", new ValidationOptions { StopOnFirstError = false })
                .Setup(validator => validator
                                        .IsNotNullOrEmpty(c => c.FirstName)
                                        .IsNotNullOrEmpty(c => c.LastName)
                                        .IfThen(c => c.Organization != null, "Organization Title and Address are mandatory.", c => c.Organization.ValidateUsing("Company_Validation_Example"))
                                        .IfThen(c => c.Organization != null, "Current job details are missing.", c => c.CurrentJob.ValidateUsing("Job_Validation_Example"))
                                        .IfThen(c => c.Organization != null, "Business address is missing.",
                                                c => c.BusinessAddress.ValidateUsing("Address_Validation_Example"))
                );
            var organizationValidation = new Validation<Company>("Company_Validation_Example", new ValidationOptions { StopOnFirstError = false })
                .Setup(validator => validator
                                        .IsNotNullOrEmpty(c => c.Name)
                                        .PassesSavedValidation(c => c.OfficeAddress, "Address_Validation_Example")
                );
            var addressValidation = new Validation<Contract.Address>("Address_Validation_Example")
                .Setup(validator => validator
                                        .IsNotNullOrEmpty(c => c.AddressLine1)
                                        .IsNotNullOrEmpty(c => c.AddressLine2)
                                        .IsNotNullOrEmpty(c => c.City)
                                        .IsNotNullOrEmpty(c => c.StateOrCounty)
                                        .IsNotNullOrEmpty(c => c.Country)
                                        .IsNotNullOrEmpty(c => c.Zipcode)
                );
            var jobValidation = new Validation<Job>("Job_Validation_Example")
                .Setup(validator => validator
                                        .IsNotNullOrEmpty(j => j.Title)
                );

            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            validationRepository.Save(validation);
            validationRepository.Save(organizationValidation);
            validationRepository.Save(addressValidation);
            validationRepository.Save(jobValidation);

            var contact = new ContactBuilder().WithFirstName().WithLastName().WithOrganization().WithCurrentJob().WithBusinessAddress().GetContact();

            var contactValidator = contact.ValidateUsing("Contact_Validation_Example");
            Assert.IsTrue(contactValidator.IsValid);

            contact.BusinessAddress.AddressLine1 = null;
            contactValidator = contact.ValidateUsing("Contact_Validation_Example");
            Assert.IsFalse(contactValidator.IsValid);
            Assert.AreEqual(contactValidator.Errors[0].Message, "Business address is missing.");
            Assert.IsTrue(contactValidator.Errors[0].Cause.Contains("The target member Address.AddressLine1 was null or empty."));

        }
    }
}