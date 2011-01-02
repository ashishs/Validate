namespace Validate
{
    /// <summary>
    /// This specifies the behaviour of the validator.
    /// </summary>
    public class ValidationOptions
    {
        /// <summary>
        /// Specifies whether validation should stop on the first validation error. Default value is true.
        /// </summary>
        public bool StopOnFirstError { get; set; }

        /// <summary>
        /// Specifies whether validation eception should be thrown when validation errors occur.
        /// </summary>
        public bool ThrowValidationExceptionOnValidationError { get; set; }

        /// <summary>
        /// The Validation result transformer. This raises a custom exception given a validator with errors.
        /// </summary>
        public ValidationResultToExceptionTransformer ValidationResultToExceptionTransformer { get; set; }
        
        public ValidationOptions()
        {
            StopOnFirstError = true;
            ThrowValidationExceptionOnValidationError = false;
        }
    }
}