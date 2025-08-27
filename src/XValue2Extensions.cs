using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace eXtensionSharp;

public static class XValue2Extensions
{
    /// <summary>
    /// Safely converts an object to the specified type. 
    /// If the conversion fails, no exception is thrown and a default value is returned instead.
    /// </summary>
    /// <typeparam name="T">The target type to convert to.</typeparam>
    /// <param name="src">
    /// The source object to convert.
    /// </param>
    /// <param name="default">
    /// A fallback value to return if <paramref name="src"/> is null, empty, 
    /// or cannot be converted. This value will also be converted to <typeparamref name="T"/> if possible.
    /// </param>
    /// <param name="opt">
    /// A <see cref="ConvertOptions"/> instance that controls conversion behavior 
    /// (culture, number styles, date/time formats, handling of empty strings, etc.). 
    /// If omitted, default options are used.
    /// </param>
    /// <returns>
    /// The converted value of <paramref name="src"/> as <typeparamref name="T"/> if successful;  
    /// otherwise, the converted value of <paramref name="default"/> if possible;  
    /// if both conversions fail, <c>default(T)</c> is returned.
    /// </returns>
    /// <remarks>
    /// Internally uses <see cref="FlexibleConvert.ChangeType{T}(object, T, ConvertOptions)"/> 
    /// to support:
    /// <list type="bullet">
    ///   <item><description>Nullable types, enums, GUID, DateTime, DateTimeOffset, and TimeSpan</description></item>
    ///   <item><description>Numeric conversions with <see cref="ConvertOptions.NumberStyles"/> and <see cref="ConvertOptions.Culture"/></description></item>
    ///   <item><description>Boolean polymorphic input (e.g., "Y/N", "1/0", "on/off", "true/false")</description></item>
    ///   <item><description><see cref="System.Text.Json.JsonElement"/> and <see cref="TypeConverter"/>-based conversions</description></item>
    ///   <item><description>Optional handling of empty strings as null (<see cref="ConvertOptions.EmptyStringIsNull"/>)</description></item>
    /// </list>
    /// The method does not throw exceptions on failure. The fallback order is:
    /// <c>src → T</c> if successful → else <c>default → T</c> if successful → else <c>default(T)</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Numbers / booleans
    /// var i1 = "1,234".xValue2&lt;int&gt;(opt: new ConvertOptions { NumberStyles = NumberStyles.AllowThousands });
    /// var b1 = "Y".xValue2&lt;bool&gt;();   // true
    ///
    /// // Date and time
    /// var dt = "2025-08-27 13:05:00".xValue2&lt;DateTime&gt;();
    ///
    /// // Guid / Enum
    /// var id = "b3f5c3f4-5b9f-4a8e-b5a2-6fda2c9f6d7a".xValue2&lt;Guid&gt;();
    /// var color = "DarkBlue".xValue2&lt;ConsoleColor&gt;();
    ///
    /// // Fallback to provided default
    /// var port = "not-a-number".xValue2&lt;int&gt;(default: 8080);   // 8080
    ///
    /// // Empty string treated as null, returns default(T)
    /// var v = "   ".xValue2&lt;int?&gt;(opt: new ConvertOptions { EmptyStringIsNull = true }); // null
    /// </code>
    /// </example>
    /// <seealso cref="FlexibleConvert"/>
    /// <seealso cref="ConvertOptions"/>
    public static T xValue2<T>(this object src, object @default = null, ConvertOptions opt = null)
    {
        // default도 한 번만 변환 시도(분기 최소화)
        T defV = default;
        if (@default is not null && FastConvert.TryChangeType(@default, out T tmp, opt)) defV = tmp;

        return FastConvert.ChangeType(src, defV, opt);
    }
}

public sealed class ConvertOptions
{
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;
    public NumberStyles NumberStyles { get; init; } = NumberStyles.Float | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign;
    public bool EnumIgnoreCase { get; init; } = true;
    public string[] DateTimeFormats { get; init; } = new[] { "yyyy-MM-dd", "yyyyMMdd", "yyyy-MM-dd HH:mm:ss", "O", "s" };
    public string[] TimeSpanFormats { get; init; } = new[] { "c", @"hh\:mm\:ss", @"hh\:mm", @"d\.hh\:mm\:ss" };
    public bool EmptyStringIsNull { get; init; } = true;
}

public static class FastConvert
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> TcCache = new();

    // --- Public API ---
    public static T ChangeType<T>(object src, T @default = default, ConvertOptions? opt = null)
        => TryChangeType(src, out T v, opt) ? v : @default;

    public static bool TryChangeType<T>(object src, out T value, ConvertOptions? opt = null)
    {
        opt ??= new ConvertOptions();
        if (TryChangeType(src, typeof(T), out var boxed, opt))
        { value = (T)boxed!; return true; }
        value = default!; return false;
    }

    public static object ChangeType(object src, Type targetType, object @default = null, ConvertOptions opt = null)
        => TryChangeType(src, targetType, out var v, opt ?? new ConvertOptions()) ? v : @default;

    private static bool TryChangeType(object src, Type targetType, out object result, ConvertOptions opt)
    {
        // null/DBNULL
        if (src is null || src is DBNull)
        { result = GetDefault(targetType); return true; }

        // Nullable 언랩
        var u = Nullable.GetUnderlyingType(targetType);
        var eff = u ?? targetType;

        // 동일 형식
        if (eff.IsInstanceOfType(src)) { result = src; return true; }

        // 문자열 소스 전처리
        string? s = src as string;
        if (s is not null && opt.EmptyStringIsNull && string.IsNullOrWhiteSpace(s))
        { result = GetDefault(targetType); return true; }

        // JsonElement
        if (src is JsonElement je && TryFromJsonElement(je, eff, opt, out result)) return true;

        // Enum
        if (eff.IsEnum && TryToEnum(src, eff, opt.EnumIgnoreCase, out result)) return true;

        // 특수 타입
        if (eff == typeof(Guid)            && TryToGuid(src, out var g))   { result = g;   return true; }
        if (eff == typeof(DateTime)        && TryToDateTime(src, opt, out var dt)) { result = dt; return true; }
        if (eff == typeof(DateTimeOffset)  && TryToDateTimeOffset(src, opt, out var dto)) { result = dto; return true; }
        if (eff == typeof(TimeSpan)        && TryToTimeSpan(src, opt, out var ts)) { result = ts; return true; }
        if (eff == typeof(bool)            && TryToBoolean(src, out var b)) { result = b;  return true; }

        // 숫자 (TypeCode 스위치로 분기 비용 최소화)
        if (TryToNumber(src, eff, opt, out result)) return true;

        // 마지막 폴백: TypeConverter
        try
        {
            var tc = TcCache.GetOrAdd(eff, TypeDescriptor.GetConverter);
            if (tc is not null)
            {
                if (tc.CanConvertFrom(src.GetType())) { result = tc.ConvertFrom(null, opt.Culture, src); return true; }
                if (s is not null && tc.CanConvertFrom(typeof(string))) { result = tc.ConvertFrom(null, opt.Culture, s); return true; }
            }
        }
        catch { /* ignore */ }

        result = null;
        return false;
    }

    // --- Helpers (최적화 경로만 유지) ---
    private static bool TryFromJsonElement(JsonElement je, Type t, ConvertOptions opt, out object result)
    {
        try
        {
            switch (je.ValueKind)
            {
                case JsonValueKind.String:
                    if (t == typeof(string)) { result = je.GetString(); return true; }
                    if (t == typeof(Guid) && Guid.TryParse(je.GetString(), out var g)) { result = g; return true; }
                    if (t == typeof(DateTime) && DateTime.TryParse(je.GetString(), opt.Culture, DateTimeStyles.RoundtripKind, out var dt)) { result = dt; return true; }
                    if (t == typeof(DateTimeOffset) && DateTimeOffset.TryParse(je.GetString(), opt.Culture, DateTimeStyles.RoundtripKind, out var dto)) { result = dto; return true; }
                    break;
                case JsonValueKind.Number:
                    if (t == typeof(int)    && je.TryGetInt32(out var i32)) { result = i32; return true; }
                    if (t == typeof(long)   && je.TryGetInt64(out var i64)) { result = i64; return true; }
                    if (t == typeof(double) && je.TryGetDouble(out var d))  { result = d;  return true; }
                    if (t == typeof(decimal) && decimal.TryParse(je.GetRawText(), NumberStyles.Any, opt.Culture, out var dec)) { result = dec; return true; }
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    if (t == typeof(bool)) { result = je.GetBoolean(); return true; }
                    break;
            }
            // 복합 타입: 직렬화 경로 (필요 시)
            result = JsonSerializer.Deserialize(je.GetRawText(), t);
            return result is not null;
        }
        catch { result = null; return false; }
    }

    private static bool TryToEnum(object src, Type enumType, bool ignoreCase, out object result)
    {
        if (src is string es)
        {
            if (Enum.TryParse(enumType, es, ignoreCase, out var ev)) { result = ev; return true; }
            if (long.TryParse(es, NumberStyles.Integer, CultureInfo.InvariantCulture, out var num))
            { result = Enum.ToObject(enumType, num); return true; }
        }
        else if (IsNumeric(src.GetType()))
        {
            var num = System.Convert.ToInt64(src, CultureInfo.InvariantCulture);
            result = Enum.ToObject(enumType, num);
            return true;
        }
        result = null; return false;
    }

    private static bool TryToGuid(object src, out Guid g)
    {
        if (src is Guid gg) { g = gg; return true; }
        if (src is string s && Guid.TryParse(s.Trim(), out g)) return true;
        g = default; return false;
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
        dt = default; return false;
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
        dto = default; return false;
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
        ts = default; return false;
    }

    private static bool TryToBoolean(object src, out bool b)
    {
        if (src is bool bb) { b = bb; return true; }
        if (src is string s)
        {
            s = s.Trim();
            if (bool.TryParse(s, out b)) return true;
            // 고속 매핑: 길이로 선분기
            switch (s.Length)
            {
                case 1:
                    b = s[0] is '1' or 'y' or 'Y' or 't' or 'T';
                    if (!b) b = !(s[0] is not ('0' or 'n' or 'N' or 'f' or 'F')) ? false : b;
                    return s[0] is '1' or 'y' or 'Y' or 't' or 'T' or '0' or 'n' or 'N' or 'f' or 'F';
                case 2:
                    if (s.Equals("on", StringComparison.OrdinalIgnoreCase)) { b = true; return true; }
                    if (s.Equals("no", StringComparison.OrdinalIgnoreCase)) { b = false; return true; }
                    break;
                case 3:
                    if (s.Equals("yes", StringComparison.OrdinalIgnoreCase)) { b = true; return true; }
                    off:
                    break;
                case 4:
                    if (s.Equals("true", StringComparison.OrdinalIgnoreCase)) { b = true; return true; }
                    if (s.Equals("off",  StringComparison.OrdinalIgnoreCase)) { b = false; return true; }
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
        b = default; return false;
    }

    private static bool TryToNumber(object src, Type t, ConvertOptions opt, out object result)
    {
        result = null;

        // 문자열이면 TryParse 직접
        if (src is string s)
        {
            s = s.Trim();
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:    if (byte.TryParse(s,   opt.NumberStyles, opt.Culture, out var b)) { result = b; return true; } break;
                case TypeCode.SByte:   if (sbyte.TryParse(s,  opt.NumberStyles, opt.Culture, out var sb)) { result = sb; return true; } break;
                case TypeCode.Int16:   if (short.TryParse(s,  opt.NumberStyles, opt.Culture, out var i16)) { result = i16; return true; } break;
                case TypeCode.UInt16:  if (ushort.TryParse(s, opt.NumberStyles, opt.Culture, out var ui16)) { result = ui16; return true; } break;
                case TypeCode.Int32:   if (int.TryParse(s,    opt.NumberStyles, opt.Culture, out var i32)) { result = i32; return true; } break;
                case TypeCode.UInt32:  if (uint.TryParse(s,   opt.NumberStyles, opt.Culture, out var ui32)) { result = ui32; return true; } break;
                case TypeCode.Int64:   if (long.TryParse(s,   opt.NumberStyles, opt.Culture, out var i64)) { result = i64; return true; } break;
                case TypeCode.UInt64:  if (ulong.TryParse(s,  opt.NumberStyles, opt.Culture, out var ui64)) { result = ui64; return true; } break;
                case TypeCode.Single:  if (float.TryParse(s,  opt.NumberStyles, opt.Culture, out var f)) { result = f; return true; } break;
                case TypeCode.Double:  if (double.TryParse(s, opt.NumberStyles, opt.Culture, out var d)) { result = d; return true; } break;
                case TypeCode.Decimal: if (decimal.TryParse(s,opt.NumberStyles, opt.Culture, out var m)) { result = m; return true; } break;
                default: return false;
            }
            return false;
        }

        // 문자열이 아니면 IConvertible 경로(빠름, 예외 드뭄)
        try
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:    result = System.Convert.ToByte(src,  opt.Culture); return true;
                case TypeCode.SByte:   result = System.Convert.ToSByte(src, opt.Culture); return true;
                case TypeCode.Int16:   result = System.Convert.ToInt16(src, opt.Culture); return true;
                case TypeCode.UInt16:  result = System.Convert.ToUInt16(src,opt.Culture); return true;
                case TypeCode.Int32:   result = System.Convert.ToInt32(src, opt.Culture); return true;
                case TypeCode.UInt32:  result = System.Convert.ToUInt32(src,opt.Culture); return true;
                case TypeCode.Int64:   result = System.Convert.ToInt64(src, opt.Culture); return true;
                case TypeCode.UInt64:  result = System.Convert.ToUInt64(src,opt.Culture); return true;
                case TypeCode.Single:  result = System.Convert.ToSingle(src,opt.Culture); return true;
                case TypeCode.Double:  result = System.Convert.ToDouble(src,opt.Culture); return true;
                case TypeCode.Decimal: result = System.Convert.ToDecimal(src,opt.Culture); return true;
                default: return false;
            }
        }
        catch { result = null; return false; }
    }

    private static bool IsNumeric(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        switch (Type.GetTypeCode(t))
        {
            case TypeCode.Byte: case TypeCode.SByte:
            case TypeCode.Int16: case TypeCode.UInt16:
            case TypeCode.Int32: case TypeCode.UInt32:
            case TypeCode.Int64: case TypeCode.UInt64:
            case TypeCode.Single: case TypeCode.Double:
            case TypeCode.Decimal: return true;
            default: return false;
        }
    }

    private static bool IsNullable(Type t) => !t.IsValueType || Nullable.GetUnderlyingType(t) is not null;
    private static object GetDefault(Type t) => IsNullable(t) ? null : Activator.CreateInstance(t);
}
