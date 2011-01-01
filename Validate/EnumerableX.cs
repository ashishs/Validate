using System.Collections.Generic;
using System.Linq;

namespace Validate
{
    public static class EnumerableX
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {   
            return enumerable == null || enumerable.Count() == 0;
        }
    }
}