using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Validate.ValidationExpressions
{
    public class MatchesRegexTargetMemberExpression<T> : TargetMemberValidationExpression<T, string>
    {
        private readonly string _regexPattern;
        private readonly RegexOptions _regexOptions;

        public MatchesRegexTargetMemberExpression(Expression<Func<T, string>> targetMemberExpression, string regexPattern, RegexOptions regexOptions, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _regexPattern = regexPattern;
            _regexOptions = regexOptions;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (!Regex.IsMatch(target, _regexPattern, _regexOptions))
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}