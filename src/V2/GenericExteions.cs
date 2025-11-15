using System;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eXtensionSharp;

public static class GenericExteions
{
    extension<T>(T source)
    {
        /// <summary>
        /// Gets the value of a specified property of an object.
        /// </summary>
        /// <param name="obj">The object whose property value is to be retrieved.</param>
        /// <param name="propertyName">The name of the property to get the value of.</param>
        /// <returns>The value of the property, or null if the property does not exist.</returns>
        public T2 xGetPropertyValue<T2>(string propertyName)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName);
            var v = propertyInfo?.GetValue(source);
            return v.xValue<T2>();
        }


        /// <summary>
        /// Sets the value of a specified property of an object.
        /// </summary>
        /// <param name="obj">The object whose property value is to be set.</param>
        /// <param name="propertyName">The name of the property to set the value of.</param>
        /// <param name="value">The value to set the property to.</param>
        public void xSetPropertyValue(string propertyName, object value)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(source, value);
        }

        public bool xIsNull()
        {
            return source is null;
        }

        public bool xIsNotNull()
        {
            return source is not null;
        }

        /// <summary>
        /// xIsEmpty does not support number type.<br/>
        /// xIsEmpty does not support DateTime type.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool xIsEmpty()
        {
            if (source.xIsNull())
            {
                return true;
            }

            if (source is string v)
            {
                return string.IsNullOrWhiteSpace(v);
            }
            
            switch (source)
            {
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return true;

                default: return false;
            }
        }

        public bool xIsNotEmpty()
        {
            return !source.xIsEmpty();
        }

        public bool xIsSame(T compare)
        {
            if (source.xIsEmpty()) return false;
            if (compare.xIsEmpty()) return false;
            return source.Equals(compare);
        }

        public bool xIsNotSame(T compare)
        {
            return !source.xIsSame(compare);
        }

        public void xIf(Expression<Func<T, bool>> compareExpression, Action<T> match, Action<T> notMatch = null)
        {
            var @case = compareExpression.Compile().Invoke(source);
            if(@case) match?.Invoke(source);
            else notMatch?.Invoke(source);
        }

        public string xSerialize(JsonSerializerOptions options = null)
        {
            if (options.xIsEmpty())
            {
                options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true, 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                };
            }
            
            return JsonSerializer.Serialize(source, options);
        }

        /// <summary>
        /// Converts an object to a dictionary where the keys are property names and the values are the property values.
        /// </summary>        
        /// <returns>A dictionary representing the object.</returns>
        public DynamicDictionary<object> xToDictionary()
        {
            if(source.xIsEmpty()) return default;
            if(!source.GetType().IsClass) return default;

            var result = new DynamicDictionary<object>();
            var props = source.GetType().GetProperties();
            foreach (var prop in props)
            {
                result.Add(prop.Name, prop.GetValue(source, null));
            }
            return result;
        } 
   }
}

