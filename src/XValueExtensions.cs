using System.Text.Json;
using eXtensionSharp;

namespace eXtensionSharp
{
    public static class XValueExtensions
    {
        /// <summary>
        /// object to T value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var s = 1;
        /// var ss = s.xValue<string>();
        /// Console.WriteLine(ss); //output:"1";
        /// or
        /// var s = "";
        /// var ss = s.xValue<string>("10");
        /// Console.WriteLine(ss); //output:"10";
        /// </code>
        /// </example>
        public static T xValue<T>(this object src, object @default = null)
        {
            if (src.xIsEmpty())
            {
                if (@default.xIsEmpty()) return default;
                return (T)Convert.ChangeType(@default, typeof(T))!;
            }

            if (typeof(T).IsEnum)
            {
                return (T)src;
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

            if (src is Guid guid) return (T)Convert.ChangeType(guid.ToString(), typeof(T));

            if (src is DateTime time)
            {
                if (typeof(T) == typeof(int))
                    return (T)Convert.ChangeType(time.xToDate(ENUM_DATE_FORMAT.YYYYMMDD), typeof(T));

                return (T)Convert.ChangeType(time.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD), typeof(T));
            }

            return (T)Convert.ChangeType(src, typeof(T));
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
        /// var ss = o.xAs<string>();
        /// Console.WriteLine(ss); //output:"10";
        /// </code>
        /// </example>
        public static T xAs<T>(this object src)
        {
            if(src.xIsEmpty()) return default;
            return (T)src;
        }

        /// <summary>
        /// casting src to T (safe casting)
        /// </summary>
        /// <param name="src"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T xAsSafe<T>(this object src)
        {
            if(src.xIsEmpty()) return default;
            if (src is T dest)
            {
                return dest;
            }

            return default;
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
        public static T xClone<T>(this T src)
        {
            if (src.xIsEmpty()) return default;
            var json = JsonSerializer.Serialize(src);
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Compares two strings for equality, ignoring case sensitivity.
        /// </summary>
        /// <param name="src">The source string to compare.</param>
        /// <param name="dest">The destination string to compare against.</param>
        /// <returns>True if the strings are equal ignoring case; otherwise, false.</returns>
        /// <example>
        /// <code>
        /// string name1 = "hello";
        /// string name2 = "HELLO";
        /// bool areEqual = name1.xEquals(name2); // returns true
        /// </code>
        /// </example>
        public static bool xEquals(this string src, string dest)
        {
            return src.Equals(dest, StringComparison.OrdinalIgnoreCase);
        }
    }
}