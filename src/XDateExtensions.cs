using System;
using System.Globalization;

namespace eXtensionSharp
{
    public static class XDateExtensions
    {
        public static DateTime xToDate(this int date)
        {
            var str = date.ToString();
            var dt = new DateTime(int.Parse(str.xSubstring(0, 4)), int.Parse(str.xSubstring(4, 2)), int.Parse(str.xSubstring(6, 2)));
            return dt;
        }
        
        public static DateTime xToDate(this string date)
        {
            var datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        public static string xToDate(this DateTime date, ENUM_DATE_FORMAT format = null)
        {
            if (format.xIsEmpty()) format = ENUM_DATE_FORMAT.YYYY_MM_DD;
            return date.ToString(format);
        }

        public static DateTime xToMin(this DateTime date)
        {
            return DateTime.Parse(date.ToShortDateString());
        }

        public static DateTime xToMax(this DateTime date)
        {
            return DateTime.Parse($"{date.AddDays(1).ToShortDateString()}");
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
        /// Get Year (2Letters)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string xToShortYear(this DateTime date)
        {
            return date.ToString("yyyy").xSubstring(2, 2);
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

        public static DateTime xToFromDate(this DateTime dateTime, bool isMonth = false)
        {
            if (isMonth)
            {
                return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0); 
            }
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        public static DateTime xToToDate(this DateTime dateTime, bool isMonth = false)
        {
            if (isMonth)
            {
                return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0).AddMonths(1);
            }
            return (new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0)).AddDays(1);
        }

        public static DateTime xToLastDate(this DateTime dateTime)
        {
            var lastDay = dateTime.xToLastDay();
            return new DateTime(dateTime.Year, dateTime.Month, lastDay, 0, 0, 0, 0);
        }
        
        public static int xToDigitize(this DateTime dt)
        {
            if (int.TryParse($"{dt.Year}{dt.Month.ToString().PadLeft(2, '0')}{dt.Day.ToString().PadLeft(2, '0')}",
                    out int yearMonthDay))
            {
                return yearMonthDay;
            }

            return 0;
        }

        public static int xToMonth(this int yearMonthDay)
        {
            var str = yearMonthDay.ToString();
            return str.xSubstring(4, 2).xValue<int>(0);
        }

        public static int xToDay(this int yearMonthDay)
        {
            var str = yearMonthDay.ToString();
            return str.xSubstring(6, 2).xValue<int>(0);
        }

        public static int xToWeekCount(this DateTime date)
        {
            // first generate all dates in the month of 'date'
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
            // then filter the only the start of weeks
            var weekends = from d in dates
                where d.DayOfWeek == DayOfWeek.Monday
                select d;
            return weekends.Count();
        }

        public static DateTime xToWeekStart(this DateTime date)
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

        public static DateTime xToWeekStop(this DateTime date)
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

        public static int xToLastDay(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        public static DateTime xToFirstSaturdayPerMonth(this DateTime date, DayOfWeek week = DayOfWeek.Saturday)
        {
            DateTime dt = new DateTime(date.Year, date.Month, 1); // 현재 월의 첫 날
            while (date.DayOfWeek != week) // 첫째 주 토요일 찾기
            {
                date = date.AddDays(1);
            }

            return date;
        }
        
        
        public static bool xIsYearEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from!.Value.Year == to!.Value.Year;
        }
        
        public static bool xIsMonthEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from.xIsYearEquals(to) && (from!.Value.Month == to!.Value.Month);
        }
        
        public static bool xIsDayEquals(this DateTime? from, DateTime? to)
        {
            if (from.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return from.xIsYearEquals(to) && from.xIsMonthEquals(from) && (from!.Value.Day == to!.Value.Day);
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

    public class ENUM_DATE_FORMAT : XEnumBase<ENUM_DATE_FORMAT>
    {
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD = Define("yyyy-MM-dd");
        public static readonly ENUM_DATE_FORMAT YYYY_MM = Define("yyyy-MM");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS = Define("yyyy-MM-dd HH:mm:ss");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS_FFF = Define("yyyy-MM-dd HH:mm:ss.fff");
        public static readonly ENUM_DATE_FORMAT YYYYMMDD = Define("yyyyMMdd");
        public static readonly ENUM_DATE_FORMAT YYYYMM = Define("yyyyMM");
        public static readonly ENUM_DATE_FORMAT YYYY_FS_MM_FS_DD = Define("yyyy/MM/dd");
        public static readonly ENUM_DATE_FORMAT YYYYMMDDHHMMSS = Define("yyyyMMddHHmmss");
        public static readonly ENUM_DATE_FORMAT YYYYMMDDHH = Define("yyyyMMddHH");
        public static readonly ENUM_DATE_FORMAT HHMMSS = Define("HHmmss");
        public static readonly ENUM_DATE_FORMAT YYMMDD = Define("yyMMdd");
    }
}