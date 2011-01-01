using System.Linq.Expressions;

namespace Validate
{
    public static class GenericX
    {
        public static Validator<T> Validate<T>(this T obj, ValidationOptions options = null)
        {
            return new Validator<T>(obj, options: options);
        }
    }
}