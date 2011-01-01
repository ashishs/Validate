namespace Validate
{
    public abstract class ValidationResultToExceptionTransformer
    {
        public Validator Validator { get; internal set; }

        public abstract void Throw();
    }

    public class ValidationResultToValidationExceptionTransformer : ValidationResultToExceptionTransformer
    {
        public override void Throw()
        {
            throw new ValidationException(Validator.Errors);
        }
    }
}