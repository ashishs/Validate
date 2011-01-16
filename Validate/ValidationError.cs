namespace Validate
{
    /// <summary>
    /// A Validation Error.
    /// </summary>
    public class ValidationError
    {
        public string Message { get; private set; }
        public object Value { get; private set; }
        
        /// <summary>
        /// The cause of the validation error.
        /// </summary>
        public string Cause { get; private set; }

        public ValidationError(string message, object value, TargetMemberMetadata targetMemberMetadata, string cause = null)
        {
            Message = message;
            Value = value;
            Cause = cause ?? string.Empty;
        }
    }
}