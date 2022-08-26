using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading.Tasks;

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

        public static bool IsNullableType<T>(T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) != null;
        }
        #endregion

        #region [xIf Series]

        public static void xIfTrue(this bool state, Action execute, Action elseExecute = null)
        {
            if (state) execute();
            else
            {
                if (elseExecute.xIsNotEmpty()) elseExecute();    
            }
        }

        public static void xIfFalse(this bool state, Action execute, Action elseExecute = null)
        {
            if (!state) execute();
            else
            {
                if(elseExecute.xIsNotEmpty()) elseExecute();
            }
        }
        
        public static void xIfEmpty(this object obj, Action action, Action elseAction = null)
        {
            if (obj.xIsEmpty()) action();
            else
            {
                if (elseAction.xIsNotEmpty()) elseAction(); 
            }
        }
        
        public static T xIfEmpty<T>(this T obj, Func<T> execute, Func<T> elseExecute = null)
        {
            if (obj.xIsEmpty()) return execute();
            else
            {
                if (elseExecute.xIsNotEmpty()) 
                    return elseExecute();
            }

            return default;
        }
        
        public static void xIfNotEmpty(this object obj, Action action, Action elseAction = null)
        {
            if (obj.xIsNotEmpty()) action();
            else {
                if(elseAction.xIsNotEmpty())
                {
                    elseAction();
                }
            }
        }
        
        public static T xIfNotEmpty<T>(this T obj, Func<T> execute, Func<T> elseExecute = null)
        {
            if (obj.xIsNotEmpty()) return execute();
            else {
                if(elseExecute.xIsNotEmpty())
                {
                    return elseExecute();
                }
            }

            return default;
        }
        
        public static T2 xIfNotEmpty<T1, T2>(this T1 obj, Func<T2> execute, Func<T2> elseExecute = null)
        {
            if (obj.xIsNotEmpty()) return execute();
            else
            {
                if (elseExecute.xIsNotEmpty()) return elseExecute();
            };

            return default;
        }
        
        public static async Task xIfNotEmptyAsync<T>(this T obj, Func<Task> func)
        {
            if (obj.xIsNotEmpty()) await func();   
        }

        public static async Task xIfEmptyAsync<T>(this T obj, Func<Task> func)
        {
            if (obj.xIsNotEmpty()) await func();
        }

        #endregion


        
    }
}