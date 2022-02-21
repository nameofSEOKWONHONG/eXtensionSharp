using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

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

        public static void xIfEmpty(this string str, Action @if, Action @else = null)
        {
            if (str.xIsEmpty()) @if();
            else
            {
                if (@else.xIsNotEmpty()) @else();
            }
        }

        public static void xIfEmpty<T>(this T obj, Action @if, Action @else = null)
        {
            if (obj.xIsEmpty()) @if();
            else
            {
                if (@else.xIsNotEmpty()) @else();
            }
        }
        
        public static string xIfEmpty(this string str, Func<string> func)
        {
            if (str.xIsEmpty()) return func();
            return str;
        }

        /// <summary>
        /// 일반 예외
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="Exception"></exception>
        public static void xIfEmptyThrow<T>(this T obj, Func<string> func)
        {
            if (obj.xIsEmpty())
            {
                var msg = func();
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// 지정 예외
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        public static void xIfEmptyThorw<T, TException>(this T obj, Func<TException> func) where TException : Exception
        {
            obj.xIfEmpty(() => func());
        }

        public static void xIfNotEmpty(this string str, Action @if, Action @else = null)
        {
            if (str.xIsNotEmpty()) @if();
            else
            {
                if (@else.xIsNotEmpty()) @else();
            }
        }

        public static string xIfNotEmpty(this string str, Func<string> func)
        {
            if (str.xIsNotEmpty()) return func();
            return str;
        }

        public static void xIfNotEmpty<T>(this T obj, Action @if, Action @else = null)
        {
            if (obj.xIsNotEmpty()) @if();
            else
            {
                if (@else.xIsNotEmpty()) @else();
            }
        }
        
        public static async Task xIfNotEmptyAsync<T>(this T obj, Func<Task> func)
        {
            if (obj.xIsNotEmpty()) await func();   
        }

        public static async Task xIfEmptyAsync<T>(this T obj, Func<Task> func)
        {
            if (obj.xIsNotEmpty()) await func();
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
                if ((obj as string).xIsEmpty()) return true;
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

        public static string xTrim(this string src)
        {
            return src.xIsEmpty() ? string.Empty : src.Trim();
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

        public static T xDatabaseTry<T>(this IDbConnection connection, Func<IDbConnection, T> dbConnection)
        {
            T result = default;
            if(connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                result = dbConnection(connection);
                return result;
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<T> xDatabaseTryAsync<T>(this IDbConnection connection,
            Func<IDbConnection, Task<T>> dbConnection)
        {
            T result = default;
            if(connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                result = await dbConnection(connection);
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}