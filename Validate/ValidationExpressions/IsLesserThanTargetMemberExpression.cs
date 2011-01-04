using System;
using System.Linq.Expressions;

namespace Validate.ValidationExpressions
{
    public class IsLesserThanTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IComparable
    {
        private readonly U _lesserThan;

        public IsLesserThanTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U lesserThan, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _lesserThan = lesserThan;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_lesserThan) >= 0)
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}