using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsLesserThanTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly U _lesserThan;

        public IsLesserThanTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U lesserThan, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _lesserThan = lesserThan;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = message.Populate(targetType: GetTargetTypeName(), targetMember: GetTargetMemberName(), targetValueLesserThan: _lesserThan);
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_lesserThan) >= 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                 cause: "{{The target member {0}.{1} with value {2} was not lesser than {3}.}}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), target, _lesserThan)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(), GetTargetMemberName());
        }
    }
}