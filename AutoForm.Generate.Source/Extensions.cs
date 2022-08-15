using System.Collections.Generic;

namespace AutoForm.Generate
{
    internal static class Extensions
    {
        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> enumeration, IEnumerable<T> range)
        {
            foreach(var original in enumeration)
            {
                yield return original;
            }
            foreach(var extra in range)
            {
                yield return extra;
            }
        }
    }
}
