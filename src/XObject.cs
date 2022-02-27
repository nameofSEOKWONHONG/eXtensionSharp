using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XObject
    {
        public static bool xIsTrue(this bool state)
        {
            return state is true;
        }

        public static bool xIsFalse(this bool state)
        {
            return state is false;
        }
        
        public static void xIfTrue(this bool state, Action execute, Action elseExecute = null)
        {
            if (state) execute();
            else
            {
                elseExecute.xIfNotEmpty(elseExecute);
            }
        }

        public static void xIfFalse(this bool state, Action execute, Action elseExecute = null)
        {
            if (!state) execute();
            else
            {
                elseExecute.xIfNotEmpty(elseExecute);
            }
        }

        public static T xIfTrue<T>(this bool state, Func<T> execute, Func<T> elseExecute = null)
        {
            T t = default;
            state.xIfTrue(() =>
            {
                t = execute();
            }, () =>
            {
                elseExecute.xIfNotEmpty(() =>
                {
                    t = elseExecute();
                });
            });

            return t;
        }
        
        public static T xIfFalse<T>(this bool state, Func<T> execute, Func<T> elseExecute = null)
        {
            T t = default;
            state.xIfFalse(() =>
            {
                t = execute();
            }, () =>
            {
                elseExecute.xIfNotEmpty(() =>
                {
                    t = elseExecute();
                });
            });

            return t;
        }
        
        public static void xIfEmpty<T>(this T obj, Action execute, Action elseExecute = null)
        {
            if (obj.xIsEmpty()) execute();
            else
            {
                if (elseExecute.xIsNotEmpty()) elseExecute();
            }
        }
        
        public static T xIfEmpty<T>(this T obj, Func<T> execute, Func<T> elseExecute = null)
        {
            T t = default;
            if (obj.xIsEmpty())
            {
                t = execute();
            }
            else
            {
                elseExecute.xIfNotEmpty(() => t = elseExecute());
            }

            return t;
        }
        
        public static void xIfNotEmpty<T>(this T obj, Action execute, Action elseExecute = null)
        {
            if (obj.xIsNotEmpty()) execute();
            else
            {
                if (elseExecute.xIsNotEmpty()) elseExecute();
            }
        }

        public static T xIfNotEmpty<T>(this T obj, Func<T> execute, Func<T> elseExecute = null)
        {
            T t = default;
            obj.xIfNotEmpty(() =>
            {
                t = execute();
            }, () =>
            {
                elseExecute.xIfNotEmpty(() =>
                {
                    t = elseExecute();
                });
            });

            return t;
        }

        public static T2 xIfNotEmpty<T1, T2>(this T1 obj, Func<T2> execute, Func<T2> elseExecute = null)
        {
            T2 t = default;
            obj.xIfNotEmpty(() =>
            {
                t = execute();
            }, () =>
            {
                elseExecute.xIfNotEmpty(() => { t = elseExecute(); });
            });

            return t;
        }
        
        public static async Task xIfNotEmptyAsync<T>(this T obj, Func<Task> func)
        {
            if (obj.xIsNotEmpty()) await func();   
        }

        public static async Task xIfEmptyAsync<T>(this T obj, Func<Task> func)
        {
            if (obj.xIsNotEmpty()) await func();
        }
            

        public static bool xIsNull<T>(this T obj)
        {
            return obj is null;
        }

        public static bool xIsNotNull<T>(this T obj)
        {
            return obj is not null;
        }

        public static bool xIsEmpty<T>(this T obj)
        {
            if (obj.xIsNull())
            {
                return true;
            }

            switch (obj)
            {
                case string s when s.xIsEmpty():
                case ICollection {Count: <= 0}:
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool xIsNotEmpty<T>(this T obj)
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

        public static bool IsNullableType<T>(T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static T xDbExecute<T>(this IDbConnection connection, Func<IDbConnection, T> dbConnection)
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

        public static async Task<T> xDbExecuteAsync<T>(this IDbConnection connection,
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