namespace Validate
{
    /// <summary>
    /// Abstract base class for the validation result to exception transformer.
    /// </summary>
    public abstract class ValidationResultToExceptionTransformer
    {
        public Validator Validator { get; internal set; }

        /// <summary>
        /// Override this method to specify custom validation exception behaviour.
        /// </summary>
        public abstract void Throw();
    }

    /// <summary>
    /// Default implementation of the validation result to exception transformer. Coverts validation error(s) to a ValidationException.
    /// </summary>
    public class ValidationResultToValidationExceptionTransformer : ValidationResultToExceptionTransformer
    {
        public override void Throw()
        {
            throw new ValidationException(Validator.Errors);
        }
    }
}