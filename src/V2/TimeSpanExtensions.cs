using System;

namespace eXtensionSharp.V2;

public static class TimeSpanExtensions
{
    extension(TimeSpan source)
    {
        /// <summary>
        /// Checks if a TimeSpan value is between two nullable TimeSpan values, inclusive.
        /// </summary>
        /// <param name="from">The lower bound nullable TimeSpan.</param>
        /// <param name="to">The upper bound nullable TimeSpan.</param>
        /// <returns>True if the TimeSpan value is between the two bounds, otherwise false.</returns>
        public bool xIsBetween(TimeSpan? from, TimeSpan? to)
        {
            if (from.xIsEmpty()) throw new Exception("from is empty");
            if (to.xIsEmpty()) throw new Exception("to is empty");
            
            if (source <= TimeSpan.Zero) throw new Exception("not allow value");
            
            if (source >= from && source <= to) return true;
            return false;
        }

      
    }
}