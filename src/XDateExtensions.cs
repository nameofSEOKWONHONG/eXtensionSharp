using System.Globalization;

namespace eXtensionSharp
{
    public static class XDateExtensions
    {
        /// <summary>
        /// Converts an integer date (e.g., 20241231) to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="date">Integer representation of the date (e.g., 20241231).</param>
        /// <returns>A <see cref="DateTime"/> object if successful; otherwise, null.</returns>
        public static DateTime? xToYearMonthDay(this int date)
        {
            var str = date.xValue<string>();
            if (str.Length < 8) return null;

            var dt = new DateTime(
                int.Parse(str.xSubstring(0, 4)),
                int.Parse(str.xSubstring(4, 2)),
                int.Parse(str.xSubstring(6, 2)));
            return dt;
        }

        /// <summary>
        /// Converts a string representation of a date to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="date">The string representation of the date.</param>
        /// <returns>A <see cref="DateTime"/> object if successful; otherwise, null.</returns>
        public static DateTime? xConvertToDate(this string date)
        {
            if (DateTime.TryParse(date, out var datetime)) return datetime;
            return null;
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> object into a string with a specified format.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> to format.</param>
        /// <param name="format">The desired date format (default: "yyyy-MM-dd").</param>
        /// <returns>A formatted string representation of the date.</returns>
        public static string xToDateFormat(this DateTime date, string format = null)
        {
            if (format.xIsEmpty()) format = "yyyy-MM-dd";
            return date.ToString(format);
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> object into a string using a specific culture and format.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> to format.</param>
        /// <param name="cultureInfo">The culture to use for formatting.</param>
        /// <param name="format">The desired date format (default: "d").</param>
        /// <returns>A formatted string representation of the date.</returns>
        public static string xToDateFormat(this DateTime date, CultureInfo cultureInfo, string format = null)
        {
            if (format.xIsEmpty()) format = "d";
            return date.ToString(format, cultureInfo);
        }

        /// <summary>
        /// Extracts the year from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToYear(this DateTime date)
        {
            return date.ToString("yyyy");
        }

        /// <summary>
        /// Extracts the month from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToMonth(this DateTime date)
        {
            return date.ToString("MM");
        }

        /// <summary>
        /// Extracts the day from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToDay(this DateTime date)
        {
            return date.ToString("dd");
        }

        /// <summary>
        /// Extracts the hour (12-hour format) from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToHour(this DateTime date)
        {
            return date.ToString("hh");
        }

        /// <summary>
        /// Extracts the minutes from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToMinute(this DateTime date)
        {
            return date.ToString("mm");
        }

        /// <summary>
        /// Extracts the seconds from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToSecond(this DateTime date)
        {
            return date.ToString("ss");
        }

        /// <summary>
        /// Extracts the short (two-digit) year from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public static string xToShortYear(this DateTime date)
        {
            return date.ToString("yy");
        }

        /// <summary>
        /// Gets the full name of the month from a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object.</param>
        /// <param name="cultureName">The culture to use for formatting (default: "en-US").</param>
        /// <returns>The full month name.</returns>
        public static string xToMonthName(this DateTime dateTime, string cultureName = "en-US")
        {
            var culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        /// <summary>
        /// Gets the abbreviated (three-letter) name of the month from a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object.</param>
        /// <param name="cultureName">The culture to use for formatting (default: "en-US").</param>
        /// <returns>The abbreviated month name.</returns>
        public static string xToShortMonthName(this DateTime dateTime, string cultureName = "en-US")
        {
            var culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to the start of the day (00:00:00).
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object.</param>
        /// <param name="isMonth">If true, returns the first day of the month at 00:00:00.</param>
        /// <returns>The adjusted <see cref="DateTime"/> object.</returns>
        public static DateTime xFromDate(this DateTime dateTime, bool isMonth = false)
        {
            if (isMonth) return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to the next day at 00:00:00.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object.</param>
        /// <param name="isMonth">If true, returns the first day of the next month at 00:00:00.</param>
        /// <returns>The adjusted <see cref="DateTime"/> object.</returns>
        public static DateTime xToDate(this DateTime dateTime, bool isMonth = false)
        {
            if (isMonth) return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0).AddMonths(1);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0).AddDays(1);
        }


        /// <summary>
        /// Retrieves the last day of the specified year and month.
        /// </summary>
        /// <param name="dateTime">The input date to calculate the last day of its month.</param>
        /// <returns>A <see cref="DateTime"/> object representing the last day of the month at 00:00:00.</returns>
        public static DateTime xToLastDate(this DateTime dateTime)
        {
            var lastDay = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year, dateTime.Month, lastDay, 0, 0, 0, 0);
        }
        
        /// <summary>
        /// Converts a <see cref="DateTime"/> object into an integer formatted as yyyyMMdd.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> object to convert.</param>
        /// <returns>An integer representation of the date, or 0 if the conversion fails.</returns>
        public static int xToDigitize(this DateTime dt)
        {
            if (int.TryParse($"{dt.Year}{dt.Month.ToString().PadLeft(2, '0')}{dt.Day.ToString().PadLeft(2, '0')}",
                    out int yearMonthDay))
            {
                return yearMonthDay;
            }

            return 0;
        }

        /// <summary>
        /// Calculates the number of Mondays (weeks) in the given month.
        /// </summary>
        /// <param name="date">The date to determine the month for calculation.</param>
        /// <returns>The number of weeks (Mondays) in the month.</returns>
        public static int xWeekCountInMonth(this DateTime date)
        {
            // first generate all dates in the month of 'date'
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
            // then filter the only the start of weeks
            var weekends = from d in dates
                where d.DayOfWeek == DayOfWeek.Monday
                select d;
            return weekends.Count();
        }

        /// <summary>
        /// Gets the start date of the week (Monday) for the specified date.
        /// </summary>
        /// <param name="date">The date for which to calculate the start of the week.</param>
        /// <returns>A <see cref="DateTime"/> object representing the Monday of the week.</returns>
        public static DateTime xStartOfWeek(this DateTime date)
        {
            DateTime weekStart;
            int monday = 1;
            int crtDay = (int)date.DayOfWeek;
            if (date.DayOfWeek == DayOfWeek.Sunday)
                crtDay = 7;
            int difference = crtDay - monday;
            weekStart = date.AddDays(-difference);
            return weekStart;
        }

        /// <summary>
        /// Gets the end date of the week (Sunday) for the specified date.
        /// </summary>
        /// <param name="date">The date for which to calculate the end of the week.</param>
        /// <returns>A <see cref="DateTime"/> object representing the Sunday of the week.</returns>
        public static DateTime xEndOfWeek(this DateTime date)
        {
            DateTime weekStart;
            int sunday = 7;
            int crtDay = (int)date.DayOfWeek;
            if (date.DayOfWeek == DayOfWeek.Sunday)
                crtDay = 7;
            int difference = sunday - crtDay;
            weekStart = date.AddDays(difference);
            return weekStart;
        }

        /// <summary>
        /// Finds the first occurrence of the specified day of the week in the month of the given date.
        /// </summary>
        /// <param name="date">The date to determine the month for calculation.</param>
        /// <param name="week">The target day of the week (default is Saturday).</param>
        /// <returns>A <see cref="DateTime"/> object representing the first occurrence of the specified day of the week.</returns>
        public static DateTime xDateForDayInWeek(this DateTime date, DayOfWeek week = DayOfWeek.Saturday)
        {
            DateTime dt = new DateTime(date.Year, date.Month, 1); // 현재 월의 첫 날
            while (date.DayOfWeek != week) // 첫째 주 토요일 찾기
            {
                date = date.AddDays(1);
            }

            return date;
        }
        
        /// <summary>
        /// Compares whether two nullable <see cref="DateTime"/> objects have the same year.
        /// </summary>
        /// <param name="from">The first date to compare.</param>
        /// <param name="to">The second date to compare.</param>
        /// <returns>True if the years are equal; otherwise, false.</returns>
        public static bool xIsYearEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year == to!.Value.Year;
        }
        
        /// <summary>
        /// Compares whether two nullable <see cref="DateTime"/> objects have the same year and month.
        /// </summary>
        /// <param name="from">The first date to compare.</param>
        /// <param name="to">The second date to compare.</param>
        /// <returns>True if the year and month are equal; otherwise, false.</returns>
        public static bool xIsMonthEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from.xIsYearEquals(to) && (from!.Value.Month == to!.Value.Month);
        }
        
        /// <summary>
        /// Compares whether two nullable <see cref="DateTime"/> objects have the same year, month, and day.
        /// </summary>
        /// <param name="from">The first date to compare.</param>
        /// <param name="to">The second date to compare.</param>
        /// <returns>True if the year, month, and day are equal; otherwise, false.</returns>
        public static bool xIsDayEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from.xIsYearEquals(to) && from.xIsMonthEquals(from) && (from!.Value.Day == to!.Value.Day);
        }

        /// <summary>
        /// Converts an integer representing milliseconds to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="i">The number of milliseconds.</param>
        /// <returns>A <see cref="TimeSpan"/> object, or <see cref="TimeSpan.Zero"/> if the input is less than or equal to 0.</returns>
        public static TimeSpan xFromMilliseconds(this int i)
        {
            if(i <= 0) return TimeSpan.Zero;
            return TimeSpan.FromMilliseconds(i);
        }

        /// <summary>
        /// Converts an integer representing seconds to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="i">The number of seconds.</param>
        /// <returns>A <see cref="TimeSpan"/> object, or <see cref="TimeSpan.Zero"/> if the input is less than or equal to 0.</returns>
        public static TimeSpan xFromSeconds(this int i)
        {
            if(i <= 0) return TimeSpan.Zero;
            return TimeSpan.FromSeconds(i);
        }
        
        /// <summary>
        /// Converts an integer representing minutes to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="i">The number of minutes.</param>
        /// <returns>A <see cref="TimeSpan"/> object, or <see cref="TimeSpan.Zero"/> if the input is less than or equal to 0.</returns>
        public static TimeSpan xFromMinutes(this int i)
        {
            if(i <= 0) return TimeSpan.Zero;
            return TimeSpan.FromMinutes(i);
        }

        /// <summary>
        /// Retrieves the day of the week as a string in the specified culture.
        /// </summary>
        /// <param name="date">The date to retrieve the day of the week for.</param>
        /// <param name="culture">The culture for the day name (default is the current culture).</param>
        /// <returns>The name of the day of the week.</returns>
        public static string xToDayOfWeek(this DateTime date, string culture = null)
        {
            if (culture.xIsEmpty()) culture = CultureInfo.CurrentCulture.Name;
            return date.ToString("dddd", new CultureInfo(culture));
        }

        /// <summary>
        /// Converts a Unix timestamp (in seconds) to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="tsSecond">The Unix timestamp in seconds.</param>
        /// <param name="local">Whether to return the date in local time (default is true).</param>
        /// <returns>A <see cref="DateTime"/> object representing the timestamp.</returns>
        public static DateTime xToDateTime(this long tsSecond, bool local = true)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(tsSecond);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }
        
        /// <summary>
        /// Converts a Unix timestamp (in milliseconds) to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="tsMs">The Unix timestamp in milliseconds.</param>
        /// <param name="local">Whether to return the date in local time (default is true).</param>
        /// <returns>A <see cref="DateTime"/> object representing the timestamp.</returns>
        public static DateTime xToDateTimeMs(this long tsMs, bool local = true)
        {
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(tsMs);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }
        
        /// <summary>
        /// Converts a <see cref="DateTime"/> object to a specified timezone.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <param name="timezoneId">The target timezone ID (default is "Korea Standard Time").</param>
        /// <returns>A <see cref="DateTime"/> object in the specified timezone.</returns>
        public static DateTime xConvertDateTime(this DateTime date, string timezoneId = "Korea Standard Time")
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(date, timezone);
        }
    }
}


// public static class WeekHelper {
//
// #region Public Methods
// public static DateTime GetWeekStart(DateTime date) {
//     DateTime weekStart;
//     int monday = 1;
//     int crtDay = (int)date.DayOfWeek;
//     if (date.DayOfWeek == DayOfWeek.Sunday)
//         crtDay = 7;
//     int difference = crtDay - monday;
//     weekStart = date.AddDays(-difference);
//     return weekStart;
// }
// public static DateTime GetWeekStop(DateTime date) {
//     DateTime weekStart;
//     int sunday = 7;
//     int crtDay = (int)date.DayOfWeek;
//     if (date.DayOfWeek == DayOfWeek.Sunday)
//         crtDay = 7;
//     int difference = sunday - crtDay;
//     weekStart = date.AddDays(difference);
//     return weekStart;
// }
// public static void GetWeekInterval(int year, int weekNo,
//     out DateTime weekStart, out DateTime weekStop) {
//     GetFirstWeekOfYear(year, out weekStart, out weekStop);
//     if (weekNo == 1)
//         return;
//     weekNo--;
//     int daysToAdd = weekNo * 7;
//     DateTime dt = weekStart.AddDays(daysToAdd);
//     GetWeekInterval(dt, out weekStart, out weekStop);
// }
// public static List<KeyValuePair<DateTime, DateTime>> GetWeekSeries(DateTime toDate) {
//     //gets week series from beginning of the year
//     DateTime dtStartYear = new DateTime(toDate.Year, 1, 1);
//     List<KeyValuePair<DateTime, DateTime>> list = GetWeekSeries(dtStartYear, toDate);
//     if (list.Count > 0) {
//         KeyValuePair<DateTime, DateTime> week = list[0];
//         list[0] = new KeyValuePair<DateTime, DateTime>(dtStartYear, week.Value);
//     }
//     return list;
// }
// public static List<KeyValuePair<DateTime, DateTime>> GetWeekSeries(DateTime fromDate, DateTime toDate) {
//     if (fromDate > toDate)
//         return null;
//     List<KeyValuePair<DateTime, DateTime>> list = new List<KeyValuePair<DateTime, DateTime>>(100);
//     DateTime weekStart, weekStop;
//     toDate = GetWeekStop(toDate);
//     while (fromDate <= toDate) {
//         GetWeekInterval(fromDate, out weekStart, out weekStop);
//         list.Add(new KeyValuePair<DateTime, DateTime>(weekStart, weekStop));
//         fromDate = fromDate.AddDays(7);
//     }
//     return list;
// }
// public static void GetFirstWeekOfYear(int year, out DateTime weekStart, out DateTime weekStop) {
//     DateTime date = new DateTime(year, 1, 1);
//     GetWeekInterval(date, out weekStart, out weekStop);
// }
// public static void GetWeekInterval(DateTime date,
//     out DateTime dtWeekStart, out DateTime dtWeekStop) {
//     dtWeekStart = GetWeekStart(date);
//     dtWeekStop = GetWeekStop(date);
// }
// #endregion Public Methods
// }    

/// <summary>
/// datetime format from korea.
/// </summary>
// public class ENUM_DATE_FORMAT : ENUM_DATE_FORMAT
// {
//     public static readonly ENUM_DATE_FORMAT YYYY_MM_DD = Define("yyyy-MM-dd");
//     public static readonly ENUM_DATE_FORMAT YYYY_MM = Define("yyyy-MM");
//     public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS = Define("yyyy-MM-dd HH:mm:ss");
//     public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS_FFF = Define("yyyy-MM-dd HH:mm:ss.fff");
//     public static readonly ENUM_DATE_FORMAT YYYYMMDD = Define("yyyyMMdd");
//     public static readonly ENUM_DATE_FORMAT YYYYMM = Define("yyyyMM");
//     public static readonly ENUM_DATE_FORMAT YYYY_FS_MM_FS_DD = Define("yyyy/MM/dd");
//     public static readonly ENUM_DATE_FORMAT YYYYMMDDHHMMSS = Define("yyyyMMddHHmmss");
//     public static readonly ENUM_DATE_FORMAT YYYYMMDDHH = Define("yyyyMMddHH");
//     public static readonly ENUM_DATE_FORMAT HHMMSS = Define("HHmmss");
//     public static readonly ENUM_DATE_FORMAT YYMMDD = Define("yyMMdd");
// }