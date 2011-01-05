using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNullOrEmptyTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IEnumerable
    {
        public IsNullOrEmptyTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = message.Populate(targetType: GetTargetTypeName(), targetMember: GetTargetMemberName());
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target != null && target.OfType<object>().FirstOrDefault() != null)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                "{{ The target member {0}.{1} was not null or empty. Its value was {2}}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), target)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(), GetTargetMemberName());
        }
    }
}