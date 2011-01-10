using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNotEqualToOtherMemberTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly Expression<Func<T, U>> _notEqualToSelector;

        public IsNotEqualToOtherMemberTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, Expression<Func<T, U>> notEqualToSelector, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _notEqualToSelector = notEqualToSelector;
            VerifyValidationExpression(message, _notEqualToSelector);
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = TargetMemberExpression.Compile();
            var compiledNotEqualToSelector = _notEqualToSelector.Compile();
            var notEqualToMemberDisplayName = "{0}.{1}".WithFormat(GetTargetTypeName(_notEqualToSelector), GetTargetMemberName(_notEqualToSelector));
            var validationMessage = Message.Populate(targetType: GetTargetTypeName(TargetMemberExpression), targetMember: GetTargetMemberName(TargetMemberExpression), targetValueNotEqualTo: notEqualToMemberDisplayName);
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  var equalTo = compiledNotEqualToSelector(v.Target);
                                                                  if (target.Equals(equalTo))
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                                     cause: "{{The target member {0}.{1} with value {2} was not equal to {3} with value {4}.}}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression), target, notEqualToMemberDisplayName, equalTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }
}