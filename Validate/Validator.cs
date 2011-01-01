using System;
using System.Collections.Generic;
using Validate;

namespace Validate
{
    public interface IValidator
    {
        bool IsValid { get; }
        List<ValidationError> Errors { get; }

    }

    public abstract class Validator : IValidator
    {
        public abstract bool IsValid { get; }

        public abstract List<ValidationError> Errors { get; set; }
    }

    public class Validator<T> : Validator
    {
        public T Target { get; private set; }
        private ValidationOptions options;

        internal Validator(T target, ValidationOptions options = null)
        {
            Target = target;
            this.options = options ?? new ValidationOptions();
            Errors = new List<ValidationError>();

        }

        public override List<ValidationError> Errors { get; set; }

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
    }
}