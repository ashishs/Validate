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
            if (typeof(Validator).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("Validators cannot be validated using validate extensions.");
            return new Validator<T>(obj, options: options);
        }
    }
}