using System;
using System.Collections.Generic;

namespace Validate
{
    /// <summary>
    /// A Validation Exception
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// The list of errors which caused this validation exception. When StopOnFirstError is set on the validator, Errors contains only the first validation error.
        /// </summary>
        public IEnumerable<ValidationError> Errors { get; private set; }

        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}