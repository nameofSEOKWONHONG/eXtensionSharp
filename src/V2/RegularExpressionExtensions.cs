using System;
using System.Text.RegularExpressions;

namespace eXtensionSharp;

public static class RegularExpressionExtensions
{
    //ref : https://medium.com/@mohsen_rajabi/how-to-write-a-regex-very-fast-in-c-best-practice-875d386c0485
    private static Regex _alphabetRegex =
        new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _alphabetAndNumberRegex =
        new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static Regex _numericRegex = new Regex(@"^(?<digit>-?\d+)(\.(?<scale>\d*))?$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _hscodeRegex = new Regex("^[0-9]{4}[.][0-9]{2}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _nationCellPhoneRegex =
        new Regex("^[0-9]{2,3}[)][0-9]{3}-[0-9]{3,4}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _cellPhoneRegex =
        new Regex("^[0-9]{3}-[0-9]{3,4}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _telPhoneRegex =
        new Regex("^[0-9]{2,3}-[0-9]{3,4}-[0-9]{4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _emailRegex =
        new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static Regex _rrnKor =
        new Regex(@"^[0-9]{6}(-)?[0-9]{7}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    extension(string source)
    {
        public bool xIsNumber()
        {
            if (source.xIsEmpty()) return false;
            return double.TryParse(source, out double num);
        }

        public bool xIsAlphabet()
        {
            if (source.xIsEmpty()) return false;
            return _alphabetRegex.IsMatch(source);
        }

        public bool xIsAlphabetAndNumber()
        {
            if (source.xIsEmpty()) return false;
            return _alphabetAndNumberRegex.IsMatch(source);
        }

        public bool xIsNumeric()
        {
            if (source.xIsEmpty()) return false;
            return _numericRegex.IsMatch(source);
        }

        /// <summary>
        /// HSCODE (국제통일상품분류체계 - 6204.62-1000) 패턴화
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool xIsHSCODE()
        {
            if (source.xIsEmpty()) return false;
            return _hscodeRegex.IsMatch((source));
        }

        public bool xIsNationCellPhone()
        {
            if (source.xIsEmpty()) return false;
            return _nationCellPhoneRegex.IsMatch(source);
        }

        public bool xIsCellPhone()
        {
            if (source.xIsEmpty()) return false;
            return _cellPhoneRegex.IsMatch(source);
        }

        public bool xIsTelPhone()
        {
            if (source.xIsEmpty()) return false;
            return _telPhoneRegex.IsMatch(source);
        }

        public bool xIsEmail()
        {
            if (source.xIsEmpty()) return false;
            return _emailRegex.IsMatch(source);
        }


        public bool xIsKoreanRRN()
        {
            if (source.xIsEmpty()) return false;
            return _rrnKor.IsMatch(source);
        }
    }
}
