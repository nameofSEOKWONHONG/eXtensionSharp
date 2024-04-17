namespace eXtensionSharp;

public static class XCultureExtensions
{
    public static string xToLanguageDisplayName(this string cultureName)
    {
        if (cultureName.xIsEmpty()) return string.Empty;
        
        var culture = new System.Globalization.CultureInfo(cultureName);
        return culture.DisplayName;
    }

    public static string xToLanguageEngName(this string cultureName)
    {
        if (cultureName.xIsEmpty()) return string.Empty;
        
        var culture = new System.Globalization.CultureInfo(cultureName);
        return culture.EnglishName;
    }
}