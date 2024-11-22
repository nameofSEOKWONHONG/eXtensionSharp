using System.Collections;
using System.Collections.Immutable;
using System.Data;
using System.Dynamic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace eXtensionSharp
{
    public static class XCollectionExtensions
    {
        /// <summary>
        /// Copies the property values from one object to another. Only properties that have both a getter and a setter are copied.
        /// </summary>
        /// <typeparam name="T1">The type of the source and destination objects.</typeparam>
        /// <param name="src">The source object from which to copy the values.</param>
        /// <param name="dest">The destination object to which the values are copied.</param>
        public static void xValueChange<T1>(this T1 src, T1 dest)
        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.CanWrite.xIsFalse()) return true;
                var v = GetPropertyValue(src, item.Name);
                SetPropertyValue(dest, item.Name, v);

                return true;
            });
        }

        /// <summary>
        /// Gets the value of a specified property of an object.
        /// </summary>
        /// <param name="obj">The object whose property value is to be retrieved.</param>
        /// <param name="propertyName">The name of the property to get the value of.</param>
        /// <returns>The value of the property, or null if the property does not exist.</returns>
        private static object GetPropertyValue(object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo?.GetValue(obj);
        }

        /// <summary>
        /// Sets the value of a specified property of an object.
        /// </summary>
        /// <param name="obj">The object whose property value is to be set.</param>
        /// <param name="propertyName">The name of the property to set the value of.</param>
        /// <param name="value">The value to set the property to.</param>
        private static void SetPropertyValue(object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(obj, value);
        }
        
        /// <summary>
        /// Maps the properties of one object to another, optionally excluding certain properties from being mapped.
        /// </summary>
        /// <typeparam name="T1">The type of the source object.</typeparam>
        /// <param name="src">The source object whose properties will be mapped.</param>
        /// <param name="dest">The destination object to which the properties will be mapped.</param>
        /// <param name="notMappingNames">An array of property names to exclude from mapping.</param>
        public static void xMapping<T1>(this T1 src, T1 dest, string[] notMappingNames = null)

        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.MemberType == MemberTypes.Property)
                {
                    if (IsTypeMatch(item.PropertyType.ToString()))
                    {
                        if (notMappingNames.xIsNotEmpty())
                        {
                            if(item.Name.xContains(notMappingNames)) return true;
                        }
                        var v = GetPropertyValue(src, item.Name);
                        SetPropertyValue(dest, item.Name, v);           
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// Maps the properties of one object to another of a different type, optionally excluding certain properties from being mapped.
        /// </summary>
        /// <typeparam name="T1">The type of the source object.</typeparam>
        /// <typeparam name="T2">The type of the destination object.</typeparam>
        /// <param name="src">The source object whose properties will be mapped.</param>
        /// <param name="dest">The destination object to which the properties will be mapped.</param>
        /// <param name="notMappingNames">An array of property names to exclude from mapping.</param>
        public static void xMapping<T1, T2>(this T1 src, T2 dest, string[] notMappingNames = null)

        {
            var props = src.xGetProperties();
            props.xForEach(item =>
            {
                if (item.MemberType == MemberTypes.Property)
                {
                    if (IsTypeMatch(item.PropertyType.ToString()))
                    {
                        if (notMappingNames.xIsNotEmpty())
                        {
                            if(item.Name.xContains(notMappingNames)) return true;
                        }
                        
                        var v = GetPropertyValue(src, item.Name);
                        var exist = dest.xGetProperties().Where(m => m.Name == item.Name);
                        if (exist.Any())
                        {
                            SetPropertyValue(dest, item.Name, v);    
                        }       
                    }
                }
                return true;
            });
        } 

        /// <summary>
        /// Checks if a property type matches one of the predefined types (e.g., int, string, DateTime, etc.).
        /// </summary>
        /// <param name="propertyTypeName">The name of the property type to check.</param>
        /// <returns>True if the property type matches one of the predefined types, otherwise false.</returns>
        private static bool IsTypeMatch(string propertyTypeName)
        {
            return propertyTypeName.xContains(new string[]
            {
                DataTypeName.Int16,
                DataTypeName.Int32,
                DataTypeName.Int64,
                DataTypeName.Double,
                DataTypeName.String,
                DataTypeName.Float,
                DataTypeName.Decimal,
                DataTypeName.DateTime,
                DataTypeName.Byte,
                DataTypeName.SByte,
                DataTypeName.Char,
                DataTypeName.UInt, 
                DataTypeName.IntPtr,
                DataTypeName.UIntPtr,
                DataTypeName.Long,
                DataTypeName.ULong,
                DataTypeName.Short,
                DataTypeName.UShort,
                
                DataTypeName.NullableInt16,
                DataTypeName.NullableInt32,
                DataTypeName.NullableInt64,
                DataTypeName.NullableDouble,
                DataTypeName.NullableFloat,
                DataTypeName.NullableDecimal,
                DataTypeName.NullableDateTime,
                DataTypeName.NullableByte,
                DataTypeName.NullableSByte,
                DataTypeName.NullableChar,
                DataTypeName.NullableUInt, 
                DataTypeName.NullableIntPtr,
                DataTypeName.NullableUIntPtr,
                DataTypeName.NullableLong,
                DataTypeName.NullableULong,
                DataTypeName.NullableShort,
                DataTypeName.NullableUShort,
                DataTypeName.NullableDateTime
            });
        }

        /// <summary>
        /// Converts an object to a dictionary where the keys are property names and the values are the property values.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert to a dictionary.</typeparam>
        /// <param name="value">The object to convert to a dictionary.</param>
        /// <returns>A dictionary representing the object.</returns>
        public static DynamicDictionary<object> xToDictionary<T>(this T value) where T : class
        {
            var result = new DynamicDictionary<object>();
            var props = value.GetType().GetProperties();
            foreach (var prop in props)
            {
                result.Add(prop.Name, prop.GetValue(value, null));
            }
            return result;
        }

        /// <summary>
        /// Converts an IEnumerable collection of objects to a collection of dictionaries, where each dictionary represents an object.
        /// </summary>
        /// <typeparam name="T">The type of objects in the collection.</typeparam>
        /// <param name="values">The collection of objects to convert to dictionaries.</param>
        /// <returns>A collection of dictionaries representing the objects.</returns>
        public static IEnumerable<DynamicDictionary<object>> xToDictionaries<T>(this IEnumerable<T> values) where T : class
        {
            if(typeof(T) == typeof(ExpandoObject)) 
            {
                return values.Select(m => new DynamicDictionary<object>((IDictionary<string, object>)m));
            }

            var list = new List<DynamicDictionary<object>>();
            foreach (var value in values)
            {
                var item = value.xToDictionary<T>();
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Converts a DataTable to a collection of dictionaries where each dictionary represents a row in the DataTable.
        /// </summary>
        /// <param name="dataTable">The DataTable to convert to dictionaries.</param>
        /// <returns>A collection of dictionaries representing the rows of the DataTable.</returns>
        public static IEnumerable<IDictionary<string, object>> xDataTableToDictionaries(this DataTable dataTable)
        {
            if (dataTable.xIsEmpty()) return new List<IDictionary<string, object>>();
            return dataTable.AsEnumerable().Select(row =>
                dataTable.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => row[column])
            ).ToList();
        }
        
        /// <summary>
        /// Counts the number of elements in a collection, optionally using a predicate to filter the elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to count the elements of.</param>
        /// <param name="predicate">A function to test each element, or null to count all elements.</param>
        /// <returns>The number of elements in the collection, or the number of elements matching the predicate.</returns>
        public static int xCount<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (enumerable.xIsEmpty()) return 0;
            if (predicate.xIsNotEmpty()) return enumerable.Count(predicate);
            return enumerable.Count();
        }

        /// <summary>
        /// Filters the elements of a collection based on a predicate function.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to filter.</param>
        /// <param name="predicate">A function to test each element.</param>
        /// <returns>A filtered collection of elements that match the predicate.</returns>
        public static IEnumerable<T> xWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) where T : class
        {
            return CheckEnumerable(enumerable).Where(predicate);
        }

        /// <summary>
        /// Projects each element of a collection to a new form using a provided transformation function.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the collection.</typeparam>
        /// <typeparam name="T2">The type of the elements in the resulting collection.</typeparam>
        /// <param name="enumerable">The collection to transform.</param>
        /// <param name="predicate">A function to transform each element.</param>
        /// <returns>A collection of transformed elements.</returns>
        public static IEnumerable<T2> xSelect<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> predicate) where T1 : class
        {
            return CheckEnumerable(enumerable).Select(predicate);
        }

        private static IEnumerable<T> CheckEnumerable<T>(IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty()) return Enumerable.Empty<T>();
            return enumerable;
        }

        /// <summary>
        /// Checks if a string contains a specific substring.
        /// </summary>
        /// <param name="src">The string to search within.</param>
        /// <param name="compare">The substring to search for.</param>
        /// <returns>True if the string contains the substring, otherwise false.</returns>
        public static bool xContains(this string src, string compare)
        {
            if (src.xIsEmpty()) return false;
            return src.Contains(compare);
        }

        /// <summary>
        /// Checks if a string contains any of the substrings in a list.
        /// </summary>
        /// <param name="src">The string to search within.</param>
        /// <param name="compares">The list of substrings to search for.</param>
        /// <returns>True if the string contains any of the substrings, otherwise false.</returns>
        public static bool xContains(this string src, string[] compares)
        {
            if (src.xIsEmpty()) return false;
            return compares.FirstOrDefault(src.Contains).xIsNotEmpty();
        }

        /// <summary>
        /// Checks if a value is contained within a collection.
        /// </summary>
        /// <typeparam name="T">The type of value and collection elements.</typeparam>
        /// <param name="src">The value to search for.</param>
        /// <param name="compares">The collection of values to search within.</param>
        /// <returns>True if the value is contained within the collection, otherwise false.</returns>
        public static bool xContains<T>(this T src, IEnumerable<T> compares)
        {
            if (src.xIsEmpty()) return false;
            return compares.Contains(src);
        }

        public static bool xContains<T>(this IEnumerable<T> src, IEnumerable<T> compares)
        {
            if (src.xIsEmpty()) return false;
            return src.Where(m => m.xContains(compares)).xIsNotEmpty();
        }

        /// <summary>
        /// Checks if any elements in a collection satisfy a given condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="src">The collection to check.</param>
        /// <returns>True if any elements satisfy the condition, otherwise false.</returns>
        public static bool xAny<T>(this IEnumerable<T> src) 
        {
            if(src.xIsEmpty()) return false;
            return src.Any();
        }
        
        /// <summary>
        /// Retrieves the first element from a collection that matches a given condition, or the first element if no condition is specified.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to retrieve the element from.</param>
        /// <param name="predicate">A function to test each element, or null to retrieve the first element.</param>
        /// <returns>The first element that matches the condition, or the first element in the collection if no condition is provided.</returns>
        public static T xFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotEmpty()) return enumerable.FirstOrDefault(predicate);
            return enumerable.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the last element from a collection that matches a given condition, or the last element if no condition is specified.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to retrieve the element from.</param>
        /// <param name="predicate">A function to test each element, or null to retrieve the last element.</param>
        /// <returns>The last element that matches the condition, or the last element in the collection if no condition is provided.</returns>
        public static T xLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotEmpty()) return enumerable.LastOrDefault(predicate);
            return enumerable.LastOrDefault();
        }

        /// <summary>
        /// Converts a collection to a list, returning an empty list if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to convert to a list.</param>
        /// <returns>A list containing the elements from the collection, or an empty list if the collection is empty.</returns>
        public static List<T> xToList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.xIsEmpty() ? new List<T>() : new List<T>(enumerable);
        }

        /// <summary>
        /// Converts a collection to an array, returning an empty array if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to convert to an array.</param>
        /// <returns>An array containing the elements from the collection, or an empty array if the collection is empty.</returns>
        public static T[] xToArray<T>(this IEnumerable<T> enumerable) where T : new()
        {
            if (enumerable.xIsEmpty()) return new T[0];
            return enumerable.ToArray();
        }

        /// <summary>
        /// Checks if a value is between two other values, inclusive.
        /// </summary>
        /// <typeparam name="T">The type of the values, which must implement INumber.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="from">The lower bound value.</param>
        /// <param name="to">The upper bound value.</param>
        /// <returns>True if the value is between the two bounds, otherwise false.</returns>
        public static bool xIsBetween<T>(this T value, T from, T to) where T : INumber<T>
        {
            if (from > to) throw new Exception("from value is greater than to value.");
            if (value >= from && value <= to) return true;
            return false;
        }
        
        /// <summary>
        /// Checks if a DateTime value is between two other DateTime values, inclusive.
        /// </summary>
        /// <param name="value">The DateTime value to check.</param>
        /// <param name="from">The lower bound DateTime.</param>
        /// <param name="to">The upper bound DateTime.</param>
        /// <returns>True if the DateTime value is between the two bounds, otherwise false.</returns>
        public static bool xIsBetween(this DateTime value, DateTime from, DateTime to)
        {
            if (value >= from && value <= to) return true;
            return false;
        }

        /// <summary>
        /// Checks if a TimeSpan value is between two other TimeSpan values, inclusive.
        /// </summary>
        /// <param name="value">The TimeSpan value to check.</param>
        /// <param name="from">The lower bound TimeSpan.</param>
        /// <param name="to">The upper bound TimeSpan.</param>
        /// <returns>True if the TimeSpan value is between the two bounds, otherwise false.</returns>
        public static bool xIsBetween(this TimeSpan value, TimeSpan from, TimeSpan to)
        {
            if (value <= TimeSpan.Zero) throw new Exception("not allow value");
            if (value >= from && value <= to) return true;
            return false;
        }
        
        /// <summary>
        /// Checks if a TimeSpan value is between two nullable TimeSpan values, inclusive.
        /// </summary>
        /// <param name="value">The TimeSpan value to check.</param>
        /// <param name="from">The lower bound nullable TimeSpan.</param>
        /// <param name="to">The upper bound nullable TimeSpan.</param>
        /// <returns>True if the TimeSpan value is between the two bounds, otherwise false.</returns>
        public static bool xIsBetween(this TimeSpan value, TimeSpan? from, TimeSpan? to)
        {
            if (from.xIsEmpty()) throw new Exception("from is empty");
            if (to.xIsEmpty()) throw new Exception("to is empty");
            
            if (value <= TimeSpan.Zero) throw new Exception("not allow value");
            
            if (value >= from && value <= to) return true;
            return false;
        }        

        /// <summary>
        /// Checks if a char value is between two other char values, inclusive, with an option for strict comparison.
        /// </summary>
        /// <param name="value">The char value to check.</param>
        /// <param name="from">The lower bound char.</param>
        /// <param name="to">The upper bound char.</param>
        /// <param name="isStrict">If true, uses a strict comparison (case-sensitive), otherwise uses a non-strict comparison (case-insensitive).</param>
        /// <returns>True if the char value is between the two bounds, otherwise false.</returns>
        public static bool xIsBetween(this char value, char from, char to, bool isStrict = true)
        {
            var v = Convert.ToByte(isStrict ? value : char.ToUpper(value));
            var f = Convert.ToByte(isStrict ? from : char.ToUpper(from));
            var t = Convert.ToByte(isStrict ? to : char.ToUpper(to));
            if (f <= v && t >= v) return true;
            return false;
        }
        
        /// <summary>
        /// Converts a collection to a Span.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="items">The collection to convert to a Span.</param>
        /// <returns>A Span containing the elements from the collection.</returns>
        public static Span<T> xToSpan<T>(this IEnumerable<T> items) where T : class, new()
        {
            return items.xToArray().AsSpan();
        }

        /// <summary>
        /// Converts a collection to an ImmutableArray.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="items">The collection to convert to an ImmutableArray.</param>
        /// <returns>An ImmutableArray containing the elements from the collection.</returns>
        public static ImmutableArray<T> xToImmutableArray<T>(this IEnumerable<T> items)
        {
            var array = items.ToArray();
            return Unsafe.As<T[], ImmutableArray<T>>(ref array);
        }

        /// <summary>
        /// Converts an object to a byte array by serializing it to a JSON string and then converting that string to bytes.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert.</typeparam>
        /// <param name="value">The object to convert to a byte array.</param>
        /// <returns>A byte array representing the object.</returns>
        public static byte[] xToBytes<T>(this T value)
        {
            var objToString = System.Text.Json.JsonSerializer.Serialize(value, new JsonSerializerOptions()
            {
                WriteIndented = false
            });
            return System.Text.Encoding.UTF8.GetBytes(objToString);
        }
    }
}