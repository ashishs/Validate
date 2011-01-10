using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsGreaterThanOtherMemberTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly Expression<Func<T, U>> _greaterThanSelector;

        public IsGreaterThanOtherMemberTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, Expression<Func<T, U>> greaterThanSelector, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _greaterThanSelector = greaterThanSelector;
            VerifyValidationExpression(message, greaterThanSelector);
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var greaterThanDisplayName = "{0}.{1}".WithFormat(GetTargetTypeName(_greaterThanSelector), GetTargetMemberName(_greaterThanSelector));
            var validationMessage = Message.Populate(targetType: GetTargetTypeName(TargetMemberExpression), targetMember: GetTargetMemberName(TargetMemberExpression), targetValueGreaterThan: greaterThanDisplayName);
            var compiledSelector = TargetMemberExpression.Compile();
            var compiledGreaterThanSelector = _greaterThanSelector.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  var greaterThan = compiledGreaterThanSelector(v.Target);
                                                                  if (target.CompareTo(greaterThan) <= 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                                     cause: "{{The target member {0}.{1} with value {2} was not greater than {3} with value.}}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression), target, greaterThanDisplayName, greaterThan)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }
}