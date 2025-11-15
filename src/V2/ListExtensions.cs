using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;

namespace eXtensionSharp;

public static class ListExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>
        /// Retrieves an element from the specified index of a read-only list.
        /// If the index is out of range or the list is empty, the default value of the element type is returned.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="items">The read-only list from which to retrieve the element.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>The element at the specified index, or the default value of the type if the index is invalid or the list is empty.</returns>
        public T xGet(int index)
        {
            var src = source.ToList();
            if (source.xIsEmpty()) return default;
            if (index < 0) return default;
            if (index >= source.Count()) return default;
            return src[index];
        }
        
        /// <summary>
        /// Converts an IEnumerable collection of objects to a collection of dictionaries, where each dictionary represents an object.
        /// </summary>
        /// <returns>A collection of dictionaries representing the objects.</returns>
        public IEnumerable<DynamicDictionary<object>> xToDictionaries()
        {
            if(typeof(T) == typeof(ExpandoObject)) 
            {
                return source.Select(m => new DynamicDictionary<object>((IDictionary<string, object>)m));
            }

            var list = new List<DynamicDictionary<object>>();
            foreach (var value in source)
            {
                var item = value.xToDictionary<T>();
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Counts the number of elements in a collection, optionally using a predicate to filter the elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="predicate">A function to test each element, or null to count all elements.</param>
        /// <returns>The number of elements in the collection, or the number of elements matching the predicate.</returns>
        public int xCount(Func<T, bool> predicate = null)
        {
            if (source.xIsEmpty()) return 0;
            if (predicate.xIsNotEmpty()) return source.Count(predicate);
            return source.Count();
        }

        public bool xContains(IEnumerable<T> compares)
        {
            if (source.xIsEmpty()) return false;
            return source.Any(m => compares.Contains(m));
        }

        public string xJoin(string separator = ",")
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Retrieves the first element from a collection that matches a given condition, or the first element if no condition is specified.
        /// </summary>
        /// <param name="predicate">A function to test each element, or null to retrieve the first element.</param>
        /// <returns>The first element that matches the condition, or the first element in the collection if no condition is provided.</returns>
        public T xFirst(Func<T, bool> predicate = null)
        {
            if (source.xIsEmpty()) return default;
            if (predicate.xIsNotEmpty()) return source.FirstOrDefault(predicate);
            
            return source.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the last element from a collection that matches a given condition, or the last element if no condition is specified.
        /// </summary>
        /// <param name="predicate">A function to test each element, or null to retrieve the last element.</param>
        /// <returns>The last element that matches the condition, or the last element in the collection if no condition is provided.</returns>
        public T xLast(Func<T, bool> predicate = null)
        {
            if (source.xIsEmpty()) return default;
            if (predicate.xIsNotEmpty()) return source.LastOrDefault(predicate);
            
            return source.LastOrDefault();
        }

        /// <summary>
        /// Converts a collection to a list, returning an empty list if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to convert to a list.</param>
        /// <returns>A list containing the elements from the collection, or an empty list if the collection is empty.</returns>
        public List<T> xToList()
        {
            return source.xIsEmpty() ? new List<T>() : new List<T>(source);
        }

        /// <summary>
        /// Converts a collection to an array, returning an empty array if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="enumerable">The collection to convert to an array.</param>
        /// <returns>An array containing the elements from the collection, or an empty array if the collection is empty.</returns>
        public T[] xToArray()
        {
            if (source.xIsEmpty()) return new T[0];
            return source.ToArray();
        }  

        public Span<T> xAsSpan(int start=0, int length=0)
        {
            var src = source.ToList();
            if (source.xIsEmpty()) return Span<T>.Empty;
            if (start < 0 || start >= src.Count) throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0 || (start + length) > src.Count) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 0 && start == 0) return CollectionsMarshal.AsSpan(src);
            if (length == 0) length = src.Count - start;
            return CollectionsMarshal.AsSpan(src).Slice(start, length);
        }

        public ReadOnlySpan<T> xAsReadOnlySpan(int start = 0, int length = 0)
        {
            return source.xAsSpan(start, length);
        }        

        public Memory<T> xAsMemory(int start = 0, int length = 0)
        {
            var src = source.ToList();
            if (source.xIsEmpty()) return Memory<T>.Empty;
            if (start < 0 || start >= src.Count) throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0 || (start + length) > src.Count) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 0 && start == 0) return CollectionsMarshal.AsSpan(src).ToArray();
            if (length == 0) length = src.Count - start;
            return CollectionsMarshal.AsSpan(src).Slice(start, length).ToArray();
        }

        public ReadOnlyMemory<T> xAsReadOnlyMemory(int start = 0, int length = 0)
        {
            return source.xAsMemory(start, length);
        }     

        public DataTable xToDataTable()
        {
            if (source.xIsEmpty()) return new DataTable();
            
            var properties = source.xFirst().GetType().GetProperties();

            var dt = new DataTable();
            foreach (var property in properties) dt.Columns.Add(property.Name, property.PropertyType);

            source.xForEach(item =>
            {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach (var property in itemProperty) row[property.Name] = property.GetValue(item);
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }           
    }
}
