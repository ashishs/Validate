using System;
using System.Linq.Expressions;

namespace Validate
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
            if (typeof(Validator).IsAssignableFrom(typeof(T)) || typeof(IValidationMetadata).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("Validators/Validations cannot be validated using validate extensions.");
            
            return new Validator<T>(obj, options: options);
        }

        /// <summary>
        /// Creates a validator for validating this object
        /// </summary>
        /// <typeparam name="T">The type of this object</typeparam>
        /// <param name="obj">The object to be validated</param>
        /// <param name="validationAlias">The named validation to run on this object</param>
        /// <param name="validationRepositoryFactory">A validation repository factory. If none is given, the default implmentation is used which uses a in memory repository.</param>
        /// <returns>An instance of the Validator class</returns>
        public static Validator<T> ValidateUsing<T>(this T obj, string validationAlias, IValidationRepositoryFactory validationRepositoryFactory = null)
        {
            var validationRepository = validationRepositoryFactory == null ? new ValidationRepositoryFactory().GetValidationRepository() : validationRepositoryFactory.GetValidationRepository();
            var validation = validationRepository.Get<T>(validationAlias);
            return validation.RunAgainst(obj);
        }
    }
}