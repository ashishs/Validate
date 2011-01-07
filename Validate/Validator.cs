using System.Collections.Generic;
using System.Collections.ObjectModel;
using Validate.Extensions;

namespace Validate
{
    /// <summary>
    /// Object validator. This implementation can be chained using the extension methods in the validatorX class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T> : AbstractValidator
    {
        public T Target { get; private set; }

        private List<ValidationMethod<T>> _validations = new List<ValidationMethod<T>>();

        public Validator(T target, ValidationOptions options = null)
        {
            Target = target;
            this.options = options ?? new ValidationOptions();

            // TODO: Think of a better way to do this
            base.options.ValidationResultToExceptionTransformer.Validator = this;
        }

        public ReadOnlyCollection<ValidationMethod<T>> Validations
        {
            get
            {
                return _validations.AsReadOnly();
            }
        }

        public void AddValidation(ValidationMethod<T> validationMethod)
        {   
            _validations.Add(validationMethod);
        }

        internal object ValidationTarget
        {
            get { return Target; }
        }

        internal bool ValidateFurther
        {
            get
            {
                return options.Enabled && options.StopOnFirstError ? Errors.IsNullOrEmpty() : options.Enabled;
            }
        }
    }
}