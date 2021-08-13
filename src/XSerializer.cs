using System.Collections.Generic;
using System.Text.Json;

namespace eXtensionSharp
{
    public static class XSerializer
    {
        public static T xToEntity<T>(this string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static IEnumerable<T> xToEntities<T>(this string jsonString)
        {
            return JsonSerializer.Deserialize<IEnumerable<T>>(jsonString);
        }

        public static string xToJson<T>(this T entity, JsonSerializerOptions serializerOptions = null)
            where T : class
        {
            if (serializerOptions.xIsNotNull())
                return JsonSerializer.Serialize(entity, serializerOptions);
            return JsonSerializer.Serialize(entity);
        }

        public static string xToJson<TKey, TValue>(this XDictionaryPool<TKey, TValue> xDictionaryPool)
        {
            var dic = xDictionaryPool.ToDictionary();
            return JsonSerializer.Serialize(dic);
        }

        public static string xToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}