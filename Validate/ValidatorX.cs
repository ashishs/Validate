using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Validate
{
    /// <summary>
    /// Validator extension methods.
    /// </summary>
    public static class ValidatorX
    {
        public static Validator<T> IsNotNull<T, U>(this Validator<T> validator, Func<T, U> selector, string message) where U : class
        {
            Func <Validator<T> ,Validator<T>> validation = (v) =>
                {
                    var target = selector(v.Target);
                    if (target == null)
                        v.AddError(new ValidationError(message, target, cause: message));
                    return v;
                };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNotNullOrEmpty<T, U>(this Validator<T> validator, Func<T, U> selector, string message) where U : IEnumerable
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target == null || target.OfType<object>().Count() == 0)
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNull<T, U>(this Validator<T> validator, Func<T, U> selector, string message) where U : class
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target != null)
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNullOrEmpty<T, U>(this Validator<T> validator, Func<T, U> selector, string message) where U : IEnumerable
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (!(target == null || target.OfType<object>().Count() == 0))
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsEqualTo<T, U>(this Validator<T> validator, Func<T, U> selector, U equalTo, string message)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (!target.Equals(equalTo))
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNotEqualTo<T, U>(this Validator<T> validator, Func<T, U> selector, U notEqualTo, string message)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target.Equals(notEqualTo))
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsGreaterThan<T, U>(this Validator<T> validator, Func<T, U> selector, U greaterThanValue, string message) where U : IComparable
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target.CompareTo(greaterThanValue) <= 0)
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsLesserThan<T, U>(this Validator<T> validator, Func<T, U> selector, U lesserThanValue, string message) where U : IComparable
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target.CompareTo(lesserThanValue) >= 0)
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsBetween<T, U>(this Validator<T> validator, Func<T, U> selector, U greaterThanOrEqualToValue, U lesserThanOrEqualToValue, string message) where U : IComparable
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target.CompareTo(lesserThanOrEqualToValue) > 0 || target.CompareTo(greaterThanOrEqualToValue) < 0)
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsNotBetween<T, U>(this Validator<T> validator, Func<T, U> selector, U greaterThanOrEqualToValue, U lesserThanOrEqualToValue, string message) where U : IComparable
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = selector(v.Target);
                if (target.CompareTo(lesserThanOrEqualToValue) <= 0 && target.CompareTo(greaterThanOrEqualToValue) >= 0)
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsTrue<T>(this Validator<T> validator, Predicate<T> predicate, string message)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = v.Target;
                if (!predicate(target))
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IsFalse<T>(this Validator<T> validator, Predicate<T> predicate, string message)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = v.Target;
                if (predicate(target))
                    v.AddError(new ValidationError(message, target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> Or<T>(this Validator<T> validator, string message, params Validator[] validators)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var match = validators.Any(val => val.IsValid);
                if (!match)
                    v.AddError(new ValidationError(message, v.Target, cause: GetCauses(validators).Join(" And ")));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> Or<T>(this Validator<T> validator, string message, params Predicate<T>[] predicates)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var match = predicates.Any(p => p(v.Target));
                if (!match)
                    validator.AddError(new ValidationError(message, v.Target, cause: "{{ None of the predicates returned true. }}"));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> And<T>(this Validator<T> validator, string message, params Validator[] validators)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var match = validators.All(val => val.IsValid);
                if (!match)
                    v.AddError(new ValidationError(message, v.Target, cause: GetCauses(validators).Join(" Or ")));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> And<T>(this Validator<T> validator, string message, params Predicate<T>[] predicates)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var match = predicates.All(p => p(v.Target));
                if (!match)
                    v.AddError(new ValidationError(message, v.Target, cause: "{{ Atleast one of the predicates returned false. }}"));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IfThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Predicate<T>[] predicates)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                if (ifThis(v.Target) && predicates.Select(p => p).FirstOrDefault(p => !p(v.Target)) != null)
                    v.AddError(new ValidationError(message, v.Target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IfThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Validator[] validators)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var match = ifThis(v.Target);
                if (match && validators.Any(val => !val.IsValid))
                    v.AddError(new ValidationError(message, v.Target, cause: GetCauses(validators).Join(" ")));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IfNotThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Predicate<T>[] predicates)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                if (!ifThis(v.Target) && predicates.Select(p => p).FirstOrDefault(p => !p(v.Target)) != null)
                    v.AddError(new ValidationError(message, v.Target, cause: message));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
        }

        public static Validator<T> IfNotThen<T>(this Validator<T> validator, Predicate<T> ifThis, string message, params Validator[] validators)
        {
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var match = ifThis(v.Target);
                if (!match && validators.Any(val => !val.IsValid))
                    validator.AddError(new ValidationError(message, v.Target, cause: GetCauses(validators).Join(" ")));
                return v;
            };
            validator.AddValidation(validation, message);
            return validation.ExecuteInValidationBlock(validator, message);
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

        private static List<string> GetCauses(IEnumerable<Validator> validators)
        {
            return validators.SelectMany(v => v.Errors.Select(e => "{{{0}}}".WithFormat(e.Cause))).ToList();
        }

        public static Validator<T> ExecuteInValidationBlock<T>(this Func<Validator<T>,Validator<T>> func, Validator<T> validator, string message)
        {
            try
            {
                return validator.ValidateFurther ? func(validator) : validator;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                validator.AddError(new ValidationError(message, validator.Target, cause: "{{{0} : Exception : {1}}}".WithFormat(message, ex.ToString())));
            }
            return validator;
        }

        public static Validator ExecuteInValidationBlock(this Func<Validator, Validator> func, Validator validator, string message)
        {
            try
            {
                return validator.ValidateFurther ? func(validator) : validator;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                validator.AddError(new ValidationError(message, validator.ValidationTarget, cause: "{{{0} : Exception : {1}}}".WithFormat(message, ex.ToString())));
            }
            return validator;
        }
    }
}
