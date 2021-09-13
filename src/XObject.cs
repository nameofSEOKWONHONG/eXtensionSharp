using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eXtensionSharp
{
    public static class XObject
    {
        public static void xIfTrue(this bool obj, Action action)
        {
            if (obj) action();
        }

        public static void xIfFalse(this bool obj, Action action)
        {
            if (!obj) action();
        }

        public static bool xIsTrue(this bool obj)
        {
            return obj.Equals(true);
        }

        public static bool xIsFalse(this bool obj)
        {
            return obj.Equals(false);
        }

        public static string xIfNullOrEmpty(this string str, Func<string, string> func)
        {
            if (str.xIsNullOrEmpty()) return func(str);
            return str;
        }

        public static string xIfNotNullOrEmpty(this string str, Func<string, string> func)
        {
            if (!str.xIsNullOrEmpty()) return func(str);
            return str;
        }

        public static T xIfNull<T>(this T obj, Func<T, T> predicate)
        {
            if (predicate.xIsNull()) return predicate(obj);
            return obj;
        }

        public static void xIf<T>(this T obj, Action<T> predicate)
        {
            if (obj.xIsNotNull()) predicate(obj);
        }

        public static T xIfNotNull<T>(this T obj, Func<T, T> predicate, T defaultValue)
        {
            if (obj.xIsNotNull()) return predicate(obj);
            return defaultValue;
        }

        public static bool xIsNull(this object obj)
        {
            if (obj == null) return true;
            return false;
        }

        public static bool xIsNotNull(this object obj)
        {
            if (obj == null) return false;
            return true;
        }

        public static bool xIsEmpty(this object obj)
        {
            if (obj.xIsNull())
            {
                return true;
            }

            if (obj is string)
            {
                if ((obj as string).xIsNullOrEmpty()) return true;
            }
            else if (obj is ICollection)
            {
                if ((obj as ICollection).Count <= 0)
                    return true;
            }

            return false;
        }

        public static bool xIsNotEmpty(this object obj)
        {
            return !obj.xIsEmpty();
        }

        public static string xGetValue(this string src, string @default = null)
        {
            return src.xIfNullOrEmpty(x => @default).xTrim();
        }

        public static string xGetValue(this object src, object @default = null)
        {
            if (src.xIsNull() && @default.xIsNull()) return string.Empty;
            if (src.xIsNotNull()) return Convert.ToString(src).xTrim();

            if (@default.xIsNotNull()) return Convert.ToString(@default).xTrim();

            return string.Empty;
        }

        public static T xGetValue<T>(this object src, object @default = null)
        {
            if (src.xIsNull() && @default.xIsNull()) return default;

            if (src.xIsNotNull()) return (T) Convert.ChangeType(src, typeof(T));

            if (@default.xIsNotNull()) return (T) Convert.ChangeType(@default, typeof(T));

            return default;
        }

        public static T xGetValue<T>(this T src, T @default = null) where T : class, new()
        {
            if (src.xIsNull() && @default.xIsNotNull())
                return @default;
            if (src.xIsNull() && @default.xIsNull()) return new T();

            return src;
        }

        public static string xGetValue<T>(this XEnumBase<T> src, XEnumBase<T> defaultValue = null)
            where T : XEnumBase<T>, new()
        {
            if (defaultValue.xIsNotNull()) return src.ToString().xIfNotNull(x => x, defaultValue.ToString());

            return src.ToString();
        }

        public static XEnumBase<T> xGetValue<T>(this string src, XEnumBase<T> defaultValue = null)
            where T : XEnumBase<T>, new()
        {
            if (src.xIsNullOrEmpty()) return XEnumBase<T>.Parse(src);

            if (defaultValue.xIsNotNull()) return defaultValue;

            return null;
        }

        public static string xTrim(this string src)
        {
            return src.xIsNullOrEmpty() ? string.Empty : src.Trim();
        }

        public static bool xIsEquals<T>(this T src, T compare)
        {
            return src.Equals(compare);
        }

        public static bool xIsEquals<T>(this T src, IEnumerable<T> compares)
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
        {
            return compare.xIsEquals(srcs);
        }

        public static bool xContains(this string src, IEnumerable<string> compares)
        {
            var isContain = false;
            compares.xForEach(compare =>
            {
                isContain = src.Contains(compare);
                if (isContain) return false;
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

        public static string xToHash(this string str)
        {
            return str.GetHashCode().ToString();
        }

        public static string xToName(this Type type)
        {
            return type.Name;
        }
    }
}