using System;

namespace Validate.Extensions
{
    public static class GenericX
    {
        /// <summary>
        /// Creates a validator for validating this object
        /// </summary>
        /// <typeparam name="T">The type of this object</typeparam>
        /// <param name="obj">The object to be validated</param>
        /// <param name="options">Optional behaviour for the validator</param>
        /// <returns>An instance of the Validator class</returns>
        public static Validator<T> Validate<T>(this T obj, ValidationOptions options = null)
        {
            if (typeof(AbstractValidator).IsAssignableFrom(typeof(T)) || typeof(IValidationMetadata).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("Validators/Validations cannot be validated using validate extensions.");
            
            return new Validator<T>(obj, options: options);
        }

        /// <summary>
        /// Creates a validator for validating this object
        /// </summary>
        /// <typeparam name="T">The type of this object</typeparam>
        /// <param name="obj">The object to be validated</param>
        /// <param name="validationAlias">The named validation to run on this object</param>
        /// <returns>An instance of the Validator class</returns>
        public static Validator<T> ValidateUsing<T>(this T obj, string validationAlias)
        {
            var validationRepository = new ValidationRepositoryFactory().GetValidationRepository();
            var validation = validationRepository.Get<T>(validationAlias);
            return validation.RunAgainst(obj);
        }

        public static Validator<T> ValidateUsing<T>(this T obj, Type abstractClassValidator)
        {
            var classValidator = (AbstractClassValidator<T>) Activator.CreateInstance(abstractClassValidator, obj);
            return classValidator.Validate();
        }
    }
}