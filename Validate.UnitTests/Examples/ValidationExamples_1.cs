using System;
using NUnit.Framework;
using Validate;
using Validate.Extensions;
using Validate.UnitTests.Examples.Contract;

namespace Validate.UnitTests.Examples
{
    [TestFixture]
    public class ValidationExamples_1
    {
        [Test]
        public void ShouldValidateFirstAndLastName()
        {
            // Create a contact
            var contact = new ContactBuilder().WithFirstName().WithLastName().GetContact();

            // Check for first name and last name
            var validator = contact.Validate()
                                   .IsNotNullOrEmpty(c => c.FirstName)
                                   .IsNotNullOrEmpty(c => c.LastName, "Last name is mandatory.");
            Assert.IsTrue(validator.IsValid);
            
            // Clear out last name and validate again
            contact.LastName = null;
            validator = contact.Validate()
                               .IsNotNullOrEmpty(c => c.FirstName, "First name is mandatory.")
                               .IsNotNull(c => c.LastName);
            Assert.IsFalse(validator.IsValid);
            Assert.IsNotEmpty(validator.Errors);
            Assert.AreEqual("Contact.LastName should not be null.", validator.Errors[0].Message);
        }
        
        [Test]
        public void ShouldValidateBusinessAddressAndCurrentJobIfOrganizationIsGiven()
        {
            // Create a contact
            var contact = new ContactBuilder().WithOrganization().WithBusinessAddress().WithCurrentJob().GetContact();

            var validator = contact.Validate()
                                   .IfThen(c => c.Organization != null, 
                                           "Address and Current Job should be provided.", 
                                           c => c.BusinessAddress.Validate()
                                                                  .IsNotNullOrEmpty(a => a.AddressLine1, "Line 1 is mandatory.")
                                                                  .IsNotNullOrEmpty(a => a.AddressLine2, "Line 2 is mandatory.")
                                                                  .IsNotNullOrEmpty(a => a.City, "City is mandatory.")
                                                                  .IsNotNullOrEmpty(a => a.Country, "Country is mandatory.")
                                                                  .IsNotNullOrEmpty(a => a.Zipcode, "Zipcode is mandatory."),
                                           c => c.CurrentJob.Validate()
                                                             .IsNotNullOrEmpty(j => j.Title, "Job title is mandatory.")
                                                             .IsLesserThan(j => j.From, DateTime.Today, "From date should be earlier than today.")
                                                             .IfThen(j => j.To.HasValue, "To should be after from.", j => j.To.Value > j.From)
                                          );
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldValidateIfOneOfBusinessOrPersonalEmailIsGiven()
        {
            // Create a contact
            var contact = new ContactBuilder().WithPersonalEmail().WithBusinessEmail().GetContact();

            var validator = contact.Validate()
                            .Or("Atleast one of personal email or business email must be specified.",
                                c => c.PersonalEmail.Validate().IsNotNullOrEmpty(e => e, "Personal email is required."),
                                c => c.BusinessEmail.Validate().IsNotNullOrEmpty(e => e, "Business email is required.")
                               );
            Assert.IsTrue(validator.IsValid);

            // Validator should raise error if both personal email and business email are null
            contact.PersonalEmail = null;
            contact.BusinessEmail = null;
            validator = contact.Validate()
                            .Or("Atleast one of personal email or business email must be specified.",
                                c => c.PersonalEmail.Validate().IsNotNullOrEmpty(e => e, "Personal email is required."),
                                c => c.BusinessEmail.Validate().IsNotNullOrEmpty(e => e, "Business email is required.")
                               );
            Assert.IsFalse(validator.IsValid);
            Assert.IsNotEmpty(validator.Errors);
        }
    }

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
            Assert.AreEqual("Business address is missing.", contactValidator.Errors[0].Message);
            Assert.IsTrue(contactValidator.Errors[0].Cause.Contains("The target member Address.AddressLine1 was null or empty."));

        }
    }
}