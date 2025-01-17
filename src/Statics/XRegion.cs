using System.Globalization;

namespace eXtensionSharp
{
    public class XRegion
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
    }
}