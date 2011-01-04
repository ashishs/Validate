using System;
using System.Linq.Expressions;

namespace Validate.ValidationExpressions
{
    public class IsNotEqualToTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly U _notEqualTo;

        public IsNotEqualToTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U notEqualTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _notEqualTo = notEqualTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.Equals(_notEqualTo))
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}