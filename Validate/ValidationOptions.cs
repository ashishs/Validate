namespace Validate
{
    public class ValidationOptions
    {
        public bool StopOnFirstError { get; set; }

        public ValidationOptions()
        {
            StopOnFirstError = true;
        }
    }
}