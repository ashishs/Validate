using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Validate.Extensions;

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
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueMatchesRegex: _regexPattern);
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (!Regex.IsMatch(target, _regexPattern, _regexOptions))
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                 cause: "{{The target member {0}.{1} with value {2} did not match pattern {3}.}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, _regexPattern)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}