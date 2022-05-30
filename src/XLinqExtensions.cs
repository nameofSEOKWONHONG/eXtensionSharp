using System;
using System.Collections.Generic;
using System.Linq;

namespace eXtensionSharp
{
    public static class XLinqExtensions
    {
        public static int xCount<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty()) return 0;
            return enumerable.Count();
        }

        public static IEnumerable<T> xWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class
        {
            return CreateNew(enumerable).Where(predicate);
        }

        public static IEnumerable<T> xSelect<T>(this IEnumerable<T> enumerable, Func<T, T> predicate)
            where T : class
        {
            return CreateNew(enumerable).Select(predicate);
        }

        private static IEnumerable<T> CreateNew<T>(IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty())
            {
                return new List<T>();
            }

            return enumerable;
        }
        
        public static bool xContains(this string src, IEnumerable<string> compares)
        {
            var isContain = true;
            compares.xForEach(compare =>
            {
                isContain = src.Contains(compare);
                if (!isContain) return false;
                return true;
            });

            return isContain;
        }

        public static bool xContains<T>(this T src, IEnumerable<T> compares)
        {
            return compares.Contains(src);
        }

        public static bool xContains<T>(this IEnumerable<T> src, IEnumerable<T> compares)
        {
            var isContain = true;
            src.xForEach(item =>
            {
                isContain = item.xContains(compares);
                if (!isContain) return false;
                return true;
            });

            return isContain;
        }

        public static T xFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotNull()) return enumerable.FirstOrDefault(predicate);

            return enumerable.FirstOrDefault();
        }

        public static T xLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotNull()) return enumerable.LastOrDefault(predicate);

            return enumerable.LastOrDefault();
        }

        public static List<T> xToList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? new List<T>() : new List<T>(enumerable);
        }

        public static T[] xToArray<T>(this IEnumerable<T> enumerable) where T : new()
        {
            if (enumerable.xIsNull()) return new T[0];
            return enumerable.ToArray();
        }
    }
}