using System.Collections;
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

        public static bool xIsEmpty<T>(this T obj)
        {
            if (obj.xIsNull())
            {
                return true;
            }
            switch (obj) {
                case null: return true;
                case Int16 i16 when i16 <= 0:
                case Int32 i32 when i32 <= 0:
                case Int64 i64 when i64 <= 0:
                case double d when d <= 0:
                case float f when f <= 0:
                case decimal de when de <= 0:
                case DateTime dt when dt <= DateTime.MinValue:
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

        public static bool xIsEquals<T>(this IEnumerable<T> items, T compare)
            where T : class
        {
            if (items.xIsEmpty()) return false;
            if (compare.xIsEmpty()) return false;
            return compare.xIsEquals(items);
        }
        
        public static bool xIsEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year.Equals(to!.Value.Year) && 
                   from.Value.Month.Equals(to.Value.Month) && 
                   from.Value.Day.Equals(to.Value.Day);
        }
        
        public static bool xIsNotSame<T>(this T a, T b)
        {
            if (a.xIsEmpty()) return false;
            if (b.xIsEmpty()) return false;
            return !a.Equals(b);
        }

        public static bool xIsSame<T>(this T a, T b)
        {
            if (a.xIsEmpty()) return false;
            if (b.xIsEmpty()) return false;
            return a.Equals(b);
        }
        
        public static bool IsNullableType<T>(T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type).xIsNotEmpty();
        }

        public static bool xIf(this string item, string match)
        {
            if (item.xIsEquals(match)) return true;
            return false;
        }


        #endregion [xIs Series]
    }
}