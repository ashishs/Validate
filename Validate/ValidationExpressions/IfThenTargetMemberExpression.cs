using System;
using System.Linq;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IfThenTargetMemberExpression<T> : TargetMemberValidationExpression<T, object>
    {
        private readonly Predicate<T>[] _predicates;
        private readonly Predicate<T> _ifThis;
        private readonly Func<T, IValidator>[] _nestedValidators;
        private bool _useNestedValidators;

        public IfThenTargetMemberExpression(Predicate<T> ifThis, ValidationMessage message, Func<T, IValidator>[] nestedValidators)
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
            var validationMessage = Message.Populate(targetType: typeof(T).FriendlyName());
            if (_useNestedValidators)
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      var target = v.Target;
                                                                      if (EvaluateIfPredicate(target))
                                                                      {
                                                                          var validators = _nestedValidators.Select(valFunc => valFunc(target)).ToList();
                                                                          if(validators.Any(val => !val.IsValid))
                                                                              v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                         "{{The IfThen validation for target member {0}.{1} with value {2} failed because {{{3}}} }}".WithFormat(typeof(T).FriendlyName(), "{{ Target member could not be determined }}", target, GetCauses(validators.Where(val => !val.IsValid)).Join(" "))));      
                                                                      }
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
            }
            else
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                                                                  {
                                                                      var target = v.Target;
                                                                      if (EvaluateIfPredicate(target))
                                                                      {
                                                                          if (_predicates.Any(val => !val(target)))
                                                                              v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                         "{{The IfThen validation for target member {0}.{1} with value {2} failed because {{ At least one of the predicates failed. }} }}".WithFormat(typeof(T).FriendlyName(), "{{ Target member could not be determined }}", target)));      
                                                                      }
                                                                      return v;
                                                                  };
                return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
            }
        }
    }
}