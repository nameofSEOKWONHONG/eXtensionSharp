using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace eXtensionSharp
{
    public static class XNumberExtensions
    {
        public static string xDisplayNumber<T>(this T val, ENUM_NUMBER_FORMAT_TYPE type, ENUM_VIEW_ALLOW_TYPE allow) where T : struct
        {
            if (val.GetType() == typeof(DateTime)) throw new NotSupportedException("DateTime is not support.");
            if (val.GetType() == typeof(float)) throw new NotSupportedException("float is not support.");

            var result = type switch
            {
                ENUM_NUMBER_FORMAT_TYPE.Comma => $"{val:#,#}",
                ENUM_NUMBER_FORMAT_TYPE.Rate => $"{val:##.##}",
                ENUM_NUMBER_FORMAT_TYPE.Mobile => allow switch
                {
                    ENUM_VIEW_ALLOW_TYPE.Allow => $"{val.ToString().xSubstringFirst(3)}-{val.ToString().xSubstringMiddle(3, 4)}-{val.ToString().xSubstringLast(4)}",
                    ENUM_VIEW_ALLOW_TYPE.NotAllow => $"{val.ToString().xSubstringFirst(3)}-{val.ToString().xSubstringMiddle(3, 4)}-****",
                    _ => throw new NotSupportedException()
                },
                ENUM_NUMBER_FORMAT_TYPE.Phone => MakePhoneString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.RRN => MakeRRNString(val, allow),
                _ => throw new NotSupportedException("do not convert value")
            };

            return result;
        }

        /// <summary>
        /// 추후에 https://github.com/twcclegg/libphonenumber-csharp 쪽으로 변경.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="allow"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string MakePhoneString<T>(T val, ENUM_VIEW_ALLOW_TYPE allow)
        {
            if (allow == ENUM_VIEW_ALLOW_TYPE.Allow)
            {
                var temp = val.ToString();
                if (temp.xSubstringFirst(2) == "02")
                {
                    if (temp.Length == 10)
                        return string.Format("{0}-{1}-{2}", temp.xSubstringFirst(2),
                            temp.xSubstringMiddle(2, 4),
                            temp.xSubstringLast(4));
                    return string.Format("{0}-{1}-{2}", temp.xSubstringFirst(2),
                        temp.xSubstringMiddle(2, 3),
                        temp.xSubstringLast(4));
                }

                if (temp.Length == 11)
                    return string.Format("{0}-{1}-{2}", temp.xSubstringFirst(3),
                        temp.xSubstringMiddle(3, 4),
                        temp.xSubstringLast(4));
                return string.Format("{0}-{1}-{2}", temp.xSubstringFirst(3),
                    temp.xSubstringMiddle(3, 3),
                    temp.xSubstringLast(4));
            }
            else
            {
                var temp = val.ToString();
                if (temp.xSubstringFirst(2) == "02")
                {
                    if (temp.Length == 10)
                        return string.Format("{0}-{1}-****", temp.xSubstringFirst(2),
                            temp.xSubstringMiddle(2, 4));
                    return string.Format("{0}-{1}-****", temp.xSubstringFirst(2),
                        temp.xSubstringMiddle(2, 3));
                }

                if (temp.Length == 11)
                    return string.Format("{0}-{1}-****", temp.xSubstringFirst(3),
                        temp.xSubstringMiddle(3, 4));
                return string.Format("{0}-{1}-****", temp.xSubstringFirst(3),
                    temp.xSubstringMiddle(3, 3));
            }
        }

        private static string MakeRRNString<T>(T val, ENUM_VIEW_ALLOW_TYPE allow)
        {
            if (allow == ENUM_VIEW_ALLOW_TYPE.Allow)
                return string.Format("{0}-{1}", val.ToString().xSubstringFirst(6),
                    val.ToString().xSubstringLast(7));
            return string.Format("{0}-*******", val.ToString().xSubstringFirst(6));
        }

        public static string xSubstringMiddle(this string value, int fromLen, int getLen)
        {
            return value.Substring(fromLen, getLen);
        }

        public static string xSubstringFirst(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string xSubstringLast(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }

        public static T ToSum<T>(this IEnumerable<T> args) where T : INumber<T>
        {
            T sum = T.Zero;
            foreach (T item in args)
            {
                sum = sum + item;
            }
        
            return sum;
        }
    }

    public enum ENUM_VIEW_ALLOW_TYPE
    {
        Allow,
        NotAllow
    }

    public enum ENUM_NUMBER_FORMAT_TYPE
    {
        Comma,
        Rate,
        Mobile,
        RRN,
        CofficePrice,
        Phone
    }
}