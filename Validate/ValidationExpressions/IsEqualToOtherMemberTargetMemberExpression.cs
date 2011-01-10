using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsEqualToOtherMemberTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly Expression<Func<T, U>> _equalToSelector;

        public IsEqualToOtherMemberTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, Expression<Func<T, U>> equalToSelector, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _equalToSelector = equalToSelector;
            VerifyValidationExpression(message, _equalToSelector);
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = TargetMemberExpression.Compile();
            var compiledEqualToSelector = _equalToSelector.Compile();
            var equalToMemberDisplayName = "{0}.{1}".WithFormat(GetTargetTypeName(_equalToSelector), GetTargetMemberName(_equalToSelector));
            var validationMessage = Message.Populate(targetType: GetTargetTypeName(TargetMemberExpression), targetMember: GetTargetMemberName(TargetMemberExpression), targetValueEqualTo: equalToMemberDisplayName);
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  var equalTo = compiledEqualToSelector(v.Target);
                                                                  if (!target.Equals(equalTo))
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                                     cause: "{{The target member {0}.{1} with value {2} was not equal to {3} with value {4}.}}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression), target, equalToMemberDisplayName, equalTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }
}