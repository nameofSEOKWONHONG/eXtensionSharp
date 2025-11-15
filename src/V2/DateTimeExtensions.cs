using System;
using System.Globalization;

namespace eXtensionSharp;

public static class DateTimeExtensions
{
    extension(DateTime source)
    {
        /// <summary>
        /// Checks if a DateTime value is between two other DateTime values, inclusive.
        /// </summary>        
        /// <param name="from">The lower bound DateTime.</param>
        /// <param name="to">The upper bound DateTime.</param>
        /// <returns>True if the DateTime value is between the two bounds, otherwise false.</returns>
        public bool xIsBetween(DateTime from, DateTime to)
        {
            if (source >= from && source <= to) return true;
            return false;
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> object into a string with a specified format.
        /// </summary>
        /// <param name="format">The desired date format (default: "yyyy-MM-dd").</param>
        /// <returns>A formatted string representation of the date.</returns>
        public string xToDateFormat(string format = null)
        {
            if (format.xIsEmpty()) format = "yyyy-MM-dd";
            return source.ToString(format);
        }  

        /// <summary>
        /// Formats a <see cref="DateTime"/> object into a string using a specific culture and format.
        /// </summary>
        /// <param name="cultureInfo">The culture to use for formatting.</param>
        /// <param name="format">The desired date format (default: "d").</param>
        /// <returns>A formatted string representation of the date.</returns>
        public string xToDate(CultureInfo cultureInfo = null, string format = "d")
        {
            if(cultureInfo.xIsEmpty()) 
                cultureInfo = Thread.CurrentThread.CurrentCulture;

            return source.ToString(format, cultureInfo);
        }

        /// <summary>
        /// Extracts the year from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public string xToYear(string format = "yyyy")
        {
            return source.ToString(format);
        }

        /// <summary>
        /// Extracts the month from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public string xToMonth()
        {
            return source.ToString("MM");
        }        

        /// <summary>
        /// Extracts the day from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public string xToDay()
        {
            return source.ToString("dd");
        }      

        /// <summary>
        /// Extracts the hour (12-hour format) from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public string xToHour(string format = "hh")
        {
            return source.ToString(format);
        }

        /// <summary>
        /// Extracts the minutes from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public string xToMinute()
        {
            return source.ToString("mm");
        }

        /// <summary>
        /// Extracts the seconds from a <see cref="DateTime"/> object as a string.
        /// </summary>
        public string xToSecond()
        {
            return source.ToString("ss");
        }        

        /// <summary>
        /// Gets the full name of the month from a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="cultureName">The culture to use for formatting (default: "en-US").</param>
        /// <returns>The full month name.</returns>
        public string xToMonthName(string cultureName = "en-US")
        {
            var culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetMonthName(source.Month);
        }      

        /// <summary>
        /// Gets the abbreviated (three-letter) name of the month from a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="cultureName">The culture to use for formatting (default: "en-US").</param>
        /// <returns>The abbreviated month name.</returns>
        public string xToShortMonthName(string cultureName = "en-US")
        {
            var culture = new CultureInfo(cultureName);
            return culture.DateTimeFormat.GetAbbreviatedMonthName(source.Month);
        }       

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to the start of the day (00:00:00).
        /// </summary>
        /// <param name="isMonth">If true, returns the first day of the month at 00:00:00.</param>
        /// <returns>The adjusted <see cref="DateTime"/> object.</returns>
        public DateTime xStartDate(bool isMonth = false)
        {
            if (isMonth) 
                return new DateTime(source.Year, source.Month, 1, 0, 0, 0, 0, source.Kind);

            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0, 0, source.Kind);
        }  

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to the next day at 00:00:00.
        /// </summary>
        /// <param name="isMonth">If true, returns the first day of the next month at 00:00:00.</param>
        /// <returns>The adjusted <see cref="DateTime"/> object.</returns>
        public DateTime xEndDate(bool isMonth = false)
        {
            if (isMonth) 
                return new DateTime(source.Year, source.Month, 1, 0, 0, 0, 0, source.Kind).AddMonths(1);

            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0, 0, source.Kind).AddDays(1);
        }

        /// <summary>
        /// Retrieves the last day of the specified year and month.
        /// </summary>
        /// <param name="dateTime">The input date to calculate the last day of its month.</param>
        /// <returns>A <see cref="DateTime"/> object representing the last day of the month at 00:00:00.</returns>
        public DateTime xToLastDate()
        {
            var lastDay = DateTime.DaysInMonth(source.Year, source.Month);
            return new DateTime(source.Year, source.Month, lastDay, 0, 0, 0, 0, source.Kind);
        }
        
        /// <summary>
        /// Converts a <see cref="DateTime"/> object into an integer formatted as yyyyMMdd.
        /// </summary>
        /// <returns>An integer representation of the date, or 0 if the conversion fails.</returns>
        public int xToDigitize()
        {
            if (int.TryParse($"{source.Year}{source.Month.ToString().PadLeft(2, '0')}{source.Day.ToString().PadLeft(2, '0')}",
                    out int yearMonthDay))
            {
                return yearMonthDay;
            }

            return 0;
        }

        /// <summary>
        /// Calculates the number of Mondays (weeks) in the given month.
        /// </summary>
        /// <returns>The number of weeks (Mondays) in the month.</returns>
        public int xWeekCountInMonth()
        {
            // first generate all dates in the month of 'date'
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(source.Year, source.Month)).Select(n => new DateTime(source.Year, source.Month, n));
            // then filter the only the start of weeks
            var weekends = from d in dates
                where d.DayOfWeek == DayOfWeek.Monday
                select d;
            return weekends.Count();
        }

        /// <summary>
        /// Gets the start date of the week (Monday) for the specified date.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> object representing the Monday of the week.</returns>
        public DateTime xStartOfWeek()
        {
            DateTime weekStart;
            int monday = 1;
            int crtDay = (int)source.DayOfWeek;
            if (source.DayOfWeek == DayOfWeek.Sunday)
                crtDay = 7;
            int difference = crtDay - monday;
            weekStart = source.AddDays(-difference);
            return weekStart;
        }

        /// <summary>
        /// Gets the end date of the week (Sunday) for the specified date.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> object representing the Sunday of the week.</returns>
        public DateTime xEndOfWeek()
        {
            DateTime weekStart;
            int sunday = 7;
            int crtDay = (int)source.DayOfWeek;
            if (source.DayOfWeek == DayOfWeek.Sunday)
                crtDay = 7;
            int difference = sunday - crtDay;
            weekStart = source.AddDays(difference);
            return weekStart;
        }

        /// <summary>
        /// Finds the first occurrence of the specified day of the week in the month of the given date.
        /// </summary>
        /// <param name="week">The target day of the week (default is Saturday).</param>
        /// <returns>A <see cref="DateTime"/> object representing the first occurrence of the specified day of the week.</returns>
        public DateTime xDateForDayInWeek(DayOfWeek week = DayOfWeek.Saturday)
        {
            DateTime dt = new DateTime(source.Year, source.Month, 1); // 현재 월의 첫 날
            while (source.DayOfWeek != week) // 첫째 주 토요일 찾기
            {
                source = source.AddDays(1);
            }

            return source;
        }

        /// <summary>
        /// Compares whether two nullable <see cref="DateTime"/> objects have the same year.
        /// </summary>
        /// <param name="to">The second date to compare.</param>
        /// <returns>True if the years are equal; otherwise, false.</returns>
        public bool xIsYearEquals(DateTime to)
        {
            if (source.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return source.Year == to!.Year;
        }

        /// <summary>
        /// Compares whether two nullable <see cref="DateTime"/> objects have the same year and month.
        /// </summary>
        /// <param name="to">The second date to compare.</param>
        /// <returns>True if the year and month are equal; otherwise, false.</returns>
        public bool xIsMonthEquals(DateTime to)
        {
            if (source.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return source.xIsYearEquals(to) && (source.Month == to.Month);
        }
        
        /// <summary>
        /// Compares whether two nullable <see cref="DateTime"/> objects have the same year, month, and day.
        /// </summary>
        /// <param name="to">The second date to compare.</param>
        /// <returns>True if the year, month, and day are equal; otherwise, false.</returns>
        public bool xIsDayEquals(DateTime to)
        {
            if (source.xIsEmpty()) return false;
            if (to.xIsEmpty()) return false;
            return source.xIsYearEquals(to) && source.xIsMonthEquals(source) && (source.Day == to.Day);
        }

        /// <summary>
        /// Retrieves the day of the week as a string in the specified culture.
        /// </summary>
        /// <param name="culture">The culture for the day name (default is the current culture).</param>
        /// <returns>The name of the day of the week.</returns>
        public string xToDayOfWeek(string culture = null)
        {
            if (culture.xIsEmpty()) culture = CultureInfo.CurrentCulture.Name;
            return source.ToString("dddd", new CultureInfo(culture));
        }     

        // /// <summary>
        // /// Converts a Unix timestamp (in seconds) to a <see cref="DateTime"/> object.
        // /// </summary>
        // /// <param name="tsSecond">The Unix timestamp in seconds.</param>
        // /// <param name="local">Whether to return the date in local time (default is true).</param>
        // /// <returns>A <see cref="DateTime"/> object representing the timestamp.</returns>
        // public static DateTime xToDateTime(this long tsSecond, bool local = true)
        // {
        //     var offset = DateTimeOffset.FromUnixTimeSeconds(tsSecond);
        //     return local ? offset.LocalDateTime : offset.UtcDateTime;
        // }
        
        // /// <summary>
        // /// Converts a Unix timestamp (in milliseconds) to a <see cref="DateTime"/> object.
        // /// </summary>
        // /// <param name="tsMs">The Unix timestamp in milliseconds.</param>
        // /// <param name="local">Whether to return the date in local time (default is true).</param>
        // /// <returns>A <see cref="DateTime"/> object representing the timestamp.</returns>
        // public static DateTime xToDateTimeMs(this long tsMs, bool local = true)
        // {
        //     var offset = DateTimeOffset.FromUnixTimeMilliseconds(tsMs);
        //     return local ? offset.LocalDateTime : offset.UtcDateTime;
        // }
        
        // /// <summary>
        // /// Converts a <see cref="DateTime"/> object to a specified timezone.
        // /// </summary>
        // /// <param name="date">The date to convert.</param>
        // /// <param name="timezoneId">The target timezone ID (default is "Korea Standard Time").</param>
        // /// <returns>A <see cref="DateTime"/> object in the specified timezone.</returns>
        // public static DateTime xConvertDateTime(this DateTime date, string timezoneId = "Korea Standard Time")
        // {
        //     var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        //     return TimeZoneInfo.ConvertTimeFromUtc(date, timezone);
        // }           
    }
}
