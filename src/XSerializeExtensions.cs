using System.Text.Json;
using System.Text.Json.Serialization;

namespace eXtensionSharp
{
    public static class XSerializeExtensions
    {
        public static T xDeserialize<T>(this string jsonString, JsonSerializerOptions options = null)
        {
            if (options.xIsNotEmpty())
            {
                return JsonSerializer.Deserialize<T>(jsonString, options);
            }
            
            options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };            
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
        
        public static string xSerialize<T>(this T entity, JsonSerializerOptions options = null)
            where T : class
        {
            if (options.xIsNotEmpty())
            {
                return JsonSerializer.Serialize(entity, options);
            }
            
            options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true, 
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };            
            return JsonSerializer.Serialize(entity, options);
        }        
    }
}