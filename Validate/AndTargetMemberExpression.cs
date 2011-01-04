using System;
using System.Linq;

namespace Validate
{
    public class AndTargetMemberExpression<T> : TargetMemberValidationExpression<T, object>
    {
        private readonly Predicate<T>[] _predicates;
        private readonly Func<T, Validator>[] _nestedValidators;
        private bool _useNestedValidators;

        public AndTargetMemberExpression(ValidationMessage message, Func<T, Validator>[] nestedValidators)
            : base(message)
        {
            _nestedValidators = nestedValidators;
            _useNestedValidators = true;
        }

        public AndTargetMemberExpression(ValidationMessage message, Predicate<T>[] predicates)
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
                                                                      var match = validators.All(val => val.IsValid);
                                                                      if (!match)
                                                                          v.AddError(new ValidationError(GetValidationMessage(), v.Target, cause: GetCauses(validators).Join(" Or ")));
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, GetValidationMessage(), typeof(T).Name, null);
            }
            else
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      var match = _predicates.All(p => p(v.Target));
                                                                      if (!match)
                                                                          v.AddError(new ValidationError(GetValidationMessage(), v.Target, cause: "At least one of the predicates failed."));
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, GetValidationMessage(), typeof(T).Name, null);
            }
        }
    }
}