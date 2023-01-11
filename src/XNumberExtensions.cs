using System;
using System.Collections.Generic;
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

        //ref : https://medium.com/@mohsen_rajabi/how-to-write-a-regex-very-fast-in-c-best-practice-875d386c0485
        private static Regex _numberRegex =
            new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool xIsNumber(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _numberRegex.IsMatch(str);
        }

        private static Regex _alphabetRegex =
            new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool xIsAlphabet(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _alphabetRegex.IsMatch(str);
        }

        private static Regex _alphabetAndNumberRegex =
            new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool xIsAlphabetAndNumber(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _alphabetAndNumberRegex.IsMatch(str);
        }

        private static Regex _numericRegex = new Regex(@"^(?<digit>-?\d+)(\.(?<scale>\d*))?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool xIsNumeric(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _numericRegex.IsMatch(str);
        }

        private static Regex _hscodeRegex = new Regex("^[0-9]{4}[.][0-9]{2}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// HSCODE (국제통일상품분류체계 - 6204.62-1000) 패턴화
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool xIsHSCODE(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _hscodeRegex.IsMatch((str));
        }

        private static Regex _nationCellPhoneRegex =
            new Regex("^[0-9]{2,3}[)][0-9]{3}-[0-9]{3,4}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsNationCellPhone(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _nationCellPhoneRegex.IsMatch(str);
        }

        private static Regex _cellPhoneRegex =
            new Regex("^[0-9]{3}-[0-9]{3,4}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsCellPhone(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _cellPhoneRegex.IsMatch(str);
        }

        private static Regex _telPhoneRegex =
            new Regex("^[0-9]{2,3}-[0-9]{3,4}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsTelPhone(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _telPhoneRegex.IsMatch(str);
        }

        private static Regex _emailRegex =
            new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsEmail(this string str)
        {
            if (str.xIsEmpty()) return false;
            return _emailRegex.IsMatch(str);
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