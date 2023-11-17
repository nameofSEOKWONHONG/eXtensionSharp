using System.Text;
using System.Xml.Serialization;

namespace eXtensionSharp
{
    public static class XValueExtensions
    {
        public static string xValue(this string src, string @default = null) =>
            src.xIsEmpty() ? (@default.xIsEmpty() ? string.Empty : @default) : src;

        public static string xValue(this DateTime date, ENUM_DATE_FORMAT dateFormat) =>
            date.xIsEmpty() ? DateTime.MinValue.xToDate(dateFormat) : date.xToDate(dateFormat);

        public static T xValue<T>(this object src, object @default = null)
        {
            if (src.xIsEmpty())
            {
                if (@default.xIsEmpty()) return default;
                return (T)Convert.ChangeType(@default, typeof(T))!;
            }

            if (typeof(T).IsEnum)
            {
                return (T)src;
            }

            //if (typeof(T).xIsNumber())
            //{
            //    if (typeof(T).GetType() == typeof(byte)) return (T)(object)Convert.ToByte(src);
            //    else if (typeof(T).GetType() == typeof(sbyte)) return (T)(object)Convert.ToSByte(src);
            //    else if (typeof(T).GetType() == typeof(short)) return (T)(object)Convert.ToInt16(src);
            //    else if (typeof(T).GetType() == typeof(ushort)) return (T)(object)Convert.ToUInt16(src);
            //    else if (typeof(T).GetType() == typeof(int)) return (T)(object)Convert.ToInt32(src);
            //    else if (typeof(T).GetType() == typeof(uint)) return (T)(object)Convert.ToUInt32(src);
            //    else if (typeof(T).GetType() == typeof(long)) return (T)(object)Convert.ToInt64(src);
            //    else if (typeof(T).GetType() == typeof(ulong)) return (T)(object)Convert.ToUInt64(src);
            //    else if (typeof(T).GetType() == typeof(float)) return (T)(object)Convert.ToSingle(src);
            //    else if (typeof(T).GetType() == typeof(double)) return (T)(object)Convert.ToDouble(src);
            //    else if (typeof(T).GetType() == typeof(decimal)) return (T)(object)Convert.ToDecimal(src);
            //}

            if (src is Guid guid) return (T)Convert.ChangeType(guid.ToString(), typeof(T));

            if (src is DateTime time)
            {
                if (typeof(T) == typeof(int))
                    return (T)Convert.ChangeType(time.xToDate(ENUM_DATE_FORMAT.YYYYMMDD), typeof(T));

                return (T)Convert.ChangeType(time.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD), typeof(T));
            }

            return (T)Convert.ChangeType(src, typeof(T));
        }

        public static T xAs<T>(this object src)
        {
            return (T)src;
        }

        // public static string xValue<T>(this XEnumBase<T> src, XEnumBase<T> defaultValue = null)
        //     where T : XEnumBase<T>, new()
        // {
        //     var v = src.ToString();
        //     if (v.xIsEmpty())
        //     {
        //         if (defaultValue.xIsEmpty()) return default;
        //         else return defaultValue;
        //     }
        //
        //     return v;
        // }
        //
        // public static XEnumBase<T> xValue<T>(this string src, XEnumBase<T> defaultValue = null)
        //     where T : XEnumBase<T>, new()
        // {
        //     if (src.xIsNotEmpty()) return XEnumBase<T>.Parse(src);
        //     if (defaultValue.xIsNotEmpty()) return defaultValue;
        //     return default;
        // }
    }
}