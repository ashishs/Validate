using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Validate;
using System.Linq;

namespace Validate
{
    public interface IValidator
    {
        bool IsValid { get; }
        ReadOnlyCollection<ValidationError> Errors { get; }

    }

    public abstract class Validator : IValidator
    {
        public abstract bool IsValid { get; }

        public abstract ReadOnlyCollection<ValidationError> Errors { get; }

        public abstract ValidationResultToExceptionTransformer ValidationResultToExceptionTransformer { get; }
    }

    public class Validator<T> : Validator
    {
        public T Target { get; private set; }
        private ValidationOptions options;
        private List<ValidationError> _errors = new List<ValidationError>();

        internal Validator(T target, ValidationOptions options = null)
        {
            Target = target;
            this.options = options ?? new ValidationOptions();

            // TODO: Thik of a better way to do this
            if (this.options.ValidationResultToExceptionTransformer != null)
                options.ValidationResultToExceptionTransformer.Validator = this;
        }

        public override ReadOnlyCollection<ValidationError> Errors 
        {
            get { return _errors.AsReadOnly(); }
            
        }

        public override ValidationResultToExceptionTransformer ValidationResultToExceptionTransformer { get { return options.ValidationResultToExceptionTransformer; } }

        public override bool IsValid
        {
            get
            {
                return Errors.IsNullOrEmpty();
            }
        }

        internal bool ValidateFurther
        {
            get
            {
                return options.StopOnFirstError ? Errors.IsNullOrEmpty() : true;
            }
        }

        internal void AddError(ValidationError validationError)
        {
            _errors.Add(validationError);
            if(options.StopOnFirstError && options.ThrowValidationExceptionOnValidationError)
                options.ValidationResultToExceptionTransformer.Throw();
        }
    }
}