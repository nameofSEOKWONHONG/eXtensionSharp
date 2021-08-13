using System;

namespace eXtensionSharp
{
    public static class XDateHelper
    {
        public static DateTime xToDate(this string date)
        {
            var datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        public static string xToDate(this DateTime date, ENUM_DATE_FORMAT format = null)
        {
            if (format.xIsNotNull()) date.ToString(format.ToString());
            return date.ToString(ENUM_DATE_FORMAT.DEFAULT.ToString());
        }

        public static string xToDate(this DateTime date, string format = null)
        {
            if (format.xIsNullOrEmpty()) format = "yyyy-MM-dd";
            return date.ToString(format);
        }
    }

    public class ENUM_DATE_FORMAT : XEnumBase<ENUM_DATE_FORMAT>
    {
        public static readonly ENUM_DATE_FORMAT DEFAULT = Define("yyyy-MM-dd");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD = Define("yyyy-MM-dd");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS = Define("yyyy-MM-dd HH:mm:ss");
        public static readonly ENUM_DATE_FORMAT YYYYMMDD = Define("yyyyMMdd");
        public static readonly ENUM_DATE_FORMAT YYYY_FS_MM_FS_DD = Define("yyyy/MM/dd");
        public static readonly ENUM_DATE_FORMAT YYYYMMDDHHMMSS = Define("yyyyMMddHHmmss");
        public static readonly ENUM_DATE_FORMAT HHMMSS = Define("HHmmss");
    }
}