using System;
using System.Collections.Generic;

namespace Validate
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationError> Errors { get; private set; }

        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}