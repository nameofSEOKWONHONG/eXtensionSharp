using System;
using System.Collections.Generic;
using System.Linq;

namespace eXtensionSharp
{
    public static class XLinq
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
    }
}