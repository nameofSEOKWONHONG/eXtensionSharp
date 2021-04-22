using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eXtensionSharp {
    /// <summary>
    /// String Enum Implement
    /// ref : [stackoverflow] https://stackoverflow.com/questions/8588384/how-to-define-an-enum-with-string-value
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public abstract class XENUM_BASE<TEnum> : IEquatable<TEnum>
        where TEnum : XENUM_BASE<TEnum>, new() {
        public string Value { get; private set; }

        //protected JENUM_BASE(string value) => this.Value = value;
        
        public static TEnum define(string value) {
            TEnum @enum = new TEnum();
            @enum.Value = value;
            return @enum;
        }

        #region [override]
        public override string ToString() => this.Value;

        public bool Equals(TEnum other) {
            if (other == null) return false;
            return this.Value == other?.Value;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is TEnum other) return this.Equals(other);
            return false;
        }

        public override int GetHashCode() => this.Value.GetHashCode();
        #endregion

        #region [operator]
        public static bool operator ==(XENUM_BASE<TEnum> a, XENUM_BASE<TEnum> b) => a?.Equals(b) ?? false;

        public static bool operator !=(XENUM_BASE<TEnum> a, XENUM_BASE<TEnum> b) => !(a?.Equals(b) ?? false);
        
        public static explicit operator string(XENUM_BASE<TEnum> source)
        {
            return source.Value;
        }
        public static implicit operator XENUM_BASE<TEnum>(string source)
        {
            return XENUM_BASE<TEnum>.Parse(source);
        }
        #endregion

        #region [util]
        public static XList<TEnum> AsList() {
            return (XList<TEnum>) typeof(TEnum)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(TEnum))
                .Select(p => (TEnum) p.GetValue(null))
                .xToList();
        }

        public static TEnum Parse(string value) {
            XList<TEnum> all = AsList();

            if (!all.Any(a => a.Value == value))
                throw new InvalidOperationException($"\"{value}\" is not a valid value for the type {typeof(TEnum).Name}");

            return all.Single(a => a.Value == value);
        }
        #endregion

        #region [json converter]
        public class JsonConverter<T> : Newtonsoft.Json.JsonConverter
            where T : XENUM_BASE<T>, new() {
            public override bool CanRead => true;

            public override bool CanWrite => true;

            public override bool CanConvert(Type objectType) => ImplementsGeneric(objectType, typeof(XENUM_BASE<>));

            private static bool ImplementsGeneric(Type type, Type generic) {
                while (type != null) {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
                        return true;

                    type = type.BaseType;
                }

                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer) {
                JToken item = JToken.Load(reader);
                string value = item.Value<string>();
                return XENUM_BASE<T>.Parse(value);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                if (value is XENUM_BASE<T> v)
                    JToken.FromObject(v.Value).WriteTo(writer);
            }
        }
        #endregion
    }
}