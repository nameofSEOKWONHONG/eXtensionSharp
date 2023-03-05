using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace eXtensionSharp
{
    public static class XIsIfExtensions
    {
        #region [xIs Series]

        public static bool xIsNull<T>(this T obj)
        {
            return obj is null;
        }

        public static bool xIsNotNull<T>(this T obj)
        {
            return obj is not null;
        }

        public static bool xIsTrue(this bool state)
        {
            return state is true;
        }

        public static bool xIsFalse(this bool state)
        {
            return state is false;
        }

        public static bool xIsEmpty<T>(this T obj)
        {
            if (obj.xIsNull())
            {
                return true;
            }
            switch (obj) {
                case null: return true;
                case string s when string.IsNullOrWhiteSpace(s):
                case ICollection {Count: 0}:
                case Array {Length: 0}:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return true;
                default: return false;
            }
        }
        
        public static bool xIsNotEmpty<T>(this T obj)
        {
            return !obj.xIsEmpty();
        }

        public static bool xIsEmpty(this DateTime dt)
        {
            return dt <= DateTime.MinValue;
        }

        public static bool xIsNotEmpty(this DateTime dt)
        {
            return !dt.xIsEmpty();
        }

        public static bool xIsEmptyNum<T>(this T number)
            where T : INumber<T>
        {
            T zero = default;
            return number <= zero;
        }

        public static bool xIsNotEmptyNum<T>(this T number)
            where T : INumber<T>
        {
            return !number.xIsEmptyNum();
        }

        public static bool xIsEquals<T>(this T src, T compare) where T : class
        {
            if (src.xIsEmpty()) return false;
            if (compare.xIsEmpty()) return false;
            return src.Equals(compare);
        }

        public static bool xIsEquals<T>(this T src, IEnumerable<T> compares)
            where T : class
        {
            var isEqual = false;
            compares.xForEach(item =>
            {
                isEqual = item.xIsEquals(src);
                return !isEqual;
            });

            return isEqual;
        }

        public static bool xIsEquals<T>(this IEnumerable<T> srcs, T compare)
            where T : class
        {
            if (srcs.xIsEmpty()) return false;
            if (compare.xIsEmpty()) return false;
            return compare.xIsEquals(srcs);
        }

        public static bool IsNullableType<T>(T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) != null;
        }

        #endregion [xIs Series]
    }
}