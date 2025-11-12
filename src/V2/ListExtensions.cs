using System;

namespace eXtensionSharp.V2;

public static class ListExtensions
{
    extension<T>(IReadOnlyList<T> source)
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
            if (source.xIsEmpty()) return default;
            if (index < 0) return default;
            if (index >= source.Count) return default;
            return source[index];
        }
        
        
    }
}
