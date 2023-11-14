namespace eXtensionSharp;

public static class XGeoExtensions
{
    public static bool xValidateLatitude(this double latitude)
    {
        if (latitude.xIsEmpty()) return false;
            
        if (latitude is < -90 or > 90)
        {
            return false;
        }
        return true;
    }

    public static bool xValidateLongitude(this double longitude)
    {
        if (longitude.xIsEmpty()) return false;
            
        if (longitude is < -180 or > 180)
        {
            return false;
        }
        return true;
    }
}