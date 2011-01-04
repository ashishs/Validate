using System.Collections.Generic;
using System.Linq;

namespace Validate.Extensions
{
    public static class EnumerableX
    {
        /// <summary>
        /// Checks if this enumarable is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {   
            return enumerable == null || enumerable.Count() == 0;
        }
    }
}