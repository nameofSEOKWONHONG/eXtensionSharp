using System;
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

            if (obj is string)
            {
                return string.IsNullOrWhiteSpace(obj.ToString());
            }

            if (obj.xIsNumber())
            {
                if (obj.GetType() == typeof(byte)) return Comparer<byte>.Default.Compare(Convert.ToByte(obj), default(byte)) <= 0;
                else if (obj.GetType() == typeof(sbyte)) return Comparer<sbyte>.Default.Compare(Convert.ToSByte(obj), default(sbyte)) <= 0;
                else if (obj.GetType() == typeof(short)) return Comparer<short>.Default.Compare(Convert.ToInt16(obj), default(short)) <= 0;
                else if (obj.GetType() == typeof(ushort)) return Comparer<ushort>.Default.Compare(Convert.ToUInt16(obj), default(ushort)) <= 0;
                else if (obj.GetType() == typeof(int)) return Comparer<int>.Default.Compare(Convert.ToInt32(obj), default(int)) <= 0;
                else if (obj.GetType() == typeof(uint)) return Comparer<uint>.Default.Compare(Convert.ToUInt32(obj), default(uint)) <= 0;
                else if (obj.GetType() == typeof(long)) return Comparer<long>.Default.Compare(Convert.ToInt64(obj), default(long)) <= 0;
                else if (obj.GetType() == typeof(ulong)) return Comparer<ulong>.Default.Compare(Convert.ToUInt64(obj), default(ulong)) <= 0;
                else if (obj.GetType() == typeof(float)) return Comparer<float>.Default.Compare(Convert.ToSingle(obj), default(float)) <= 0;
                else if (obj.GetType() == typeof(double)) return Comparer<double>.Default.Compare(Convert.ToDouble(obj), default(double)) <= 0;
                else if (obj.GetType() == typeof(decimal)) return Comparer<decimal>.Default.Compare(Convert.ToDecimal(obj), default(decimal)) <= 0;

                return Comparer<T>.Default.Compare(obj, default(T)) <= 0;
            }

            if (obj.xIsDateTime())
            {
                var t = obj.xAs<DateTime>();
                if (t <= DateTime.MinValue) return true;
            }

            // collection type
            switch (obj)
            {
                case string s when string.IsNullOrWhiteSpace(s):
                // ReSharper disable once HeapView.PossibleBoxingAllocation
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                // ReSharper disable once HeapView.PossibleBoxingAllocation
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return true;

                default: return false;
            }
        }

        public static bool xIsNumber<T>(this T obj)
        {
            var type = obj.GetType();
            if (type.IsPrimitive)
            {
                return (type == typeof(int) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(decimal) ||
                type == typeof(long) ||
                type == typeof(short) ||
                type == typeof(byte) ||
                type == typeof(uint) ||
                type == typeof(ulong) ||
                type == typeof(ushort) ||
                type == typeof(sbyte));
            }

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

            HashSet<T> set = new();

            foreach (var item in items)
            {
                if (!set.Add(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool xTryDuplicate<T>(this IEnumerable<T> items, out T key)
        {
            key = default;

            if (items.xIsEmpty()) return false;

            HashSet<T> set = new();

            foreach (var item in items)
            {
                if (!set.Add(item))
                {
                    key = item;
                    return true;
                }
            }

            return false;
        }

        public static bool xTryDateParse(this string date, out DateTime dateTime)
        {
            dateTime = DateTime.MinValue;

            return DateTime.TryParse(date, out dateTime);
        }
    }
}