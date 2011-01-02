using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validate
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
    }
}