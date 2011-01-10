using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNotNullTargetMemberExpression<T,U> : TargetMemberValidationExpression<T,U>
    {
        public IsNotNullTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message) : base(targetMemberExpression, message)
        {
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = Message.Populate(targetType: GetTargetTypeName(TargetMemberExpression), targetMember: GetTargetMemberName(TargetMemberExpression));
            var compiledSelector = TargetMemberExpression.Compile();
            Func <Validator<T> ,Validator<T>> validation = (v) =>
                                                               {
                                                                   var target = compiledSelector(v.Target);
                                                                   if (target == null)
                                                                   {   
                                                                       v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                                      "{{The target member {0}.{1} was null.}}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression))));
                                                                   }
                                                                   return v;
                                                               };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }
}