using System.Globalization;

namespace eXtensionSharp
{
    public static class XDateExtensions
    {
        /// <summary>
        /// int date to datetime
        /// </summary>
        /// <param name="date">20241231</param>
        /// <returns></returns>
        public static DateTime? xToYearMonthDay(this int date)
        {
            var str = date.xValue<string>();
            if(str.Length < 8) return null;
            
            var dt = new DateTime(
                int.Parse(str.xSubstring(0, 4)), 
                int.Parse(str.xSubstring(4, 2)), 
                int.Parse(str.xSubstring(6, 2)));
            return dt;
        }
        
        /// <summary>
        /// string date to datetime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? xConvertToDate(this string date)
        {
            if(DateTime.TryParse(date, out var datetime)) return datetime;
            return null;
        }

        /// <summary>
        /// convert date to format date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format">yyyy-MM-dd</param>
        /// <returns></returns>
        public static string xToDateFormat(this DateTime date, string format = null)
        {
            if (format.xIsEmpty()) format = "yyyy-MM-dd";
            return date.ToString(format);
        }

        /// <summary>
        /// convert date to culture format date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="format">y, m, d</param>
        /// <returns></returns>
        public static string xToDateFormat(this DateTime date, CultureInfo cultureInfo, string format = null)
        {
            if (format.xIsEmpty()) format = "d";
            return date.ToString(format, cultureInfo);
        }

        public static string xToYear(this DateTime date)
        {
            return date.ToString("yyyy");
        }

        public static string xToMonth(this DateTime date)
        {
            return date.ToString("MM");
        }

        public static string xToDay(this DateTime date)
        {
            return date.ToString("dd");
        }
        
        public static string xToHour(this DateTime date)
        {
            return date.ToString("hh");
        }        

        public static string xToMinute(this DateTime date)
        {
            return date.ToString("mm");
        }

        public static string xToSecond(this DateTime date)
        {
            return date.ToString("ss");
        }
        
        /// <summary>
        /// Get Year (2Letters)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string xToShortYear(this DateTime date)
        {
            return date.ToString("yy");
        }

        /// <summary>
        /// Get Month (Full Letters)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public static string xToMonthName(this DateTime dateTime, string cultureName = "en-US")
        {
            var culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetMonthName(dateTime.Month);
        }
        
        /// <summary>
        /// Get Month (3Letters)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public static string xToShortMonthName(this DateTime dateTime, string cultureName = "en-US")
        {
            var culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }
        
        /// <summary>
        /// convert date to from date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isMonth"></param>
        /// <remarks>
        /// name change xToFromDate to xFromDate.
        /// why this need?
        /// This is because when searching for dates, you need to check by day, and in the case of monthly searches, you need to check down to the hour, minute and second.
        /// query example:
        ///     select * from user where date >= @startDate and date < @endDate 
        /// </remarks>
        /// <example> 
        /// var now = DateTime.Now; //2024-11-14 15:44:40
        /// var from = now.xFromDate(); //2024-11-14 00:00:00
        /// var month = now.xFromDate(true); //2024-11-01 00:00:00
        /// </example>
        /// <returns></returns>
        public static DateTime xFromDate(this DateTime dateTime, bool isMonth = false)
        {
            if (isMonth) return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// convert date to to date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isMonth"></param>
        /// <remarks>
        /// name change xToFromDate to xFromDate.
        /// why this need?
        /// This is because when searching for dates, you need to check by day, and in the case of monthly searches, you need to check down to the hour, minute and second.
        /// query example:
        ///     select * from user where date >= @startDate and date < @endDate 
        /// </remarks>
        /// <example> 
        /// var now = DateTime.Now; //2024-11-14 15:44:40
        /// var from = now.xToDate() //2024-11-15 00:00:00
        /// var month = now.xToDate() //2024-12-01 00:00:00
        /// </example>
        /// <returns></returns>
        public static DateTime xToDate(this DateTime dateTime, bool isMonth = false)
        {
            if (isMonth) return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0).AddMonths(1);
            return (new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0)).AddDays(1);
        }

        /// <summary>
        /// get last day of year month
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime xToLastDate(this DateTime dateTime)
        {
            var lastDay = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year, dateTime.Month, lastDay, 0, 0, 0, 0);
        }
        
        /// <summary>
        /// convert datetime to integer
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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
        /// get week count in month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
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
        /// get start date of week (monday date)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
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
        /// get end date of week (sunday date)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
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
        /// 날짜의 특정 주의 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="week"></param>
        /// <returns></returns>
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
        /// compare year
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool xIsYearEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year == to!.Value.Year;
        }
        
        /// <summary>
        /// compare year month
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool xIsMonthEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from.xIsYearEquals(to) && (from!.Value.Month == to!.Value.Month);
        }
        
        /// <summary>
        /// compare year month day
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool xIsDayEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from.xIsYearEquals(to) && from.xIsMonthEquals(from) && (from!.Value.Day == to!.Value.Day);
        }

        public static TimeSpan xFromMilliseconds(this int i)
        {
            if(i <= 0) return TimeSpan.Zero;
            return TimeSpan.FromMilliseconds(i);
        }

        public static TimeSpan xFromSeconds(this int i)
        {
            if(i <= 0) return TimeSpan.Zero;
            return TimeSpan.FromSeconds(i);
        }
        
        public static TimeSpan xFromMinutes(this int i)
        {
            if(i <= 0) return TimeSpan.Zero;
            return TimeSpan.FromMinutes(i);
        }

        /// <summary>
        /// get day of week (string)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="culture"></param>
        /// <example>
        /// var now = DateTime.Now;
        /// var dayofweek = now.xToDayOfWeek(); //Monday
        /// </example>
        /// <returns></returns>
        public static string xToDayOfWeek(this DateTime date, string culture = null)
        {
            if (culture.xIsEmpty()) culture = CultureInfo.CurrentCulture.Name;
            return date.ToString("dddd", new CultureInfo(culture));
        }

        /// <summary>
        /// Unix timestamp(second) to Datetime
        /// </summary>
        /// <param name="tsSecond"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public static DateTime xFromUnixTimestampSecToDateTime(this long tsSecond, bool local = true)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(tsSecond);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }
        
        /// <summary>
        /// Unix timestamp(millisecond) to Datetime
        /// </summary>
        /// <param name="tsMs"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public static DateTime xFromUnixTimestampMsToDateTime(this long tsMs, bool local = true)
        {
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(tsMs);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }

        public static DateTime xToTimezoneDate(this DateTime date, string timezoneId = "Korea Standard Time")
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(date, timezone);
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
}