using System.Collections.Generic;
using System.Linq;

namespace Validate.UnitTests.Examples
{
    public class ContactValidator
    {
        public virtual List<ValidationError> Validate(Contact contact)
        {
            var validator = contact
                .Validate(new ValidationOptions { StopOnFirstError = false })
                .IsNotNullOrEmpty(c => c.FirstName, "First Name is mandatory.")
                .IsNotNullOrEmpty(c => c.LastName, "Last Name is mandatory.")
                // Method 1, using predicates
                .IfThen(c => c.Organization != null, "Organization title and address are mandatory.",
                        c => !c.Organization.Name.IsNullOrEmpty(),
                        c => !c.Organization.OfficeAddress.AddressLine1.IsNullOrEmpty(),
                        c => !c.Organization.OfficeAddress.AddressLine2.IsNullOrEmpty(),
                        c => !c.Organization.OfficeAddress.City.IsNullOrEmpty(),
                        c => !c.Organization.OfficeAddress.Country.IsNullOrEmpty(),
                        c => !c.Organization.OfficeAddress.Zipcode.IsNullOrEmpty()
                )
                // Method 2, using other validators
                .IfThen(c => c.Organization != null, "Job details are incorrect",
                        c => c.CurrentJob.Validate(new ValidationOptions { StopOnFirstError = false })
                                 .IsNotNullOrEmpty(j => j.Title, "Job title is mandatory")
                                 .IfThen(j => j.To.HasValue, "Job To date should be after job from date",
                                         j => j.From > j.To
                                 )
                )
                // Method 3, using saved validation rules
                // Assumes that we have saved a validation for address.
            .IfThen(c => c.Organization != null, "Office address is mandatory if person is currently working",
                         c => c.BusinessAddress.ValidateUsing("Default_Validation")
                   );
            return validator.Errors.ToList();
        }
    }
}