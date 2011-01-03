namespace Validate
{
    public interface IValidationRepositoryFactory
    {
        IValidationRepository GetValidationRepository();
    }

    public class ValidationRepositoryFactory : IValidationRepositoryFactory
    {
        private static readonly ValidationRepository ValidationRepository = new ValidationRepository();

        public virtual IValidationRepository GetValidationRepository()
        {
            return ValidationRepository;
        }
    }
}