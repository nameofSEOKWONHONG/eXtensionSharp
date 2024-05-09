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

            if (obj.xIsNumber())
            {
                if (obj is byte) return Comparer<byte>.Default.Compare(Convert.ToByte(obj), default) <= 0;
                else if (obj is sbyte) return Comparer<sbyte>.Default.Compare(Convert.ToSByte(obj), default) <= 0;
                else if (obj is short) return Comparer<short>.Default.Compare(Convert.ToInt16(obj), default) <= 0;
                else if (obj is ushort) return Comparer<ushort>.Default.Compare(Convert.ToUInt16(obj), default) <= 0;
                else if (obj is int) return Comparer<int>.Default.Compare(Convert.ToInt32(obj), default) <= 0;
                else if (obj is uint) return Comparer<uint>.Default.Compare(Convert.ToUInt32(obj), default) <= 0;
                else if (obj is long) return Comparer<long>.Default.Compare(Convert.ToInt64(obj), default) <= 0;
                else if (obj is ulong) return Comparer<ulong>.Default.Compare(Convert.ToUInt64(obj), default) <= 0;
                else if (obj is float) return Comparer<float>.Default.Compare(Convert.ToSingle(obj), default) <= 0;
                else if (obj is double) return Comparer<double>.Default.Compare(Convert.ToDouble(obj), default) <= 0;
                else if (obj is decimal) return Comparer<decimal>.Default.Compare(Convert.ToDecimal(obj), default) <= 0;

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
            return type == typeof(DateTime);
        }

        public static bool xIsNotEmpty<T>(this T obj)
        {
            return !obj.xIsEmpty();
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
    }
}