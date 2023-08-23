using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace eXtensionSharp
{
    public static class XForEachExtensions
    {
        private const int LOOP_DELAY_COUNT = 5000;
        private const int LOOP_SLEEP_MS = 100;
        
        #region [정규목록]

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<T> action)
        {
            if (iterator.xIsEmpty()) return;
            int i = 0;
            foreach (var item in iterator)
            {
                action(item);
                i++;
                if ((i % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);    
                }
            }
        }

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<T, int> action)
        {
            var i = 0;
            iterator.xForEach(item =>
            {
                action(item, i);
                i += 1;
            });
        }

        /// <summary>
        ///     use class, allow break;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="func"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Func<T, bool> func)
        {
            if (iterator.xIsEmpty()) return;
            
            var i = 0;
            foreach (var item in iterator)
            {
                var isBreak = !func(item);
                if (isBreak) break;
                
                i += 1;
                if ((i % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);    
                }                
            }
        }

        public static void xForEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func)
        {
            var i = 0;
            iterator.xForEach(item =>
            {
                var isBreak = !func(item, i);
                i += 1;
                if (isBreak) return false;
                return true;
            });
        }

        public static void xForEach(this DateTime from, DateTime to, ENUM_DATETIME_FOREACH_TYPE type,
            Func<DateTime, bool> func)
        {
            var states = new Dictionary<ENUM_DATETIME_FOREACH_TYPE, Action<DateTime, DateTime>>()
            {
                {
                    ENUM_DATETIME_FOREACH_TYPE.DAY, (from, to) =>
                    {
                        for (var i = from; i <= to; i = i.AddDays(1))
                        {
                            bool isContinue = func(i);
                            if(isContinue.xIsFalse()) break;
                        }
                            
                    }
                },
                {
                    ENUM_DATETIME_FOREACH_TYPE.MONTH, (from, to) =>
                    {
                        for (var i = from; i <= to; i = i.AddMonths(1))
                        {
                            bool isContinue = func(i);
                            if(isContinue.xIsFalse()) break;
                        }

                    }
                },
                {
                    ENUM_DATETIME_FOREACH_TYPE.YEAR, (from, to) =>
                    {
                        for (var i = from; i <= to; i = i.AddYears(1))
                        {
                            bool isContinue = func(i);
                            if(isContinue.xIsFalse()) break;
                        }
                    }
                }
            };

            states[type](from, to);
        }

        public static void xForEach(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            for (var i = fromTo.Item1; i <= fromTo.Item2; i++) action(i);
        }

        #endregion [정규목록]

        #region [확장목록]

        public static void xForEachSpan<T>(this IEnumerable<T> items, Action<T> action)
        {
            Span<T> asSpan = items.ToArray();
            asSpan.xForEachSpan((item, i) =>
            {
                action(item);
            });
        }

        public static void xForEachSpan<T>(this Span<T> items, Action<T, int> action)
        {
            ref var searchSpace = ref MemoryMarshal.GetReference(items);
            for (var i = 0; i < items.Length; i++)
            {
                var item = Unsafe.Add(ref searchSpace, i);
                action(item, i);
                if ((i % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);    
                }
            }
        }

        public static void xForEachReversal(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            for (var i = fromTo.Item2; i >= fromTo.Item1; i--) action(i);
        }

        public static void xForEachReversal<T>(this IEnumerable<T> iterator, Action<T> action)
        {
            iterator.Reverse().xForEach(action);
        }

        public static void xForEachReverse<T>(this IEnumerable<T> iterator, Func<T, bool> func)
        {
            iterator.Reverse().xForEach(func);
        }

        public static void xForEachParallel<T>(this IEnumerable<T> items, Action<T> action, ParallelOptions parallelOptions)
        {
            Parallel.ForEach(items, parallelOptions, action);
        }

        public static async Task xForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            if(items.xIsEmpty()) return;

            foreach (var value in items)
            {
                await func(value);
            }
        }

        public static async Task xForEachParallelAsync<T>(this IEnumerable<T> items, Func<T, CancellationToken, Task> func, ParallelOptions parallelOptions)
        {
            await Parallel.ForEachAsync(items, parallelOptions, async (item, token) =>
            {
                await func(item, token);
            });
        }

        #endregion [확장목록]
        
        public static IEnumerable<T[]> xBatch<T>(this IEnumerable<T> valaus, int batchSize)
        {
            var result = new List<T[]>();
            var array = valaus.ToArray();
            for (int i = 0; i < array.Length; i += batchSize)
            {
                T[] batch = array.Skip(i).Take(batchSize).ToArray();
                result.Add(batch);
            }

            return result;
        }
    }

    public class ENUM_DATETIME_FOREACH_TYPE : XEnumBase<ENUM_DATETIME_FOREACH_TYPE>
    {
        public static readonly ENUM_DATETIME_FOREACH_TYPE DAY = Define("DAY");
        public static readonly ENUM_DATETIME_FOREACH_TYPE MONTH = Define("MONTH");
        public static readonly ENUM_DATETIME_FOREACH_TYPE YEAR = Define("YEAR");
    }
}