using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNotBetweenTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly U _lesserThanOrEqualTo;
        private readonly U _greaterThanOrEqualTo;

        public IsNotBetweenTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U lesserThanOrEqualTo, U greaterThanOrEqualTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _lesserThanOrEqualTo = lesserThanOrEqualTo;
            _greaterThanOrEqualTo = greaterThanOrEqualTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = message.Populate(targetType: GetTargetTypeName(), targetMember: GetTargetMemberName(), targetValueLesserThan: _lesserThanOrEqualTo, targetValueGreaterThan: _greaterThanOrEqualTo);
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_lesserThanOrEqualTo) <= 0 && target.CompareTo(_greaterThanOrEqualTo) >= 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                 cause: "{{The target member {0}.{1} with value {2} was between [{3}, {4}].}}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), target, _lesserThanOrEqualTo, _greaterThanOrEqualTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(), GetTargetMemberName());
        }
    }
}