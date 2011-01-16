using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsLesserThanOtherMemberTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly Expression<Func<T, U>> _lesserThanSelector;
        private TargetMemberMetadata _lesserThanMetadata;

        public IsLesserThanOtherMemberTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, Expression<Func<T, U>> lesserThanSelector, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _lesserThanSelector = lesserThanSelector;
            VerifyValidationExpression(message, lesserThanSelector);
            _lesserThanMetadata = new TargetMemberExpression<T>(lesserThanSelector).GetTargetMemberMetadata();

        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var lesserThanDisplayName = "{0}.{1}".WithFormat(_lesserThanMetadata.Type.FriendlyName(), _lesserThanMetadata.MemberName);
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueLesserThan: lesserThanDisplayName);
            var compiledSelector = TargetMemberExpression.Compile();
            var compiledLesserThanSelector = _lesserThanSelector.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  var lesserThan = compiledLesserThanSelector(v.Target);
                                                                  if (target.CompareTo(lesserThan) >= 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                                     cause: "{{The target member {0}.{1} with value {2} was not lesser than {3} with value.}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, lesserThanDisplayName, lesserThan)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}