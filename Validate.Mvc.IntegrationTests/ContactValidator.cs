using Validate.Extensions;
using Validate.Mvc.IntegrationTests.Models;

namespace Validate.Mvc.IntegrationTests
{
    public class ContactValidator : AbstractClassValidator<Contact_2>
    {
        public ContactValidator(Contact_2 target)
            : base(target)
        {
        }

        public override Validator<Contact_2> Validate()
        {
            return Target.Validate(new ValidationOptions {StopOnFirstError = false})
                .IsNotNullOrEmpty(c => c.FirstName)
                .IsNotNullOrEmpty(c => c.LastName)
                .IsNotNullOrEmpty(c => c.BusinessEmail)
                .IsNotNullOrEmpty(c => c.PersonalEmail)
                .IsNotNullOrEmpty(c => c.CurrentJob.Title);
        }
    }
}