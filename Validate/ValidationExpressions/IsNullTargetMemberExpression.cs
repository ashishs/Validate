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
            var validationMessage = Message.Populate(targetType: GetTargetTypeName(TargetMemberExpression), targetMember: GetTargetMemberName(TargetMemberExpression));
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target != null)
                                                                  {
                                                                      v.AddError(new ValidationError(Message.Populate(targetValue: v.Target).ToString(),
                                                                                                     target, 
                                                                                                     cause: "{{ The target member {0}.{1} was not null. Its value was {2} }}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression), v.Target)));
                                                                  }
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }
}