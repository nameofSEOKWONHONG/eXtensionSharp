using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
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

        /// <summary>
        /// XisEmpty dose not support number type.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool xIsEmpty<T>(this T obj)
        {
            if (obj.xIsNull())
            {
                return true;
            }

            if (obj is string v)
            {
                return string.IsNullOrWhiteSpace(v);
            }

            if (obj.xIsDateTime())
            {
                var t = obj.xAs<DateTime>();
                if (t <= DateTime.MinValue) return true;
            }

            // collection type
            switch (obj)
            {
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                // ReSharper disable once HeapView.PossibleBoxingAllocation
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return true;

                default: return false;
            }
        }

        public static bool xIsDateTime<T>(this T obj)
        {
            var type = typeof(T);
            return type == typeof(DateTime);
        }

        public static bool xIsNotEmpty<T>(this T obj)
        {
            return !obj.xIsEmpty();
        }

        public static bool xIsEmptyNumber<T>(this T number)
            where T : INumber<T>
        {
            T zero = default;
            return number <= zero;
        }

        public static bool xIsNotEmptyNumber<T>(this T number)
            where T : INumber<T>
        {
            return !number.xIsEmptyNumber();
        }

        public static bool xIsSame<T>(this T src, T compare)
        {
            if (src.xIsEmpty()) return false;
            if (compare.xIsEmpty()) return false;
            return src.Equals(compare);
        }

        public static bool xIsNotSame<T>(this T src, T compare)
        {
            return !src.xIsSame(compare);
        }

        public static bool IsNullableType<T>(this T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type).xIsNotEmpty();
        }

        public static bool xIf(this string item, string match)
        {
            if (item.xIsSame(match)) return true;
            return false;
        }

        public static string xIf(this string item, string @case, Func<string> match, Func<string> notMatch)
        {
            if (item.xIsSame(@case)) return match();
            return notMatch();
        }

        #endregion [xIs Series]
    }
}