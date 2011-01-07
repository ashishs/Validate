using System.Collections.Generic;
using System.Collections.ObjectModel;
using Validate.Extensions;

namespace Validate
{
    /// <summary>
    /// The base class for all validators
    /// </summary>
    public abstract class AbstractValidator : IValidator
    {
        protected ValidationOptions options;
        protected List<ValidationError> errors = new List<ValidationError>();

        public virtual ReadOnlyCollection<ValidationError> Errors
        {
            get { return errors.AsReadOnly(); }
        }

        public virtual void Throw()
        {
            options.ValidationResultToExceptionTransformer.Throw();
        }

        /// <summary>
        /// Returns true if the object being validated is valid, false otherwise.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return Errors.IsNullOrEmpty();
            }
        }

        public virtual void AddError(ValidationError validationError)
        {
            errors.Add(validationError);
            if (options.StopOnFirstError && options.ThrowValidationExceptionOnValidationError)
                options.ValidationResultToExceptionTransformer.Throw();
        }
    }
}