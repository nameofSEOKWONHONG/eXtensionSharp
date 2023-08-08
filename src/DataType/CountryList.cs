using System.Globalization;

namespace eXtensionSharp;

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

public class CountryList {
    private CultureTypes cultureType;
    public CountryList(bool AllCultures)
    {
        cultureType = AllCultures ? CultureTypes.AllCultures : CultureTypes.SpecificCultures;
        Countries = GetAllCountries(cultureType);
    }

    public List<CountryInfo> Countries { get; set; }

    public List<CountryInfo> GetCountryInfoByName(string countryName, bool nativeName)
    {
        return nativeName ? Countries.Where(info => info.Region?.NativeName == countryName).ToList()
                          : Countries.Where(info => info.Region?.EnglishName == countryName).ToList();
    }

    public List<CountryInfo> GetCountryInfoByName(string countryName, bool nativeName, bool isNeutral)
    {
        return nativeName ? Countries.Where(info => info.Region?.NativeName == countryName &&
                                                    info.Culture?.IsNeutralCulture == isNeutral).ToList()
                          : Countries.Where(info => info.Region?.EnglishName == countryName &&
                                                    info.Culture?.IsNeutralCulture == isNeutral).ToList();
    }

    public string GetTwoLettersName(string countryName, bool nativeName)
    {
        CountryInfo country = nativeName ? Countries.FirstOrDefault(info => info.Region.NativeName == countryName)
            : Countries.FirstOrDefault(info => info.Region.EnglishName == countryName);

        return country?.Region.TwoLetterISORegionName;
    }

    public string GetCountryFullName(string twoLetter)
    {
        return Countries.First(m => m.Region.TwoLetterISORegionName == twoLetter).Region.EnglishName;
    }

    public string GetThreeLettersName(string countryName, bool nativeName)
    {
        CountryInfo country = nativeName ? Countries.FirstOrDefault(info => info.Region.NativeName == countryName)
            : Countries.FirstOrDefault(info => info.Region.EnglishName == countryName);

        return country?.Region?.ThreeLetterISORegionName;
    }

    public List<string> GetIetfLanguageTag(string countryName, bool useNativeName)
    {
        return useNativeName ? Countries.Where(info => info.Region.NativeName == countryName)
                                        .Select(info => info.Culture.IetfLanguageTag).ToList()
                             : Countries.Where(info => info.Region.EnglishName == countryName)
                                        .Select(info => info.Culture.IetfLanguageTag).ToList();
    }

    public List<int> GetRegionGeoId(string countryName, bool useNativeName)
    {
        return useNativeName ? Countries.Where(info => info.Region.NativeName == countryName)
                                        .Select(info => info.Region.GeoId).ToList()
                             : Countries.Where(info => info.Region.EnglishName == countryName)
                                        .Select(info => info.Region.GeoId).ToList();
    }

    public string GetCurrencySymbol(string isoCurrencySymbol)
    {
        return Countries.First(info => info.Region.ISOCurrencySymbol == isoCurrencySymbol).Region.CurrencySymbol;
    }

    public string GetIsoCurrencySymbolByTwoLetter(string twoLetter)
    {
        return Countries.First(info => info.Region.TwoLetterISORegionName == twoLetter).Region.ISOCurrencySymbol;
    }

    public string GetIsoCurrencySymbolByThreeLetter(string threeLetter)
    {
        return Countries.First(info => info.Region.ThreeLetterISORegionName == threeLetter).Region.ISOCurrencySymbol;
    }

    private static List<CountryInfo> GetAllCountries(CultureTypes cultureTypes)
    {
        List<CountryInfo> countries = new List<CountryInfo>();

        foreach (CultureInfo culture in CultureInfo.GetCultures(cultureTypes)) {
                if (culture.LCID != 127)
                    countries.Add(new CountryInfo() {
                        Culture = culture,
                        Region = new RegionInfo(culture.TextInfo.CultureName)
                    });
        }
        return countries;
    }
}

public class CountryInfo {
    public CultureInfo Culture { get; set; }
    public RegionInfo Region { get; set; }
}