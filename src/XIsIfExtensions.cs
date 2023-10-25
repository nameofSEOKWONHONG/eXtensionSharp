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
            
            if (obj.xIsNumber())
            {
                return EqualityComparer<T>.Default.Equals(obj, default(T));
            }

            if (obj.xIsDateTime())
            {
                var t = obj.xAs<DateTime>();
                if (t <= DateTime.MinValue) return true;
            }
            
            // collection type
            switch (obj) {
                case string s when string.IsNullOrWhiteSpace(s):
                // ReSharper disable once HeapView.PossibleBoxingAllocation
                case ICollection {Count: 0}:
                case Array {Length: 0}:
                // ReSharper disable once HeapView.PossibleBoxingAllocation
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return true;
                default: return false;
            }
        }

        public static bool xIsNumber<T>(this T obj)
        {
            var type = typeof(T);
            if (type == typeof(int) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(decimal) ||
                type == typeof(long) ||
                type == typeof(short) ||
                type == typeof(byte) ||
                type == typeof(uint) ||
                type == typeof(ulong) ||
                type == typeof(ushort) ||
                type == typeof(sbyte))
                return true;

            return false;
        }

        public static bool xIsDateTime<T>(this T obj)
        {
            var type = typeof(T);
            if (type == typeof(DateTime)) return true;

            return false;
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

        public static bool xIsSame<T>(this T src, T compare)
        {
            if (src.xIsEmpty()) return false;
            if (compare.xIsEmpty()) return false;
            return src.Equals(compare);
        }
        
        public static bool xIsNotSame<T>(this T src, T compare)
        {
            return !src.Equals(compare);
        }        
        
        public static bool xIsSameDate(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year.Equals(to!.Value.Year) &&
                   from.Value.Month.Equals(to.Value.Month) &&
                   from.Value.Day.Equals(to.Value.Day);
        }
        
        public static bool xIsSameFullDate(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year.Equals(to!.Value.Year) &&
                   from.Value.Month.Equals(to.Value.Month) &&
                   from.Value.Day.Equals(to.Value.Day) &&
                   from.Value.Hour.Equals(to.Value.Hour) &&
                   from.Value.Minute.Equals(to.Value.Minute) &&
                   from.Value.Second.Equals(to.Value.Second);
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

        public static bool xIsDuplicate<T>(this IEnumerable<T> items)
        {
            if (items.xIsEmpty()) return false;
            
            var v = items.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            
            return v.xIsNotEmpty();
        }

        public static bool xTryDuplicate<T>(this IEnumerable<T> items, out T key)
        {
            key = default;
            
            if (items.xIsEmpty()) return false;
            
            var v = items.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            if (v.xIsNotEmpty())
            {
                key = v.First();
                return true;
            }

            return false;
        }
    }
}