using System;
using System.Text.RegularExpressions;

namespace eXtensionSharp
{
    public static class XNumberExtensions
    {
        public static string xToNumber<T>(this T val, ENUM_NUMBER_FORMAT_TYPE type, ENUM_GET_ALLOW_TYPE allow)
        {
            if (val.GetType() == typeof(DateTime)) throw new NotSupportedException("DateTime is not support.");
            if (val.GetType() == typeof(float)) throw new NotSupportedException("float is not support.");

            var result = type switch
            {
                ENUM_NUMBER_FORMAT_TYPE.Comma => string.Format("{0:#,###}", val),
                ENUM_NUMBER_FORMAT_TYPE.Rate => string.Format("{0:##.##}", val),
                ENUM_NUMBER_FORMAT_TYPE.Mobile => allow switch
                {
                    ENUM_GET_ALLOW_TYPE.Allow => string.Format("{0}-{1}-{2}", val.ToString().xGetFirst(3),
                        val.ToString().xGetMiddle(3, 4),
                        val.ToString().xGetLast(4)),
                    _ => string.Format("{0}-{1}-****", val.ToString().xGetFirst(3),
                        val.ToString().xGetMiddle(3, 4))
                },
                ENUM_NUMBER_FORMAT_TYPE.Phone => MakePhoneString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.RRN => MakeRRNString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.CofficePrice => string.Format("{0}.{1}", val.ToString().xGetFirst(1),
                    val.ToString().xGetMiddle(1, 1)),
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
        private static string MakePhoneString<T>(T val, ENUM_GET_ALLOW_TYPE allow)
        {
            if (allow == ENUM_GET_ALLOW_TYPE.Allow)
            {
                var temp = val.ToString();
                if (temp.xGetFirst(2) == "02")
                {
                    if (temp.Length == 10)
                        return string.Format("{0}-{1}-{2}", temp.xGetFirst(2),
                            temp.xGetMiddle(2, 4),
                            temp.xGetLast(4));
                    return string.Format("{0}-{1}-{2}", temp.xGetFirst(2),
                        temp.xGetMiddle(2, 3),
                        temp.xGetLast(4));
                }

                if (temp.Length == 11)
                    return string.Format("{0}-{1}-{2}", temp.xGetFirst(3),
                        temp.xGetMiddle(3, 4),
                        temp.xGetLast(4));
                return string.Format("{0}-{1}-{2}", temp.xGetFirst(3),
                    temp.xGetMiddle(3, 3),
                    temp.xGetLast(4));
            }
            else
            {
                var temp = val.ToString();
                if (temp.xGetFirst(2) == "02")
                {
                    if (temp.Length == 10)
                        return string.Format("{0}-{1}-****", temp.xGetFirst(2),
                            temp.xGetMiddle(2, 4));
                    return string.Format("{0}-{1}-****", temp.xGetFirst(2),
                        temp.xGetMiddle(2, 3));
                }

                if (temp.Length == 11)
                    return string.Format("{0}-{1}-****", temp.xGetFirst(3),
                        temp.xGetMiddle(3, 4));
                return string.Format("{0}-{1}-****", temp.xGetFirst(3),
                    temp.xGetMiddle(3, 3));
            }
        }

        private static string MakeRRNString<T>(T val, ENUM_GET_ALLOW_TYPE allow)
        {
            if (allow == ENUM_GET_ALLOW_TYPE.Allow)
                return string.Format("{0}-{1}", val.ToString().xGetFirst(6),
                    val.ToString().xGetLast(7));
            return string.Format("{0}-*******", val.ToString().xGetFirst(6));
        }

        public static string xGetMiddle(this string value, int fromLen, int getLen)
        {
            return value.Substring(fromLen, getLen);
        }

        public static string xGetFirst(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string xGetLast(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }

        //ref : https://medium.com/@mohsen_rajabi/how-to-write-a-regex-very-fast-in-c-best-practice-875d386c0485
        private static Regex _numberRegex =
            new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsNumber(this string str)
        {
            str.xIfEmpty(() => str = string.Empty);
            return _numberRegex.IsMatch(str);
        }

        private static Regex _alphabetRegex =
            new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsAlphabet(this string str)
        {
            str.xIfEmpty(() => str = string.Empty);
            return _alphabetRegex.IsMatch(str);
        }

        private static Regex _alphabetAndNumberRegex =
            new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsAlphabetAndNumber(this string str)
        {
            str.xIfEmpty(() => str = string.Empty);
            return _alphabetAndNumberRegex.IsMatch(str);
        }

        private static Regex _numericRegex = new Regex(@"^(?<digit>-?\d+)(\.(?<scale>\d*))?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static bool xIsNumeric(this string str)
        {
            str.xIfEmpty(() => str = string.Empty);
            return _numericRegex.IsMatch(str);
        }
    }

    public enum ENUM_GET_ALLOW_TYPE
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