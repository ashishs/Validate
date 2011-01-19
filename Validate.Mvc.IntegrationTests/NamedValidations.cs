using Validate.Extensions;
using Validate.Mvc.IntegrationTests.Models;

namespace Validate.Mvc.IntegrationTests
{
    public class NamedValidations
    {
        public void Initialize()
        {
            var contactValidation = new Validation<Contact_1>("Contact", new ValidationOptions { StopOnFirstError = false })
                .Setup(validator => validator.IsNotNullOrEmpty(c => c.FirstName)
                                        .IsNotNullOrEmpty(c => c.LastName)
                                        .IsNotNullOrEmpty(c => c.BusinessEmail)
                                        .IsNotNullOrEmpty(c => c.PersonalEmail)
                                        .IsNotNullOrEmpty(c => c.CurrentJob.Title)
                );
            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            validationRepository.Save(contactValidation);
        }
    }
}