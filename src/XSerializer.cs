using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace eXtensionSharp {
    public static class XSerializer {
        public static T xJsonToObject<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static IEnumerable<T> xJsonToObjects<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }

        public static string xObjectToJson<T>(this T entity, Formatting? formatting = null,
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

        public static string xObjectToJson<TKey, TValue>(this XDictionaryPool<TKey, TValue> xDictionaryPool) {
            var dic = xDictionaryPool.ToDictionary();
            return JsonConvert.SerializeObject(dic);
        }

        public static string xObjectToJson(this object obj) {
            return JsonConvert.SerializeObject(obj);
        }

        public static BsonDocument xObjectToBson<T>(this T obj) {
            if (obj.xIsNull()) return null;
            return BsonDocument.Parse(JsonConvert.SerializeObject(obj));
        }

        public static T xBsonToObject<T>(this BsonDocument doc) {
            return BsonSerializer.Deserialize<T>(doc);
        }

        public static BsonDocument xFromObjectToBson<T>(this T obj) {
            return BsonDocument.Parse(obj.xObjectToJson());
        }

        public static T xFromBsonToObject<T>(this BsonDocument doc) {
            return BsonSerializer.Deserialize<T>(doc);

        }
    }
}