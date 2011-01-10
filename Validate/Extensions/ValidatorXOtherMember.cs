using System;
using System.Linq.Expressions;
using Validate.ValidationExpressions;

namespace Validate.Extensions
{
    public static class ValidatorXOtherMember
    {
        public static Validator<T> IsEqualTo<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, Expression<Func<T, U>> equalToSelector, string message = null)
        {
            var validationMessage = message == null ? ValidationMessageRepository.Instance.GetValidationMessageForIsEqualTo() : new ValidationMessage(message);

            var validationExpression = new IsEqualToOtherMemberTargetMemberExpression<T, U>(selector, equalToSelector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsNotEqualTo<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, Expression<Func<T, U>> notEqualToSelector, string message = null)
        {
            var validationMessage = message == null ? ValidationMessageRepository.Instance.GetValidationMessageForIsNotEqualTo() : new ValidationMessage(message);

            var validationExpression = new IsNotEqualToOtherMemberTargetMemberExpression<T, U>(selector, notEqualToSelector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsGreaterThan<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, Expression<Func<T, U>> greaterThanSelector, string message = null) where U : IComparable
        {
            var validationMessage = message == null ? ValidationMessageRepository.Instance.GetValidationMessageForIsGreaterThan() : new ValidationMessage(message);

            var validationExpression = new IsGreaterThanOtherMemberTargetMemberExpression<T, U>(selector, greaterThanSelector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsLesserThan<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, Expression<Func<T, U>> lesserThanSelector, string message = null) where U : IComparable
        {
            var validationMessage = message == null ? ValidationMessageRepository.Instance.GetValidationMessageForIsLesserThan() : new ValidationMessage(message);

            var validationExpression = new IsLesserThanOtherMemberTargetMemberExpression<T, U>(selector, lesserThanSelector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }
    }
}