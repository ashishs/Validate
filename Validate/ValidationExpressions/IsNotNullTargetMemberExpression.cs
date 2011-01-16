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
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName);
            var compiledSelector = TargetMemberExpression.Compile();
            Func <Validator<T> ,Validator<T>> validation = (v) =>
                                                               {
                                                                   var target = compiledSelector(v.Target);
                                                                   if (target == null)
                                                                   {   
                                                                       v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                                      "{{The target member {0}.{1} was null.}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName)));
                                                                   }
                                                                   return v;
                                                               };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}