using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace eXtensionSharp {
    public static class XLinq {
        public static int xCount<T>(this IEnumerable<T> enumerable) {
            return enumerable.AsValueEnumerable().Count();
        }
        
        public static IEnumerable<T> xWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class {
            if (enumerable.xIsEmpty()) {
                enumerable = new XList<T>();
                return enumerable;
            }

            return enumerable.AsValueEnumerable().Where(predicate);
        }

        public static IEnumerable<T> xSelect<T>(this IEnumerable<T> enumerable, Func<T, T> predicate)
            where T : class {
            return enumerable.AsValueEnumerable().Select(predicate);
        }
    }
}