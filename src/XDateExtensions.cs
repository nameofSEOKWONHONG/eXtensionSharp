using System;
using System.Globalization;

namespace eXtensionSharp
{
    public static class XDateExtensions
    {
        public static DateTime xToDate(this string date)
        {
            var datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        /// <summary>
        /// Get Date By ENUM_DATE_FORMAT, Default yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string xToDate(this DateTime date, ENUM_DATE_FORMAT format = null)
        {
            if (!format.xIsEmpty()) format = ENUM_DATE_FORMAT.YYYY_MM_DD;
            return date.ToString(format);
        }

        /// <summary>
        /// Get Year-Month-Date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string xToDate(this DateTime date, string format = "yyyy-MM-dd")
        {
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

        public static DateTime xToFromDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime xToToDate(this DateTime dateTime)
        {
            return dateTime.xToFromDate().AddDays(1);
        }
    }

    public class ENUM_DATE_FORMAT : XEnumBase<ENUM_DATE_FORMAT>
    {
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD = Define("yyyy-MM-dd");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS = Define("yyyy-MM-dd HH:mm:ss");
        public static readonly ENUM_DATE_FORMAT YYYYMMDD = Define("yyyyMMdd");
        public static readonly ENUM_DATE_FORMAT YYYY_FS_MM_FS_DD = Define("yyyy/MM/dd");
        public static readonly ENUM_DATE_FORMAT YYYYMMDDHHMMSS = Define("yyyyMMddHHmmss");
        public static readonly ENUM_DATE_FORMAT HHMMSS = Define("HHmmss");
    }
}