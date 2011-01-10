using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsTrueTargetMemberExpression<T> : TargetMemberValidationExpression<T, bool>
    {
        public IsTrueTargetMemberExpression(Expression<Func<T, bool>> targetMemberExpression, ValidationMessage message)
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
                                                                  if (!target)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: v.Target).ToString(), target,
                                                                                 cause: "{{The target member {0}.{1} was false.}}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression))));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }
}