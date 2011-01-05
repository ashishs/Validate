using System;
using System.Linq;
using Validate.Extensions;

namespace Validate.ValidationExpressions
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
            var validationMessage = message.Populate(targetType: GetTargetTypeName(), targetMember: GetTargetMemberName());
            if (_useNestedValidators)
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      var validators = _nestedValidators.Select(valFunc => valFunc(v.Target));
                                                                      var match = validators.All(val => val.IsValid);
                                                                      if (!match)
                                                                          v.AddError(new ValidationError(validationMessage.Populate(targetValue: v.Target).ToString(), v.Target,
                                                                                     "{{The AND validation for target member {0}.{1} with value {2} failed because {{{3}}} }}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), v.Target, GetCauses(validators.Where(val => !val.IsValid)).Join(" And "))));      
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, validationMessage, typeof(T).Name, null);
            }
            else
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      var match = _predicates.All(p => p(v.Target));
                                                                      if (!match)
                                                                          v.AddError(new ValidationError(validationMessage.Populate(targetValue: v.Target).ToString(), v.Target,
                                                                                     "{{The AND validation for target member {0}.{1} with value {2} failed because {{ At least one of the predicates failed. }} }}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), v.Target)));      
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, validationMessage, typeof(T).Name, null);
            }
        }
    }
}