using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Validate;
using System.Linq;
using Validate.Extensions;

namespace Validate
{
    /// <summary>
    /// The base class for all validators
    /// </summary>
    public abstract class Validator
    {
        public abstract bool IsValid { get; }

        public abstract ReadOnlyCollection<ValidationError> Errors { get; }

        public abstract ValidationResultToExceptionTransformer ValidationResultToExceptionTransformer { get; }

        internal abstract object ValidationTarget { get; }
        internal abstract bool ValidateFurther { get; }
        internal abstract void AddError(ValidationError validationError);
    }

    /// <summary>
    /// Object validator. This implementation can be chained using the extension methods in the validatorX class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T> : Validator
    {
        public T Target { get; private set; }

        private ValidationOptions _options;
        private List<ValidationError> _errors = new List<ValidationError>();
        private List<ValidationMethod<T>> _validations = new List<ValidationMethod<T>>();

        internal Validator(T target, ValidationOptions options = null)
        {
            Target = target;
            this._options = options ?? new ValidationOptions();

            // TODO: Think of a better way to do this
            _options.ValidationResultToExceptionTransformer.Validator = this;
        }

        internal override object ValidationTarget
        {
            get { return Target; }
        }

        public override ReadOnlyCollection<ValidationError> Errors 
        {
            get { return _errors.AsReadOnly(); }
            
        }

        public ReadOnlyCollection<ValidationMethod<T>> Validations
        {
            get
            {
                return _validations.AsReadOnly();
            }
        }

        public override ValidationResultToExceptionTransformer ValidationResultToExceptionTransformer { get { return _options.ValidationResultToExceptionTransformer; } }
        
        public void AddValidation(ValidationMethod<T> validationMethod)
        {   
            _validations.Add(validationMethod);
        }

        /// <summary>
        /// Returns true if the object being validated is valid, false otherwise.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Errors.IsNullOrEmpty();
            }
        }

        internal override bool ValidateFurther
        {
            get
            {
                return _options.Enabled && _options.StopOnFirstError ? Errors.IsNullOrEmpty() : _options.Enabled;
            }
        }

        internal override void AddError(ValidationError validationError)
        {
            _errors.Add(validationError);
            if(_options.StopOnFirstError && _options.ThrowValidationExceptionOnValidationError)
                _options.ValidationResultToExceptionTransformer.Throw();
        }
    }
}