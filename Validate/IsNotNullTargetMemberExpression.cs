using System;
using System.Linq.Expressions;

namespace Validate
{
    public class IsNotNullTargetMemberExpression<T,U> : TargetMemberValidationExpression<T,U>
    {
        public IsNotNullTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message) : base(targetMemberExpression, message)
        {
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func <Validator<T> ,Validator<T>> validation = (v) =>
                                                               {
                                                                   var target = compiledSelector(v.Target);
                                                                   if (target == null)
                                                                       v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                   return v;
                                                               };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}