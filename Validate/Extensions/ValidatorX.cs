using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Validate.ValidationExpressions;

namespace Validate.Extensions
{
    /// <summary>
    /// Validator extension methods.
    /// </summary>
    public static class ValidatorX
    {
        public static Validator<T> IsNotNull<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, string message = null) where U : class
        {
            var validationMessage = new ValidationMessage(message);
            var validationExpression = new IsNotNullTargetMemberExpression<T, U>(selector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsNotNullOrEmpty<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, string message = null) where U : IEnumerable
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsNotNullOrEmptyTargetMemberExpression<T, U>(selector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsNull<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, string message = null) where U : class
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsNullTargetMemberExpression<T, U>(selector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsNullOrEmpty<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, string message = null) where U : IEnumerable
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsNullOrEmptyTargetMemberExpression<T, U>(selector, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsEqualTo<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, U equalTo, string message = null)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsEqualToTargetMemberExpression<T, U>(selector, equalTo, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsNotEqualTo<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, U notEqualTo, string message = null)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsNotEqualToTargetMemberExpression<T, U>(selector, notEqualTo, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsGreaterThan<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, U greaterThanValue, string message = null) where U : IComparable
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsGreaterThanTargetMemberExpression<T, U>(selector, greaterThanValue, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsLesserThan<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, U lesserThanValue, string message = null) where U : IComparable
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsLesserThanTargetMemberExpression<T, U>(selector, lesserThanValue, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsBetween<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, U greaterThanOrEqualToValue, U lesserThanOrEqualToValue, string message = null) where U : IComparable
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsBetweenTargetMemberExpression<T, U>(selector, lesserThanOrEqualToValue, greaterThanOrEqualToValue, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsNotBetween<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, U greaterThanOrEqualToValue, U lesserThanOrEqualToValue, string message = null) where U : IComparable
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsNotBetweenTargetMemberExpression<T, U>(selector, lesserThanOrEqualToValue, greaterThanOrEqualToValue, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsTrue<T>(this Validator<T> validator, Expression<Func<T,bool>> predicate, string message = null)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsTrueTargetMemberExpression<T>(predicate, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsFalse<T>(this Validator<T> validator, Expression<Func<T, bool>> predicate, string message = null)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsFalseTargetMemberExpression<T>(predicate, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> Or<T>(this Validator<T> validator, string message, params Func<T,Validator>[] nestedValidators)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new OrTargetMemberExpression<T>(validationMessage, nestedValidators);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }
        
        public static Validator<T> Or<T>(this Validator<T> validator, string message, params Predicate<T>[] predicates)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new OrTargetMemberExpression<T>(validationMessage, predicates);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> And<T>(this Validator<T> validator, string message, params Func<T, Validator>[] nestedValidators)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new AndTargetMemberExpression<T>(validationMessage, nestedValidators);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> And<T>(this Validator<T> validator, string message , params Predicate<T>[] predicates)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new AndTargetMemberExpression<T>(validationMessage, predicates);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IfThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Predicate<T>[] predicates)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IfThenTargetMemberExpression<T>(ifThis, validationMessage, predicates);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IfThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Func<T, Validator>[] nestedValidators)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IfThenTargetMemberExpression<T>(ifThis, validationMessage, nestedValidators);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IfNotThen<T>(this Validator<T> validator, Predicate<T> ifNotThis, string message, params Predicate<T>[] predicates)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IfNotThenTargetMemberExpression<T>(ifNotThis, validationMessage, predicates);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IfNotThen<T>(this Validator<T> validator, Predicate<T> ifNotThis, string message, params Func<T, Validator>[] nestedValidators)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IfNotThenTargetMemberExpression<T>(ifNotThis, validationMessage, nestedValidators);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> MatchesRegex<T>(this Validator<T> validator, Expression<Func<T,string>> selector, string regexPattern, RegexOptions regexOptions = RegexOptions.IgnoreCase, string message = null)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new MatchesRegexTargetMemberExpression<T>(selector, regexPattern, regexOptions, validationMessage);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> Contains<T, U, V>(this Validator<T> validator, Expression<Func<T, U>> selector, string message = null, params V[] values) where U : IEnumerable<V>
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new ContainsTargetMemberExpression<T,U,V>(selector, validationMessage, values);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static Validator<T> IsOneOf<T, U>(this Validator<T> validator, Expression<Func<T, U>> selector, string message = null, params U[] oneOfValues)
        {
            var validationMessage = new ValidationMessage(message);

            var validationExpression = new IsOneOfTargetMemberExpression<T,U>(selector, validationMessage, oneOfValues);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }

        public static void Throw(this Validator validator)
        {
            Func<Validator, Validator> validation = (v) =>
            {
                if (!v.IsValid)
                    v.ValidationResultToExceptionTransformer.Throw();
                return v;
            };

            validation(validator);
        }
    }
}
