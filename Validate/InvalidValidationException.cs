using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Validate
{
    [Serializable]
    public class InvalidValidationException : Exception
    {
        public Expression ValidationExpression { get; set; }

        public InvalidValidationException()
        {
        }

        public InvalidValidationException(string message) : base(message)
        {
        }

        public InvalidValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            info.AddValue("ValidationExpression", ValidationExpression);
        }
    }
}