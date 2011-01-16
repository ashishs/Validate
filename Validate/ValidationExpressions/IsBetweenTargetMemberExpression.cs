using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsBetweenTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly U _lesserThanOrEqualTo;
        private readonly U _greaterThanOrEqualTo;

        public IsBetweenTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U lesserThanOrEqualTo, U greaterThanOrEqualTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _lesserThanOrEqualTo = lesserThanOrEqualTo;
            _greaterThanOrEqualTo = greaterThanOrEqualTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueLesserThan: _lesserThanOrEqualTo, targetValueGreaterThan: _greaterThanOrEqualTo);
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_lesserThanOrEqualTo) > 0 || target.CompareTo(_greaterThanOrEqualTo) < 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                 cause: "{{The target member {0}.{1} with value {2} was not between [{3}, {4}].}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, _lesserThanOrEqualTo, _greaterThanOrEqualTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}