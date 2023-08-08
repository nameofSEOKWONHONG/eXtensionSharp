using System.Numerics;

namespace eXtensionSharp
{
    public static class XLinqExtensions
    {
        public static int xCount<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty()) return 0;
            return enumerable.Count();
        }

        public static IEnumerable<T> xWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class
        {
            return CreateOrEnumerable(enumerable).Where(predicate);
        }

        public static IEnumerable<T2> xSelect<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> predicate)
            where T1 : class
        {
            return CreateOrEnumerable(enumerable).Select(predicate);
        }

        private static IEnumerable<T> CreateOrEnumerable<T>(IEnumerable<T> enumerable)
        {
            if (enumerable.xIsEmpty()) return Enumerable.Empty<T>();
            return enumerable;
        }

        public static bool xContains(this string src, string compare)
        {
            if (src.xIsEmpty()) return false;
            return src.Contains(compare);
        }

        public static bool xContains(this string src, string[] compares)
        {
            if (src.xIsEmpty()) return false;
            return compares.FirstOrDefault(m => src.Contains(m)).xIsNotEmpty();
        }

        public static bool xContains<T>(this T src, IEnumerable<T> compares)
        {
            if (src.xIsEmpty()) return false;
            return compares.Contains(src);
        }

        public static bool xContains<T>(this IEnumerable<T> src, IEnumerable<T> compares)
        {
            if (src.xIsNotEmpty()) return false;
            return src.Where(m => m.xContains(compares)).xIsNotEmpty();
        }

        public static T xFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotEmpty()) return enumerable.FirstOrDefault(predicate);
            return enumerable.FirstOrDefault();
        }

        public static T xLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null)
        {
            if (predicate.xIsNotNull()) return enumerable.LastOrDefault(predicate);
            return enumerable.LastOrDefault();
        }

        public static List<T> xToList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.xIsEmpty() ? new List<T>() : new List<T>(enumerable);
        }

        public static T[] xToArray<T>(this IEnumerable<T> enumerable) where T : new()
        {
            if (enumerable.xIsEmpty()) return new T[0];
            return enumerable.ToArray();
        }

        #region [between]

        #region [.net 7에서 inumber를 사용하여 하나의 function으로 변경됨.]

        public static bool xIsBetween<T>(this T value, T from, T to)
            where T : INumber<T>
        {
            if (from > to) throw new Exception("from value is greater than to value.");
            if (from <= value && to >= value) return true;
            return false;
        }

        #endregion [.net 7에서 inumber를 사용하여 하나의 function으로 변경됨.]

        public static bool xIsBetween(this DateTime value, DateTime from, DateTime to)
        {
            if (from <= value && to >= value) return true;
            return false;
        }

        public static bool xIsBetween(this TimeSpan value, TimeSpan from, TimeSpan to)
        {
            if (value <= TimeSpan.Zero) throw new Exception("not allow value");
            if (from <= value && to >= value) return true;
            return false;
        }
        
        public static bool xIsBetween(this TimeSpan value, TimeSpan? from, TimeSpan? to)
        {
            if (from.xIsEmpty()) throw new Exception("from is empty");
            if (to.xIsEmpty()) throw new Exception("to is empty");
            
            if (value <= TimeSpan.Zero) throw new Exception("not allow value");
            
            if (from <= value && to >= value) return true;
            return false;
        }        

        public static bool xIsBetween(this char value, char from, char to, bool isStrict = true)
        {
            var v = Convert.ToByte(isStrict ? value : char.ToUpper(value));
            var f = Convert.ToByte(isStrict ? from : char.ToUpper(from));
            var t = Convert.ToByte(isStrict ? to : char.ToUpper(to));
            if (f <= v && t >= v) return true;
            return false;
        }

        #endregion [between]
        
        public static Span<T> xAsSpan<T>(this IEnumerable<T> items) where T : new()
        {
            return items.xToArray().AsSpan();
        }
    }
}