namespace eXtensionSharp.Functions;

public partial class Utils
{
    public static Dictionary<string, object> xEnumToDictionary<T>() where T : Enum
    {
        var keys = Enum.GetNames(typeof(T));
        var values = Enum.GetValues(typeof(T));

        Dictionary<string, object> map = new();
        for (var i = 0; i < keys.Length; i++)
        {
            var key = keys[i];
            var value = values.GetValue(i);
            map.Add(key, value);
        }

        return map;
    }
}