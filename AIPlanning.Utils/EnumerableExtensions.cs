using System.Collections.Generic;

namespace AIPlanning.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T element)
        {
            yield return element;
        }
    }
}