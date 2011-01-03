using System;
using NUnit.Framework;
using Validate;

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
                                   .IsNotNullOrEmpty(c => c.FirstName, "First name is mandatory.")
                                   .IsNotNullOrEmpty(c => c.LastName, "Last name is mandatory.");
            Assert.IsTrue(validator.IsValid);
            
            // Clear out last name and validate again
            contact.LastName = null;
            validator = contact.Validate()
                               .IsNotNullOrEmpty(c => c.FirstName, "First name is mandatory.")
                               .IsNotNull(c => c.LastName, "Last name is mandatory.");
            Assert.IsFalse(validator.IsValid);
            Assert.IsNotEmpty(validator.Errors);
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
}