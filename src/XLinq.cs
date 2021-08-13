using System;
using System.Collections.Generic;
using System.Linq;

namespace eXtensionSharp
{
    public static class XLinq
    {
        public static int xCount<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.xIsNull()) return 0;
            return enumerable.Count();
        }

        public static IEnumerable<T> xWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class
        {
            return xNullToNew(enumerable).Where(predicate);
        }

        public static IEnumerable<T> xSelect<T>(this IEnumerable<T> enumerable, Func<T, T> predicate)
            where T : class
        {
            return xNullToNew(enumerable).Select(predicate);
        }

        private static IEnumerable<T> xNullToNew<T>(IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty())
            {
                enumerable = new List<T>();
                return enumerable;
            }

            return enumerable;
        }
    }
}