using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNotEqualToOtherMemberTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly Expression<Func<T, U>> _notEqualToSelector;
        private TargetMemberMetadata _notEqualToMetadata;

        public IsNotEqualToOtherMemberTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, Expression<Func<T, U>> notEqualToSelector, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _notEqualToSelector = notEqualToSelector;
            VerifyValidationExpression(message, _notEqualToSelector);
            _notEqualToMetadata = new TargetMemberExpression<T>(notEqualToSelector).GetTargetMemberMetadata();
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = TargetMemberExpression.Compile();
            var compiledNotEqualToSelector = _notEqualToSelector.Compile();
            var notEqualToMemberDisplayName = "{0}.{1}".WithFormat(_notEqualToMetadata.Type.FriendlyName(), _notEqualToMetadata.MemberName);
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueNotEqualTo: notEqualToMemberDisplayName);
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  var equalTo = compiledNotEqualToSelector(v.Target);
                                                                  if (target.Equals(equalTo))
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                                     cause: "{{The target member {0}.{1} with value {2} was not equal to {3} with value {4}.}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, notEqualToMemberDisplayName, equalTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}