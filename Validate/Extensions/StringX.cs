using System.Collections.Generic;
using System.Linq;

namespace Validate.Extensions
{
    public static class StringX
    {
        /// <summary>
        /// Joins multiple strings together using a separator
        /// </summary>
        public static string Join(this IEnumerable<string > enumerable, string separator)
        {
            if (enumerable.IsNullOrEmpty())
                return string.Empty;
            return string.Join(separator, enumerable.ToArray());
        }

        public static string WithFormat(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        public static bool EqualsIgnoreCase(this string s, string other)
        {
            return string.Compare(s, other, true) == 0;
        }
    }
}