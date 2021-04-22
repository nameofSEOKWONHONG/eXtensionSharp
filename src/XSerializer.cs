using System.Collections.Generic;
using Newtonsoft.Json;

namespace eXtensionSharp {
    public static class XSerializer {
        public static T xFromJsonToObject<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static IEnumerable<T> xFromJsonToObjects<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }

        public static string xFromObjectToJson<T>(this T entity, Formatting? formatting = null,
            JsonSerializerSettings serializerSettings = null)
            where T : class {
            if (formatting.xIsNotNull() && serializerSettings.xIsNotNull())
                return JsonConvert.SerializeObject(entity, formatting.Value, serializerSettings);
            if (formatting.xIsNotNull() && serializerSettings.xIsNull())
                return JsonConvert.SerializeObject(entity, formatting.Value);
            if (formatting.xIsNull() && serializerSettings.xIsNotNull())
                return JsonConvert.SerializeObject(entity, serializerSettings);
            return JsonConvert.SerializeObject(entity);
        }

        public static string xFromObjectToJson<TKey, TValue>(this XDictionaryPool<TKey, TValue> xDictionaryPool) {
            var dic = xDictionaryPool.ToDictionary();
            return JsonConvert.SerializeObject(dic);
        }

        public static string xFromObjectToJson(this object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}