using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetFabric.Hyperlinq;

namespace eXtensionSharp {
    public static class XObject {
        public static void xIfTrue(this bool obj, Action action) {
            if (obj) action();
        }

        public static void xIfFalse(this bool obj, Action action) {
            if (!obj) action();
        }

        public static bool xIsTrue(this bool obj) {
            return obj.Equals(true);
        }

        public static bool xIsFalse(this bool obj) {
            return obj.Equals(false);
        }
        
        public static string xIfNullOrEmpty(this string str, Func<string, string> func) {
            if (str.xIsNullOrEmpty()) return func(str);
            return str;
        }

        public static string xIfNotNullOrEmpty(this string str, Func<string, string> func) {
            if (!str.xIsNullOrEmpty()) return func(str);
            return str;
        }

        public static T xIfNull<T>(this T obj, Func<T, T> predicate) {
            if (predicate.xIsNull()) return predicate(obj);
            return obj;
        }

        public static T xIfNotNull<T>(this T obj, Func<T, T> predicate, T defaultValue) {
            if (obj.xIsNotNull()) return predicate(obj);
            return defaultValue;
        }        

        public static bool xIsNull(this object obj) {
            if (obj == null) return true;
            return false;
        }

        public static bool xIsNotNull(this object obj) {
            if (obj == null) return false;
            return true;
        }

        public static bool xIsEmpty(this object obj) {
            if (obj.xIsNull()) {
                return true;
            }
            else if (obj is string) {
                if ((obj as string).xIsNullOrEmpty()) return true;    
            }
            else if (obj is ICollection) {
                if ((obj as ICollection).Count > 0)
                    return true;
            }

            return false;
        }

        public static string xToValue(this string src, string @default = null) {
            return src.xIfNullOrEmpty(x => @default);
        }

        public static string xToValue(this object src, object @default = null) {
            return Convert.ToString(src);
        }
        
        public static T xToValue<T>(this object src, object @deafult = null) {
            if (src is string) {
                if (string.IsNullOrEmpty(src as string)) {
                    return (T)Convert.ChangeType(@deafult, typeof(T));
                }
            }
            return (T)Convert.ChangeType(src, typeof(T));
        }
        
        public static string xToValue<T>(this XENUM_BASE<T> src, XENUM_BASE<T> @default = null) where T : XENUM_BASE<T>, new() {
            return src.Value.xIfNotNull(x => x, @default.Value);
        }

        public static bool xIsEquals<T>(this T src, T compare) {
            return src.Equals(compare);
        }
        
        public static bool xIsEquals<T>(this T src, IEnumerable<T> compares)
            where T : struct {
            var isEqual = false;
            compares.xForEach(item => {
                isEqual = item.xIsEquals(src);
                return !isEqual;
            });

            return isEqual;
        }

        public static bool xIsEquals<T>(this IEnumerable<T> srcs, T compare) 
            where T: struct {
            return compare.xIsEquals(srcs);
        }

        

        public static bool xIsEquals(this string src, string compare) {
            return src.Equals(compare);
        }
        
        public static bool xIsEquals(this string src, IEnumerable<string> compares) {
            var isEqaul = false;
            compares.xForEach(item => {
                isEqaul = src.xIsEquals(item);
                return !isEqaul;
            });

            return isEqaul;
        }

        public static bool xIsEquals(this IEnumerable<string> srcs, string compare) {
            return compare.xIsEquals(srcs);
        }
        
        public static T xFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.xIsNotNull()) return enumerable.AsValueEnumerable().FirstOrDefault(predicate);

            return enumerable.AsValueEnumerable().FirstOrDefault();
        }

        public static T xLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.xIsNotNull()) return enumerable.AsValueEnumerable().LastOrDefault(predicate);

            return enumerable.AsValueEnumerable().LastOrDefault();
        }        
        
        public static IEnumerable<T> xToList<T>(this IEnumerable<T> enumerable) {
            return enumerable.xIfNotNull(x => x, new XList<T>());
        }

        public static T[] xToArray<T>(this IEnumerable<T> enumerable) where T : new() {
            if (enumerable.xIsNull()) return new T[0];
            return enumerable.AsValueEnumerable().ToArray();
        }        
    }
}