using System;
using System.Numerics;

namespace eXtensionSharp;

public static class INumberExtensions
{
    extension<T>(T source) where T : INumber<T>
    {
        public bool xIsEmptyNumber()
        {
            if (source.xIsEmpty()) return true;
            
            T zero = default;
            return source <= zero;
        }

        public bool xIsNotEmptyNumber()
        {
            return !source.xIsEmptyNumber();
        }

        /// <summary>
        /// Checks if a value is between two other values, inclusive.
        /// </summary>
        /// <typeparam name="T">The type of the values, which must implement INumber.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="from">The lower bound value.</param>
        /// <param name="to">The upper bound value.</param>
        /// <returns>True if the value is between the two bounds, otherwise false.</returns>
        public bool xIsBetween(T from, T to)
        {
            if (from > to) throw new Exception("from value is greater than to value.");
            if (source >= from && source <= to) return true;
            return false;
        }        

        /// <summary>
        /// Converts an integer date (e.g., 20241231) to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="date">Integer representation of the date (e.g., 20241231).</param>
        /// <returns>A <see cref="DateTime"/> object if successful; otherwise, null.</returns>
        public DateTime? xToYMD()
        {
            var str = source.xValue<string>();
            if (str.Length < 8) return null;

            var dt = new DateTime(
                int.Parse(str.xSubstring(0, 4)),
                int.Parse(str.xSubstring(4, 2)),
                int.Parse(str.xSubstring(6, 2)));
            return dt;
        }        
    }
}