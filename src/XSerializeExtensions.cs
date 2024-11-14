using System.Text.Json;
using System.Text.Json.Serialization;

namespace eXtensionSharp
{
    public static class XSerializeExtensions
    {
        public static T xDeserialize<T>(this string jsonString, JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty())
            {
                options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true, 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                };
            }
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
        
        public static string xSerialize<T>(this T entity, JsonSerializerOptions options = null)
            where T : class
        {
            if (options.xIsEmpty())
            {
                options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true, 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                };
            }
            
            return JsonSerializer.Serialize(entity, options);
        }        
    }

    // internal class ObjectIdConverter : JsonConverter<ObjectId>
    // {
    //     public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    //     {
    //         writer.WriteStringValue(value.ToString());
    //     }
    //
    //     public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //     {
    //         var stringValue = reader.GetString();
    //         if (ObjectId.TryParse(stringValue, out var objectId))
    //         {
    //             return objectId;
    //         }
    //         throw new JsonException($"Unable to convert \"{stringValue}\" to ObjectId.");
    //     }
    //
    //     public override bool CanConvert(Type typeToConvert)
    //     {
    //         return typeof(ObjectId).IsAssignableFrom(typeToConvert);
    //     }
    // }
}