using System;

namespace eXtensionSharp
{
    public static class XValueExtensions
    {
        public static string xValue(this string src, string @default = null)
        {
            src.xIfEmpty(() =>
            {
                if (@default is { } val)
                    src = val;
            });
            return src.xTrim();
        }

        public static string xValue(this DateTime date, ENUM_DATE_FORMAT dateFormat)
        {
            if (date.xIsEmpty()) return DateTime.MinValue.xToDate(dateFormat);
            return date.xToDate(dateFormat);
        }
        
        public static T xValue<T>(this object src, object @default = null)
        {
            if (src.xIsNull() && @default.xIsNull())
            {
                return default;
            }
            else if (src.xIsNotNull())
            {
                if (src is Guid guid) 
                    return (T) Convert.ChangeType(guid.ToString(), typeof(T));
                else if (src is DateTime time)
                    return (T) Convert.ChangeType(time.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD_HH_MM_SS), typeof(T));
                
                return (T) Convert.ChangeType(src, typeof(T));
            }
                
            if (@default.xIsNotNull()) 
                return (T) Convert.ChangeType(@default, typeof(T))!;

            return default;
        }

        public static T xValue<T>(this T src, T @default = null) where T : class, new()
        {
            if (src.xIsNull() && @default.xIsNotNull())
                return @default;
            if (src.xIsNull() && @default.xIsNull()) return new T();

            return src;
        }

        public static string xValue<T>(this XEnumBase<T> src, XEnumBase<T> defaultValue = null)
            where T : XEnumBase<T>, new()
        {
            if (defaultValue.xIsNotNull())
            {
                src.ToString().xIfEmpty(() =>
                {
                    if (defaultValue is not null)
                    {
                        src = defaultValue;
                    }
                });
                return src.ToString();
            }
            return src.ToString();
        }

        public static XEnumBase<T> xValue<T>(this string src, XEnumBase<T> defaultValue = null)
            where T : XEnumBase<T>, new()
        {
            if (src.xIsEmpty()) return XEnumBase<T>.Parse(src);

            if (defaultValue.xIsNotNull()) return defaultValue;

            return null;
        }
    }
}