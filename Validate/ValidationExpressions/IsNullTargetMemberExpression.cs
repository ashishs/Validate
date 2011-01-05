using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNullTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        public IsNullTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = message.Populate(targetType: GetTargetTypeName(), targetMember: GetTargetMemberName());
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target != null)
                                                                  {
                                                                      v.AddError(new ValidationError(message.Populate(targetValue: v.Target).ToString(),
                                                                                                     target, 
                                                                                                     cause: "{{ The target member {0}.{1} was not null. Its value was {2} }}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), v.Target)));
                                                                  }
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(), GetTargetMemberName());
        }
    }
}