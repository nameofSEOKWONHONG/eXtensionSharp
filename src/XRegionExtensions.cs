using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace eXtensionSharp
{
    public static class XRegionExtensions
    {
        public static IEnumerable<RegionInfo> xGetAllRegionInfos(string[] nationCodes = null)
        {
            List<CultureInfo> cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures).ToList();
            if (nationCodes.xIsNotEmpty())
            {
                cinfo.Clear();
                nationCodes.xForEach(item =>
                {
                    cinfo.Add(new CultureInfo(item));
                });
            }

            var regionInfos = new List<RegionInfo>();

            foreach (CultureInfo cul in cinfo)
            {
                char[] name = cul.Name.ToCharArray();
                if (name.Length >= 2)
                {
                    var region = new RegionInfo(cul.Name);
                    regionInfos.Add(region);
                }
            }
            return regionInfos.OrderBy(m => m.DisplayName);
        }

        public static RegionInfo xGetRegionInfo(this string regionCode)
        {
            return new RegionInfo(regionCode);
        }
    }

    /*
    This code produces the following output.

       Name:                         US
       DisplayName:                  United States
       EnglishName:                  United States
       IsMetric:                     False
       ThreeLetterISORegionName:     USA
       ThreeLetterWindowsRegionName: USA
       TwoLetterISORegionName:       US
       CurrencySymbol:               $
       ISOCurrencySymbol:            USD

    The two RegionInfo instances are equal.

    */
}