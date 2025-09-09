using System.Text.Json;
using eXtensionSharp;

namespace eXtensionSharp
{
    public static class XValueExtensions
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
        public static T xValue<T>(this object src, object @default = null, ConvertOptions options = null)
        {
            // default도 한 번만 변환 시도(분기 최소화)
            T defV = default;
            if (@default is not null && FastConvert.TryChangeType(@default, out T tmp, options)) defV = tmp;

            return FastConvert.ChangeType(src, defV, options);
        }

        /// <summary>
        /// Converts a string value to a specified type based on the provided type name.
        /// </summary>
        /// <param name="src">The source string value to be converted.</param>
        /// <param name="typeName">The name of the type to which the source value should be converted (e.g., "String", "Int32", "Boolean").</param>
        /// <returns>An object representing the converted value of the specified type.</returns>
        public static object xValueWithTypeName(this string src, string typeName)
        {
            return typeName switch
            {
                nameof(String) => src.xValue<string>(),
                nameof(Int32) => src.xValue<int>(),
                nameof(Int64) => src.xValue<long>(),
                nameof(Double) => src.xValue<double>(),
                nameof(Decimal) => src.xValue<decimal>(),
                nameof(Boolean) => src.xValue<bool>(),
                nameof(DateTime) => src.xValue<DateTime>(),
                nameof(DateTimeOffset) => src.xValue<DateTimeOffset>(),
                nameof(TimeSpan) => src.xValue<TimeSpan>(),
                nameof(Guid) => src.xValue<Guid>(),
                nameof(Byte) => src.xValue<byte>(),
            };
        }

        
        private static T xValueWithJsonElement<T>(this JsonElement element)
        {
            var v = element.ValueKind switch
            {
                JsonValueKind.String => element.GetString().xValue<T>(),
                JsonValueKind.Number => element.GetDouble().xValue<T>(),
                JsonValueKind.True => element.GetBoolean().xValue<T>(),
                JsonValueKind.False => element.GetBoolean().xValue<T>(),
                JsonValueKind.Null => default,
                JsonValueKind.Undefined => default,
                _ => throw new Exception($"Unexpected JsonValueKind {element.ValueKind}")
            };

            return v;
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
        public static T xAs<T>(this object src)
        {
            if(src.xIsEmpty()) return default;
            return (T)src;
        }

        /// <summary>
        /// casting src to T (safe casting)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// object o = "10";
        /// var ss = o.xAsSafe&lt;string&gt;();
        /// </code>
        /// </example>
        public static T xAsSafe<T>(this object src)
        {
            if(src.xIsEmpty()) return default;
            return src is T result ? result : default;
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
        /// bool areEqual = string.Equals(name1, name2, StringComparison.OrdinalIgnoreCase); // returns true
        /// </code>
        /// </example>
        public static bool xEquals(this string src, string dest)
        {
            return string.Equals(src, dest, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// get true, false of duplicate list
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool xIsDuplicate<T>(this IEnumerable<T> items)
        {
            if (items.xIsEmpty()) return false;

            HashSet<T> set = new();

            foreach (var item in items)
            {
                if (!set.Add(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// get true, false of duplicate list, and out duplicate item
        /// </summary>
        /// <param name="items"></param>
        /// <param name="duplicateItem"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool xTryDuplicate<T>(this IEnumerable<T> items, out T duplicateItem)
        {
            duplicateItem = default;

            if (items.xIsEmpty()) return false;

            HashSet<T> set = new();

            foreach (var item in items)
            {
                if (!set.Add(item))
                {
                    duplicateItem = item;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves an element from the specified index of a read-only list.
        /// If the index is out of range or the list is empty, the default value of the element type is returned.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="items">The read-only list from which to retrieve the element.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>The element at the specified index, or the default value of the type if the index is invalid or the list is empty.</returns>
        public static T xValueOfArray<T>(this IReadOnlyList<T> items, int index)
        {
            if (items.xIsEmpty()) return default;
            if (index < 0) return default;
            if (index >= items.Count) return default;
            return items[index];
        }
    }
}