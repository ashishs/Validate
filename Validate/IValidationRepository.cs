namespace Validate
{
    public interface IValidationRepository
    {
        void Save<T>(Validation<T> validationToSave);
        Validation<T> Get<T>(string validationAlias);
    }
}