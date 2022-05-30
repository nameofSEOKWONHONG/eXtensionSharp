using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;

namespace eXtensionSharp
{
    public static class XSerializeExtensions
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

        //TODO:NOT TEST
        public static T xToClone<T>(this T obj) where T : class
        {
            var target = JsonSerializer.Serialize(obj);
            var result = JsonSerializer.Deserialize<T>(target);
            return result;
        }
        
        //TODO:NOT TEST
        public static T xToClone<T>(this T obj, Func<T, T> func) where T : class
        {
            var result = obj.xToClone();
            result = func(result);
            return result;
        }
    }
}