namespace eXtensionSharp;

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

/// <summary>
/// 예외 없는 빠른 타입 변환 유틸리티.
/// </summary>
public static class FastConvert
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> TcCache = new();

    // -------------------------
    // Public API
    // -------------------------

    /// <summary>
    /// 실패 시 기본값을 반환하는 제네릭 변환.
    /// </summary>
    public static T ChangeType<T>(object src, T @default = default, ConvertOptions opt = null)
    {
        if (src.xIsEmpty()) return @default;
        return TryChangeType(src, out T v, opt) ? v : @default;
    }

    /// <summary>
    /// 제네릭 Try-Convert.
    /// </summary>
    public static bool TryChangeType<T>(object src, out T value, ConvertOptions opt = null)
    {
        opt ??= new ConvertOptions();
        if (TryChangeType(src, typeof(T), out var boxed, opt))
        { value = (T)boxed!; return true; }

        value = default!;
        return false;
    }

    /// <summary>
    /// 실패 시 지정한 기본값을 반환하는 비제네릭 변환.
    /// </summary>
    public static object ChangeType(object src, Type targetType, object @default = null, ConvertOptions opt = null)
        => TryChangeType(src, targetType, out var v, opt ?? new ConvertOptions()) ? v : @default;

    // -------------------------
    // Core
    // -------------------------
    private static bool TryChangeType(object src, Type targetType, out object result, ConvertOptions opt)
    {
        // 1) null/DBNULL → 대상 타입 기본값
        if (src is null || src is DBNull)
        { result = GetDefault(targetType); return true; }

        // 2) Nullable 언랩
        var underlying = Nullable.GetUnderlyingType(targetType);
        var effType = underlying ?? targetType;

        // 3) 동형식 빠른 경로
        if (effType.IsInstanceOfType(src)) { result = src; return true; }

        // 4) 문자열 전처리(빈 문자열 → 기본값)
        if (src is string s && opt.EmptyStringIsNull && string.IsNullOrWhiteSpace(s))
        { result = GetDefault(targetType); return true; }

        // 5) JsonElement 최적화
        if (src is JsonElement je && TryFromJsonElement(je, effType, opt, out result)) return true;

        // 6) Enum
        if (effType.IsEnum && TryToEnum(src, effType, opt.EnumIgnoreCase, out result)) return true;

        // 7) 특수 타입
        if (effType == typeof(Guid)           && TryToGuid(src, out var g))    { result = g;   return true; }
        if (effType == typeof(DateTime)       && TryToDateTime(src, opt, out var dt)) { result = dt;  return true; }
        if (effType == typeof(DateTimeOffset) && TryToDateTimeOffset(src, opt, out var dto)) { result = dto; return true; }
        if (effType == typeof(TimeSpan)       && TryToTimeSpan(src, opt, out var ts)) { result = ts;  return true; }
        if (effType == typeof(bool)           && TryToBoolean(src, out var b)) { result = b;   return true; }

        // 8) 숫자
        if (TryToNumber(src, effType, opt, out result)) return true;

        // 9) 마지막 폴백: TypeConverter
        try
        {
            var tc = TcCache.GetOrAdd(effType, TypeDescriptor.GetConverter);
            if (tc is not null)
            {
                var srcType = src.GetType();
                if (tc.CanConvertFrom(srcType))
                { result = tc.ConvertFrom(null, opt.Culture, src); return true; }

                if (src is string str && tc.CanConvertFrom(typeof(string)))
                { result = tc.ConvertFrom(null, opt.Culture, str); return true; }
            }
        }
        catch { /* ignore */ }

        result = null;
        return false;
    }

    // -------------------------
    // Helpers
    // -------------------------

    #region JsonElement

    private static bool TryFromJsonElement(JsonElement je, Type t, ConvertOptions opt, out object result)
    {
        try
        {
            switch (je.ValueKind)
            {
                case JsonValueKind.String:
                    var ss = je.GetString();
                    if (t == typeof(string)) { result = ss; return true; }
                    if (t == typeof(Guid) && Guid.TryParse(ss, out var g)) { result = g; return true; }
                    if (t == typeof(DateTime) &&
                        DateTime.TryParse(ss, opt.Culture, DateTimeStyles.RoundtripKind, out var dt))
                    { result = dt; return true; }
                    if (t == typeof(DateTimeOffset) &&
                        DateTimeOffset.TryParse(ss, opt.Culture, DateTimeStyles.RoundtripKind, out var dto))
                    { result = dto; return true; }
                    break;

                case JsonValueKind.Number:
                    if (t == typeof(int)     && je.TryGetInt32(out var i32)) { result = i32; return true; }
                    if (t == typeof(long)    && je.TryGetInt64(out var i64)) { result = i64; return true; }
                    if (t == typeof(double)  && je.TryGetDouble(out var d))  { result = d;  return true; }
                    if (t == typeof(decimal) &&
                        decimal.TryParse(je.GetRawText(), NumberStyles.Any, opt.Culture, out var dec))
                    { result = dec; return true; }
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    if (t == typeof(bool)) { result = je.GetBoolean(); return true; }
                    break;
            }

            // 복합 타입 폴백
            result = JsonSerializer.Deserialize(je.GetRawText(), t);
            return result is not null;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    #endregion

    #region Enum / Guid / DateTime(Offset) / TimeSpan / Boolean

    private static bool TryToEnum(object src, Type enumType, bool ignoreCase, out object result)
    {
        if (src is string es)
        {
            if (Enum.TryParse(enumType, es, ignoreCase, out var ev))
            { result = ev; return true; }

            if (long.TryParse(es, NumberStyles.Integer, CultureInfo.InvariantCulture, out var num))
            { result = Enum.ToObject(enumType, num); return true; }
        }
        else if (IsNumeric(src.GetType()))
        {
            var num = System.Convert.ToInt64(src, CultureInfo.InvariantCulture);
            result = Enum.ToObject(enumType, num);
            return true;
        }

        result = null;
        return false;
    }

    private static bool TryToGuid(object src, out Guid g)
    {
        if (src is Guid gg) { g = gg; return true; }
        if (src is string s && Guid.TryParse(s.Trim(), out g)) return true;
        g = default;
        return false;
    }

    private static bool TryToDateTime(object src, ConvertOptions opt, out DateTime dt)
    {
        if (src is DateTime d) { dt = d; return true; }
        if (src is string s)
        {
            s = s.Trim();
            if (DateTime.TryParseExact(s, opt.DateTimeFormats, opt.Culture,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal, out dt)) return true;

            if (DateTime.TryParse(s, opt.Culture, DateTimeStyles.RoundtripKind, out dt)) return true;
        }
        dt = default;
        return false;
    }

    private static bool TryToDateTimeOffset(object src, ConvertOptions opt, out DateTimeOffset dto)
    {
        if (src is DateTimeOffset d) { dto = d; return true; }
        if (src is string s)
        {
            s = s.Trim();
            if (DateTimeOffset.TryParseExact(s, opt.DateTimeFormats, opt.Culture,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind, out dto)) return true;

            if (DateTimeOffset.TryParse(s, opt.Culture, DateTimeStyles.RoundtripKind, out dto)) return true;
        }
        dto = default;
        return false;
    }

    private static bool TryToTimeSpan(object src, ConvertOptions opt, out TimeSpan ts)
    {
        if (src is TimeSpan t) { ts = t; return true; }
        if (src is string s)
        {
            s = s.Trim();
            foreach (var f in opt.TimeSpanFormats)
                if (TimeSpan.TryParseExact(s, f, opt.Culture, out ts)) return true;

            if (TimeSpan.TryParse(s, opt.Culture, out ts)) return true;
        }
        ts = default;
        return false;
    }

    private static bool TryToBoolean(object src, out bool b)
    {
        if (src is bool bb) { b = bb; return true; }

        if (src is string s)
        {
            s = s.Trim();

            if (bool.TryParse(s, out b)) return true;

            // 길이 기반의 빠른 매핑
            switch (s.Length)
            {
                case 1:
                    char c = s[0];
                    if (c is '1' or 'y' or 'Y' or 't' or 'T') { b = true; return true; }
                    if (c is '0' or 'n' or 'N' or 'f' or 'F') { b = false; return true; }
                    break;

                case 2:
                    if (s.Equals("on", StringComparison.OrdinalIgnoreCase)) { b = true; return true; }
                    if (s.Equals("no", StringComparison.OrdinalIgnoreCase)) { b = false; return true; }
                    break;

                case 3:
                    if (s.Equals("yes", StringComparison.OrdinalIgnoreCase)) { b = true; return true; }
                    break;

                case 3 or 4: // "off" 처리 포함
                    if (s.Equals("off", StringComparison.OrdinalIgnoreCase)) { b = false; return true; }
                    if (s.Equals("true", StringComparison.OrdinalIgnoreCase)) { b = true; return true; }
                    break;

                case 5:
                    if (s.Equals("false", StringComparison.OrdinalIgnoreCase)) { b = false; return true; }
                    break;
            }
        }
        else if (IsNumeric(src.GetType()))
        {
            b = System.Convert.ToDouble(src, CultureInfo.InvariantCulture) != 0d;
            return true;
        }

        b = default;
        return false;
    }

    #endregion

    #region Numbers

    private static bool TryToNumber(object src, Type t, ConvertOptions opt, out object result)
    {
        result = null;

        // 문자열 → 각 타입의 TryParse (문화권/스타일 반영)
        if (src is string s)
        {
            s = s.Trim();
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:    if (byte.TryParse(s,   opt.NumberStyles, opt.Culture, out var b))   { result = b;   return true; }
                    break;
                case TypeCode.SByte:   if (sbyte.TryParse(s,  opt.NumberStyles, opt.Culture, out var sb))  { result = sb;  return true; }
                    break;
                case TypeCode.Int16:   if (short.TryParse(s,  opt.NumberStyles, opt.Culture, out var i16)) { result = i16; return true; }
                    break;
                case TypeCode.UInt16:  if (ushort.TryParse(s, opt.NumberStyles, opt.Culture, out var ui16)){ result = ui16;return true; }
                    break;
                case TypeCode.Int32:   if (int.TryParse(s,    opt.NumberStyles, opt.Culture, out var i32)) { result = i32; return true; }
                    break;
                case TypeCode.UInt32:  if (uint.TryParse(s,   opt.NumberStyles, opt.Culture, out var ui32)){ result = ui32;return true; }
                    break;
                case TypeCode.Int64:   if (long.TryParse(s,   opt.NumberStyles, opt.Culture, out var i64)) { result = i64; return true; }
                    break;
                case TypeCode.UInt64:  if (ulong.TryParse(s,  opt.NumberStyles, opt.Culture, out var ui64)){ result = ui64;return true; }
                    break;
                case TypeCode.Single:  if (float.TryParse(s,  opt.NumberStyles, opt.Culture, out var f))   { result = f;   return true; }
                    break;
                case TypeCode.Double:  if (double.TryParse(s, opt.NumberStyles, opt.Culture, out var d))   { result = d;   return true; }
                    break;
                case TypeCode.Decimal: if (decimal.TryParse(s,opt.NumberStyles, opt.Culture, out var m))   { result = m;   return true; }
                    break;
                default:
                    return false;
            }
            return false;
        }

        // 비문자열 → IConvertible 경로
        try
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:    result = System.Convert.ToByte(src,   opt.Culture); return true;
                case TypeCode.SByte:   result = System.Convert.ToSByte(src,  opt.Culture); return true;
                case TypeCode.Int16:   result = System.Convert.ToInt16(src,  opt.Culture); return true;
                case TypeCode.UInt16:  result = System.Convert.ToUInt16(src, opt.Culture); return true;
                case TypeCode.Int32:   result = System.Convert.ToInt32(src,  opt.Culture); return true;
                case TypeCode.UInt32:  result = System.Convert.ToUInt32(src, opt.Culture); return true;
                case TypeCode.Int64:   result = System.Convert.ToInt64(src,  opt.Culture); return true;
                case TypeCode.UInt64:  result = System.Convert.ToUInt64(src, opt.Culture); return true;
                case TypeCode.Single:  result = System.Convert.ToSingle(src, opt.Culture); return true;
                case TypeCode.Double:  result = System.Convert.ToDouble(src, opt.Culture); return true;
                case TypeCode.Decimal: result = System.Convert.ToDecimal(src,opt.Culture); return true;
                default:
                    return false;
            }
        }
        catch
        {
            result = null;
            return false;
        }
    }

    #endregion

    #region Utils

    private static bool IsNumeric(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        switch (Type.GetTypeCode(t))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
                return true;
            default:
                return false;
        }
    }

    private static bool IsNullable(Type t) => !t.IsValueType || Nullable.GetUnderlyingType(t) is not null;

    private static object GetDefault(Type t) => IsNullable(t) ? null : Activator.CreateInstance(t);

    #endregion
}