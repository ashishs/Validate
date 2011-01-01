using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Validate
{
    public static class ValidatorX
    {
        public static Validator<T> IsNotNull<T, U>(this Validator<T> validator, Func<T, U> selector, string message) where U: class 
        {
            Func<Validator<T>> validation = () =>
                {
                    var target = selector(validator.Target);
                    if (target == null)
                        validator.Errors.Add(new ValidationError(message, target, cause: message));
                    return validator;
                };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNull<T, U>(this Validator<T> validator, Func<T, U> selector, string message) where U : class
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (target != null)
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsEqualTo<T, U>(this Validator<T> validator, Func<T, U> selector, U equalTo, string message)
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (!target.Equals(equalTo))
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNotEqualTo<T, U>(this Validator<T> validator, Func<T, U> selector, U equalTo, string message)
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (target.Equals(equalTo))
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsGreaterThan<T, U>(this Validator<T> validator, Func<T, U> selector, U greaterThanValue, string message) where U : IComparable
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (target.CompareTo(greaterThanValue) <= 0)
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsLessThan<T, U>(this Validator<T> validator, Func<T, U> selector, U lesserThanValue, string message) where U : IComparable
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (target.CompareTo(lesserThanValue) >= 0)
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsBetween<T, U>(this Validator<T> validator, Func<T, U> selector, U greaterThanOrEqualToValue, U lesserThanOrEqualToValue, string message) where U : IComparable
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (target.CompareTo(lesserThanOrEqualToValue) > 0 || target.CompareTo(greaterThanOrEqualToValue) < 0)
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNotBetween<T, U>(this Validator<T> validator, Func<T, U> selector, U greaterThanOrEqualToValue, U lesserThanOrEqualToValue, string message) where U : IComparable
        {
            Func<Validator<T>> validation = () =>
            {
                var target = selector(validator.Target);
                if (target.CompareTo(lesserThanOrEqualToValue) <= 0 && target.CompareTo(greaterThanOrEqualToValue) >= 0)
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsTrue<T>(this Validator<T> validator, Predicate<T> predicate, string message)
        {
            Func<Validator<T>> validation = () =>
            {
                var target = validator.Target;
                if (!predicate(target))
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsFalse<T>(this Validator<T> validator, Predicate<T> predicate, string message)
        {
            Func<Validator<T>> validation = () =>
            {
                var target = validator.Target;
                if (predicate(target))
                    validator.Errors.Add(new ValidationError(message, target, cause: message));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> Or<T>(this Validator<T> validator, string message, params Validator[] validators)
        {
            Func<Validator<T>> validation = () =>
            {
                var match =  validators.Any(v => v.IsValid);
                if(!match)
                    validator.Errors.Add(new ValidationError(message, validator.Target, cause: GetCauses(validators).Join(" And ")));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> And<T>(this Validator<T> validator, string message, params Validator[] validators)
        {
            Func<Validator<T>> validation = () =>
            {
                var match = validators.All(v => v.IsValid);
                if (!match)
                    validator.Errors.Add(new ValidationError(message, validator.Target, cause: GetCauses(validators).Join(" Or ")));
                return validator;
            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IfThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Predicate<T>[] predicates)
        {
            Func<Validator<T>> validation = () =>
            {
                if(ifThis(validator.Target) && predicates.Select(p => p).FirstOrDefault(p => !p(validator.Target)) != null)
                    validator.Errors.Add(new ValidationError(message, validator.Target, cause: message));
                return validator;

            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IfNotThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Predicate<T>[] predicates)
        {
            Func<Validator<T>> validation = () =>
            {
                if (!ifThis(validator.Target) && predicates.Select(p => p).FirstOrDefault(p => !p(validator.Target)) != null)
                    validator.Errors.Add(new ValidationError(message, validator.Target, cause: message));
                return validator;

            };
            return validation.ExecuteInValidationBlock(validator, message);
        }

        private static List<string> GetCauses(IEnumerable<Validator> validators)
        {
            return validators.SelectMany(v => v.Errors.Select(e => "{{{0}}}".WithFormat(e.Cause))).ToList();
        }

        public static Validator<T> ExecuteInValidationBlock<T>(this Func<Validator<T>> func, Validator<T> validator, string message)
        {
            try
            {
                return validator.ValidateFurther ? func() : validator;
            }
            catch (Exception)
            {
                validator.Errors.Add(new ValidationError(message, validator.Target, cause: message));
                return validator;
            }
        }
    }
}