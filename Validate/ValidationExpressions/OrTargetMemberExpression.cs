using System;
using System.Linq;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class OrTargetMemberExpression<T> : TargetMemberValidationExpression<T, object>
    {
        private readonly Predicate<T>[] _predicates;
        private readonly Func<T, Validator>[] _nestedValidators;
        private bool _useNestedValidators;
        
        public OrTargetMemberExpression(ValidationMessage message, Func<T, Validator>[] nestedValidators)
            : base(message)
        {
            _nestedValidators = nestedValidators;
            _useNestedValidators = true;
        }

        public OrTargetMemberExpression(ValidationMessage message, Predicate<T>[] predicates)
            : base(message)
        {
            _predicates = predicates;
            _useNestedValidators = false;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            if (_useNestedValidators)
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      var validators = _nestedValidators.Select(valFunc => valFunc(v.Target));
                                                                      var match = validators.Any(val => val.IsValid);
                                                                      if (!match)
                                                                          v.AddError(new ValidationError(GetValidationMessage(), v.Target, cause: GetCauses(validators).Join(" And ")));
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, GetValidationMessage(), typeof(T).Name, null);
            }
            else
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                {
                    var match = _predicates.Any(p => p(v.Target));
                    if (!match)
                        v.AddError(new ValidationError(GetValidationMessage(), v.Target, cause: "All of the predicates failed."));
                    return v;
                };
                return new ValidationMethod<T>(validation, GetValidationMessage(), typeof(T).Name, null);
            }
        }
    }
}