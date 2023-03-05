using System.Text.Json;
using System.Text.Json.Serialization;

namespace eXtensionSharp
{
    public static class XSerializeExtensions
    {
        public static T xToEntity<T>(this string jsonString, JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty()) options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.IgnoreCycles };
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        public static async Task<T> xToEntityAsync<T>(this Stream stream, JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty()) options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.IgnoreCycles };
            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }

        public static IEnumerable<T> xToEntities<T>(this string jsonString, JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty()) options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.IgnoreCycles };
            return JsonSerializer.Deserialize<IEnumerable<T>>(jsonString, options);
        }

        public static async Task<IEnumerable<T>> xToEntitiesAsync<T>(this Stream stream, JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty()) options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.IgnoreCycles };
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(stream, options);
        }

        public static string xToJson<T>(this T entity, JsonSerializerOptions serializerOptions = null)
            where T : class
        {
            if (!serializerOptions.xIsEmpty())
                return JsonSerializer.Serialize(entity, serializerOptions);
            return JsonSerializer.Serialize(entity);
        }

        public static string xToJson<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return JsonSerializer.Serialize(dictionary);
        }

        public static string xToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T xToClone<T>(this T src) where T : class
        {
            return new FastDeepCloner.FastDeepCloner(src).Clone<T>();
        }

        public static T xToClone<T>(this T src, Func<T, T> func) where T : class
        {
            var result = src.xToClone();
            result = func(result);
            return result;
        }
    }
}