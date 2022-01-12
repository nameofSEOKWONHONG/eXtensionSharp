using System;

namespace eXtensionSharp
{
    public static class XValue
    {
        public static string xValue(this string src, string @default = null)
        {
            src.xIfEmpty(() => src = @default);
            return src.xTrim();
        }

        public static string xValue(this object src, object @default = null)
        {
            if (src.xIsNull() && @default.xIsNull()) return string.Empty;
            else if (src.xIsNotNull()) return Convert.ToString(src).xTrim();

            if (@default.xIsNotNull()) return Convert.ToString(@default).xTrim();

            return string.Empty;
        }

        public static T xValue<T>(this object src, object @default = null)
        {
            if (src.xIsNull() && @default.xIsNull()) return default;
            else if (src.xIsNotNull()) return (T) Convert.ChangeType(src, typeof(T));

            if (@default.xIsNotNull()) return (T) Convert.ChangeType(@default, typeof(T));

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
                src.ToString().xIfEmpty(() => src = defaultValue);
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