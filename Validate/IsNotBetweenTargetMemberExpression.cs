using System;
using System.Linq.Expressions;

namespace Validate
{
    public class IsNotBetweenTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly U _lesserThanOrEqualTo;
        private readonly U _greaterThanOrEqualTo;

        public IsNotBetweenTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U lesserThanOrEqualTo, U greaterThanOrEqualTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _lesserThanOrEqualTo = lesserThanOrEqualTo;
            _greaterThanOrEqualTo = greaterThanOrEqualTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_lesserThanOrEqualTo) <= 0 && target.CompareTo(_greaterThanOrEqualTo) >= 0)
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}