using System.Numerics;
using System.Text.RegularExpressions;

namespace eXtensionSharp;

public static class XRegularExpressionExtensions
{
    public static bool xIsNumber(this string str)
    {
        if (str.xIsEmpty()) return false;
        return double.TryParse(str, out double num);
    }

    //ref : https://medium.com/@mohsen_rajabi/how-to-write-a-regex-very-fast-in-c-best-practice-875d386c0485
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

    private static Regex _rrnKor =
        new Regex(@"^[0-9]{6}(-)?[0-9]{7}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static bool xIsKoreanRRN(this string str)
    {
        if (str.xIsEmpty()) return false;
        return _rrnKor.IsMatch(str);
    }
}