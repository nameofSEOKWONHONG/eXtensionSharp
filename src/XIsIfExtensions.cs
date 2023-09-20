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
            {
                return EqualityComparer<T>.Default.Equals(obj, default(T));
            }

            if (obj.xIsNull()) return true;
            switch (obj) {
                case DateTime dt when dt <= DateTime.MinValue:
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
        
        public static bool xIsEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year.Equals(to!.Value.Year) && 
                   from.Value.Month.Equals(to.Value.Month) && 
                   from.Value.Day.Equals(to.Value.Day);
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