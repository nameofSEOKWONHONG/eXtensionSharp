using System;
using System.Numerics;
using System.Text;

namespace eXtensionSharp.V2;

public static class NumberExtensions
{
    extension(int source)
    {
        public string xToRandomString(bool lowerCase = false)
        {
            var builder = new StringBuilder(source);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.

            // char is a single Unicode character
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26

            var random = new Random();
            for (var i = 0; i < source; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}

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