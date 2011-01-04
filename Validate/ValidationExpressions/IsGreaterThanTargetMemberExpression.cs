using System;
using System.Linq.Expressions;

namespace Validate.ValidationExpressions
{
    public class IsGreaterThanTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U:IComparable
    {
        private readonly U _greaterThan;

        public IsGreaterThanTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U greaterThan, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _greaterThan = greaterThan;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_greaterThan) <= 0)
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}