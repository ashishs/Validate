using System;
using System.Linq;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IfThenTargetMemberExpression<T> : TargetMemberValidationExpression<T, object>
    {
        private readonly Predicate<T>[] _predicates;
        private readonly Predicate<T> _ifThis;
        private readonly Func<T, Validator>[] _nestedValidators;
        private bool _useNestedValidators;

        public IfThenTargetMemberExpression(Predicate<T> ifThis, ValidationMessage message, Func<T, Validator>[] nestedValidators)
            : base(message)
        {
            _ifThis = ifThis;
            _nestedValidators = nestedValidators;
            _useNestedValidators = true;
        }

        public IfThenTargetMemberExpression(Predicate<T> ifThis, ValidationMessage message, Predicate<T>[] predicates)
            : base(message)
        {
            _ifThis = ifThis;
            _predicates = predicates;
            _useNestedValidators = false;
        }

        protected virtual bool EvaluateIfPredicate(T target)
        {
            return _ifThis(target);
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            if (_useNestedValidators)
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      if (EvaluateIfPredicate(v.Target))
                                                                      {
                                                                          var validators = _nestedValidators.Select(valFunc => valFunc(v.Target));
                                                                          if (validators.Any(val => !val.IsValid))
                                                                              v.AddError(new ValidationError(GetValidationMessage(), v.Target, cause: GetCauses(validators).Join(" ")));
                                                                      }
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, GetValidationMessage(), typeof(T).Name, null);
            }
            else
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      if (EvaluateIfPredicate(v.Target))
                                                                      {
                                                                          if (_predicates.Any(p => !p(v.Target)))
                                                                              v.AddError(new ValidationError(GetValidationMessage(), v.Target, cause: "At least one of the predicates failed."));
                                                                      }
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, GetValidationMessage(), typeof(T).Name, null);
            }
        }
    }
}