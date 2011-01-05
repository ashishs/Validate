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
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (!_isOneOfValues.Any(val => val.Equals(target)))
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: "The value {0} was not found in the given values {{1}}".WithFormat(target, _isOneOfValues.Select(val => val.ToString()).Join(" | "))));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetTargetTypeName(), GetTargetMemberName());
        }
    }
}