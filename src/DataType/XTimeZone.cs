namespace eXtensionSharp;

public class XTimeZone
{
    private static Lazy<XTimeZone> _instance = new Lazy<XTimeZone>(() => new XTimeZone());
    public static XTimeZone Instance => _instance.Value;
    
    private List<TimeZonSet> _timeZonSets;

    public XTimeZone()
    {
        _timeZonSets = new List<TimeZonSet>()
        {
            new TimeZonSet()
            {
                CultureCode = "ko-KR",
                TimeZones = new List<TimeZone>()
                {
                    new TimeZone()
                    {
                        LongitudeFrom = 124,
                        LongitudeTo = 131,                        
                        LatitudeFrom = 33,
                        LatitudeTo = 39,
                        TimeZoneName ="Korea Standard Time"
                    }
                }
            },
            new TimeZonSet()
            {
                CultureCode = "en-US",
                TimeZones = new List<TimeZone>()
                {
                    new TimeZone()
                    {
                        LongitudeFrom = 67,
                        LongitudeTo = 82.5,                        
                        LatitudeFrom = 24,
                        LatitudeTo = 50,
                        TimeZoneName ="Eastern Standard Time" 
                    },
                    new TimeZone()
                    {
                        LongitudeFrom = 82.5,
                        LongitudeTo = 97.5,                        
                        LatitudeFrom = 25,
                        LatitudeTo = 49,

                        TimeZoneName ="Central Standard Time",
                    },
                    new TimeZone()
                    {
                        LongitudeFrom = 97.5,
                        LongitudeTo = 112.5,                        
                        LatitudeFrom = 31,
                        LatitudeTo = 49,

                        TimeZoneName ="Mountain Standard Time",
                    },
                    new TimeZone()
                    {
                        LongitudeFrom = 112.5,
                        LongitudeTo = 125,                        
                        LatitudeFrom = 32,
                        LatitudeTo = 49,

                        TimeZoneName ="Pacific Standard Time",
                    },
                    new TimeZone()
                    {
                        LongitudeFrom = 125,
                        LongitudeTo = 172.5,                        
                        LatitudeFrom = 51,
                        LatitudeTo = 71,

                        TimeZoneName ="Alaska Standard Time",
                    },
                    new TimeZone()
                    {
                        LongitudeFrom = 154,
                        LongitudeTo = 172.5,                        
                        LatitudeFrom = 18,
                        LatitudeTo = 30,

                        TimeZoneName ="Hawaii-Aleutian Standard Time",
                    },
                    new TimeZone()
                    {
                        LongitudeFrom = 60,
                        LongitudeTo = 67,                        
                        LatitudeFrom = 18,
                        LatitudeTo = 47,

                        TimeZoneName ="Atlantic Standard Time",
                    },                    
                },
            }
        };
    }

    /// <summary>
    /// get timezone name by culture code
    /// ** caution : multi-time zone code returns a "," separator.
    /// </summary>
    /// <param name="cultureCode"></param>
    /// <returns></returns>
    public string GetTimeZone(string cultureCode)
    {
        if (cultureCode.xIsEmpty()) return string.Empty;
        
        var selected = _timeZonSets.FirstOrDefault(m => m.CultureCode == cultureCode);
        if (selected.xIsNotEmpty())
        {
            if (selected.TimeZones.Count <= 1)
            {
                return selected.TimeZones.First().TimeZoneName;
            }
            else
            {
                return selected.TimeZones.Select(m => m.TimeZoneName).xJoin();
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// get timezone name by culture code
    /// </summary>
    /// <param name="cultureCode"></param>
    /// <returns></returns>
    public string[] GetTimeZones(string cultureCode)
    {
        if (cultureCode.xIsEmpty()) return Array.Empty<string>();
        
        var selected = _timeZonSets.FirstOrDefault(m => m.CultureCode == cultureCode);
        if (selected.xIsNotEmpty())
        {
            if (selected.TimeZones.Count <= 1)
            {
                return new[] { selected.TimeZones.First().TimeZoneName };
            }
            else
            {
                return selected.TimeZones.Select(m => m.TimeZoneName).ToArray();
            }
        }

        return Array.Empty<string>();
    }

    /// <summary>
    /// get timezone name by geo coordinate
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    public string GetTimeZone(double latitude, double longitude)
    {
        var selected = _timeZonSets.Select(timeZonSet => timeZonSet.TimeZones
                .FirstOrDefault(m => (m.LatitudeFrom <= latitude && m.LatitudeTo >= latitude) &&
                                     (m.LongitudeFrom <= longitude && m.LongitudeTo >= longitude)))
            .FirstOrDefault();

        return selected?.TimeZoneName;
    }
}

internal class TimeZonSet
{
    public string CultureCode { get; set; }
    public List<TimeZone> TimeZones { get; set; }
}

internal class TimeZone
{
    public double LatitudeFrom { get; set; }
    public double LatitudeTo { get; set; }
    public double LongitudeFrom { get; set; }
    public double LongitudeTo { get; set; }
    public string TimeZoneName { get; set; }
}