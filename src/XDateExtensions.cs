using System.Globalization;

namespace eXtensionSharp
{
    public static class XDateExtensions
    {
































        

        





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