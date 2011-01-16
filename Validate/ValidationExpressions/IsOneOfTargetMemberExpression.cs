using System;
using System.Linq;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsOneOfTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly U[] _isOneOfValues;

        public IsOneOfTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message, params U[] isOneOfValues)
            : base(targetMemberExpression, message)
        {
            _isOneOfValues = isOneOfValues ?? new U[0];
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var oneOfValuesToDisplay = _isOneOfValues.Select(val => val.ToString()).Join(" | ");
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueIsOneOf: oneOfValuesToDisplay);
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (!_isOneOfValues.Any(val => val.Equals(target)))
                                                                  {
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata, 
                                                                                 cause: "{{ The target member {0}.{1} with value {2} was not found one of the given value(s) {{ {3} }} }}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, oneOfValuesToDisplay)));
                                                                  }
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}