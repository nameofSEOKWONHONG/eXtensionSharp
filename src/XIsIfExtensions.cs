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
        /// xIsEmpty dose not support number type.<br/>
        /// xIsEmpty dose not support DateTime type.
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

            switch (obj)
            {
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return true;

                default: return false;
            }
        }
        
        public static bool xIsNotEmpty<T>(this T obj)
        {
            return !obj.xIsEmpty();
        }        

        public static bool xIsDateTime<T>(this T obj)
        {
            return obj is DateTime;
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

        public static void xIf<T>(this T item, T compare, Action match, Action notMatch)
        {
            if (item.xIsSame(compare)) match();
            else notMatch();
        }

        #endregion [xIs Series]
    }
}