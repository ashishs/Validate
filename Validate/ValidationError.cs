namespace Validate
{
    public class ValidationError
    {
        public string Message { get; private set; }
        public object Value { get; private set; }
        public string Cause { get; private set; }

        public ValidationError(string message, object value, string cause = null)
        {
            Message = message;
            Value = value;
            Cause = cause ?? string.Empty;
        }
    }
}