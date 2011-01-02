using System;

namespace Validate
{
    public class Validation<T>
    {
        private readonly string _alias;
        private readonly ValidationOptions _options;
        private Validator<T> _validator;
        private readonly object _lock = new object();
        
        public Validation(string alias = null, ValidationOptions options = null)
        {
            _alias = alias.IsNullOrEmpty() ? "DEFAULT".WithFormat(typeof (T).FullName) : alias;
            
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

        public Validator<T> ValidationTarget
        {
            get
            {
                lock (_lock)
                {
                    if (_validator == null)
                        _validator = default(T).Validate(_options);
                }
                return _validator;
            }
        }

        public Validator<T> RunAgainst(T target)
        {
            var validator = target.Validate(GetOptionsWhileValidating());
            foreach(var validation in _validator.Validations)
            {
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