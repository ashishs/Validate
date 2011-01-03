using System;

namespace Validate
{
    public interface IValidationMetadata
    {
        string Alias { get; }
        Type ValidationTargetType { get; }
    }

    public class Validation<T> : IValidationMetadata
    {
        public string Alias { get; private set; }
        private readonly ValidationOptions _options;
        private Validator<T> _validator;
        public Type ValidationTargetType { get { return typeof (T); } }
        private readonly object _lock = new object();

        public Validation(string alias = null, ValidationOptions options = null)
        {
            Alias = alias.IsNullOrEmpty() ? "Default_Validation" : alias;
            
            _options = options ?? new ValidationOptions();
            _options.Enabled = false;
        }

        public Validation<T> Setup(Func<Validator<T>, Validator<T>> validations) 
        {
            lock (_lock)
            {
                if (_validator == null)
                    _validator = default(T).Validate(_options);
                _validator = validations(_validator);
            }
            return this;
        }

        public Validator<T> RunAgainst(T target)
        {
            var validator = target.Validate(GetOptionsWhileValidating());
            for(int i=0; i< _validator.Validations.Count; i++)
            {
                var validation = _validator.Validations[i];
                validation.ExecuteInValidationBlock(validator, "");
            }

            return validator;
        }

        public ValidationOptions GetOptionsWhileValidating()
        {
            var options = new ValidationOptions
                              {
                                  Enabled = true,
                                  StopOnFirstError = _options.StopOnFirstError,
                                  ThrowValidationExceptionOnValidationError = _options.ThrowValidationExceptionOnValidationError,
                                  ValidationResultToExceptionTransformer = _options.ValidationResultToExceptionTransformer
                              };
            return options;
        }
    }
}