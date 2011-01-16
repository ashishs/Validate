using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsEqualToTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly U _equalTo;

        public IsEqualToTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U equalTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _equalTo = equalTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueEqualTo: _equalTo);
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (!target.Equals(_equalTo))
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                 cause: "{{The target member {0}.{1} with value {2} was not equal to {3}.}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, _equalTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}