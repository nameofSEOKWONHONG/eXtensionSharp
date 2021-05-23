using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
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
                if ((obj as ICollection).Count <= 0)
                    return true;
            }

            return false;
        }

        public static bool xIsNotEmpty(this object obj) {
            return !obj.xIsEmpty();
        }

        public static string xValue(this string src, string @default = null) {
            return src.xIfNullOrEmpty(x => @default).xTrim();
        }

        public static string xValue(this object src, object @default = null) {
            if (src.xIsNull() && @default.xIsNull()) return string.Empty;
            if (src.xIsNotNull()) {
                return Convert.ToString(src).xTrim();
            }

            if (@default.xIsNotNull()) {
                return Convert.ToString(@default).xTrim();
            }

            return string.Empty;
        }
        
        public static T xValue<T>(this object src, object @default = null) {
            if (src.xIsNull() && @default.xIsNull()) return default;
            if (src.xIsNotNull()) {
                return (T)Convert.ChangeType(src, typeof(T));
            }

            if (@default.xIsNotNull()) {
                return (T)Convert.ChangeType(@default, typeof(T));
            }

            return default;
        }

        public static T xValue<T>(this string src) where T : struct {
            return src.xStringToEnum<T>();
        }
        
        public static string xValue<T>(this XENUM_BASE<T> src, XENUM_BASE<T> defaultValue = null) where T : XENUM_BASE<T>, new() {
            if (defaultValue.xIsNotNull()) {
                return src.Value.xIfNotNull(x => x, defaultValue.Value);    
            }

            return src.Value;
        }

        public static XENUM_BASE<T> xValue<T>(this string src, XENUM_BASE<T> defaultValue = null) where T : XENUM_BASE<T>, new() {
            if (src.xIsNullOrEmpty()) {
                return (XENUM_BASE<T>)src; 
            }
            
            if (defaultValue.xIsNotNull()) {
                return defaultValue;
            }

            return null;
        }

        public static string xTrim(this string src) {
            return src.xIsNullOrEmpty() ? string.Empty : src.Trim();
        }

        public static bool xIsEquals<T>(this T src, T compare) {
            return src.Equals(compare);
        }
        
        public static bool xIsEquals<T>(this T src, IEnumerable<T> compares) {
            var isEqual = false;
            compares.xForEach(item => {
                isEqual = item.xIsEquals(src);
                return !isEqual;
            });

            return isEqual;
        }

        public static bool xIsEquals<T>(this IEnumerable<T> srcs, T compare) {
            return compare.xIsEquals(srcs);
        }

        public static bool xContains(this string src, IEnumerable<string> compares) {
            var isContain = false;
            compares.xForEach(compare => {
                isContain = src.Contains(compare);
                if (isContain) return false;
                return true;
            });

            return isContain;
        }
        
        public static T xFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.xIsNotNull()) return enumerable.AsValueEnumerable().FirstOrDefault(predicate);

            return enumerable.AsValueEnumerable().FirstOrDefault();
        }

        public static T xLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.xIsNotNull()) return enumerable.AsValueEnumerable().LastOrDefault(predicate);

            return enumerable.AsValueEnumerable().LastOrDefault();
        }        
        
        public static XList<T> xToList<T>(this IEnumerable<T> enumerable) {
            return enumerable == null ? new XList<T>() : new XList<T>(enumerable);
        }

        public static T[] xToArray<T>(this IEnumerable<T> enumerable) where T : new() {
            if (enumerable.xIsNull()) return new T[0];
            return enumerable.AsValueEnumerable().ToArray();
        }

        public static string xToHash(this string str) {
            return str.GetHashCode().ToString();
        }

        public static string xToName(this Type type) {
            return type.Name;
        }
    }
}