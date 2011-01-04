using System;
using System.Linq.Expressions;

namespace Validate.ValidationExpressions
{
    public class IsEqualToTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly U _equalTo;

        public IsEqualToTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U equalTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _equalTo = equalTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (!target.Equals(_equalTo))
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}