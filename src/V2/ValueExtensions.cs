using System;
using System.Text.Json;

namespace eXtensionSharp;

public static class ValueExtensions
{
    extension(object source)
    {
        /// <summary>
        /// object to T value,
        /// </summary>
        /// <remarks>
        /// 10-22-24 : support json element, support value kind type is string, number, true, false, null, undefined
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = 1;
        /// var ss = s.xValue&lt;string&gt;();
        /// Console.WriteLine(ss); //output:"1";
        /// or
        /// var s = "";
        /// var ss = s.xValue&lt;string&gt;("10");
        /// Console.WriteLine(ss); //output:"10";
        /// </code>
        /// </example>
        public T xValue<T>(object @default = null)
        {
            if (source.xIsEmpty())
            {
                if (@default.xIsEmpty()) return default;
                return (T)Convert.ChangeType(@default, typeof(T))!;
            }

            if (typeof(T).IsEnum)
            {
                return (T)source;
            }

            if (source is JsonElement t)
            {
                return t.ValueKind switch
                {
                    JsonValueKind.String => t.GetString().xValue<T>(),
                    JsonValueKind.Number => t.GetDouble().xValue<T>(),
                    JsonValueKind.True => t.GetBoolean().xValue<T>(),
                    JsonValueKind.False => t.GetBoolean().xValue<T>(),
                    JsonValueKind.Null => default,
                    JsonValueKind.Undefined => default,
                    _ => throw new Exception($"Unexpected JsonValueKind {t.ValueKind}")
                };
            }

            //if (typeof(T).xIsNumber())
            //{
            //    if (typeof(T).GetType() == typeof(byte)) return (T)(object)Convert.ToByte(src);
            //    else if (typeof(T).GetType() == typeof(sbyte)) return (T)(object)Convert.ToSByte(src);
            //    else if (typeof(T).GetType() == typeof(short)) return (T)(object)Convert.ToInt16(src);
            //    else if (typeof(T).GetType() == typeof(ushort)) return (T)(object)Convert.ToUInt16(src);
            //    else if (typeof(T).GetType() == typeof(int)) return (T)(object)Convert.ToInt32(src);
            //    else if (typeof(T).GetType() == typeof(uint)) return (T)(object)Convert.ToUInt32(src);
            //    else if (typeof(T).GetType() == typeof(long)) return (T)(object)Convert.ToInt64(src);
            //    else if (typeof(T).GetType() == typeof(ulong)) return (T)(object)Convert.ToUInt64(src);
            //    else if (typeof(T).GetType() == typeof(float)) return (T)(object)Convert.ToSingle(src);
            //    else if (typeof(T).GetType() == typeof(double)) return (T)(object)Convert.ToDouble(src);
            //    else if (typeof(T).GetType() == typeof(decimal)) return (T)(object)Convert.ToDecimal(src);
            //}

            if (typeof(T) == typeof(Guid))
            {
                if (source is Guid g) return (T)(object)g;
                if (Guid.TryParse(source.ToString(), out var parsedGuid))
                    return (T)(object)parsedGuid;

                throw new InvalidCastException($"Cannot convert {source} to Guid.");
            }

            if (source.GetType() == typeof(Guid) && typeof(T) == typeof(string))
            {
                if (Guid.TryParse(source.ToString(), out var output))
                {
                    return (T)(object)output.ToString();
                }

                return (T)default;
            }
            
            if (typeof(T) == typeof(DateTime))
            {
                if (source is DateTime dt) return (T)(object)dt;
                if (DateTime.TryParse(source.ToString(), out var parsedDate))
                    return (T)(object)parsedDate;

                throw new InvalidCastException($"Cannot convert {source} to DateTime.");
            }

            if (source is DateTime time)
            {
                if (typeof(T) == typeof(int))
                    return (T)Convert.ChangeType(time.xToDateFormat("yyyyMMdd"), typeof(T));

                return (T)Convert.ChangeType(time.xToDateFormat("yyyy-MM-dd"), typeof(T));
            }

            return (T)Convert.ChangeType(source, typeof(T));
        }
        
        /// <summary>
        /// casting src to T (hard casting)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// object o = "10";
        /// var ss = o.xAs&lt;string&gt;();
        /// Console.WriteLine(ss); //output:"10";
        /// </code>
        /// </example>
        public T xAs<T>()
        {
            if(source.xIsEmpty()) return default;
            return (T)source;
        }

        /// <summary>
        /// Creates a deep copy of the source object by serializing it to JSON and then deserializing it back to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the object is to be cloned.</typeparam>
        /// <param name="src">The source object to clone.</param>
        /// <returns>A new instance of type T that is a deep copy of the source object, or default(T) if the source is considered empty.</returns>
        /// <remarks>
        /// This method checks if the source object is empty using the xIsEmpty extension method. If the source is empty, it returns the default value for type T.
        /// The cloning is performed by serializing the source object to a JSON string and then deserializing that string to the specified type.
        /// This method can throw serialization-related exceptions if the object type is not compatible with JSON serialization.
        /// Is not good code.
        /// </remarks>
        /// <example>
        /// <code>
        /// public class Person
        /// {
        ///     public string Name { get; set; }
        ///     public int Age { get; set; }
        /// }
        ///
        /// Person originalPerson = new Person { Name = "John", Age = 30 };
        /// Person clonedPerson = originalPerson.xClone();
        ///
        /// Console.WriteLine($"Original: {originalPerson.Name}, {originalPerson.Age}");
        /// Console.WriteLine($"Cloned: {clonedPerson.Name}, {clonedPerson.Age}");
        /// // Output:
        /// // Original: John, 30
        /// // Cloned: John, 30
        /// </code>
        /// </example>         
        public T xClone<T>()
        {
            if (source.xIsEmpty()) return default;
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(json);
        }


    }
}
