using System.Globalization;

namespace eXtensionSharp;

/// <summary>
/// 변환 동작을 제어하는 옵션.
/// </summary>
public sealed class ConvertOptions
{
    /// <summary>숫자/날짜 파싱에 사용할 문화권.</summary>
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;

    /// <summary>숫자 파싱 스타일(부호/천단위/부동소수점 허용 등).</summary>
    public NumberStyles NumberStyles { get; init; }
        = NumberStyles.Float | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign;

    /// <summary>열거형 이름 매칭 시 대소문자 무시 여부.</summary>
    public bool EnumIgnoreCase { get; init; } = true;

    /// <summary>DateTime/DateTimeOffset TryParseExact 에 사용할 포맷들.</summary>
    public string[] DateTimeFormats { get; init; }
        = new[] { "yyyy-MM-dd", "yyyyMMdd", "yyyy-MM-dd HH:mm:ss", "O", "s" };

    /// <summary>TimeSpan TryParseExact 에 사용할 포맷들.</summary>
    public string[] TimeSpanFormats { get; init; }
        = new[] { "c", @"hh\:mm\:ss", @"hh\:mm", @"d\.hh\:mm\:ss" };

    /// <summary>빈 문자열(공백 포함)을 null/기본값으로 간주할지.</summary>
    public bool EmptyStringIsNull { get; init; } = true;
}