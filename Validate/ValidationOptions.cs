namespace Validate
{
    public class ValidationOptions
    {
        public bool StopOnFirstError { get; set; }
        public bool ThrowValidationExceptionOnValidationError { get; set; }
        public ValidationResultToExceptionTransformer ValidationResultToExceptionTransformer { get; set; }
        
        public ValidationOptions()
        {
            StopOnFirstError = true;
            ThrowValidationExceptionOnValidationError = false;
        }
    }
}