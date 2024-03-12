using System.Numerics;
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

        public static async Task xForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            if (items.xIsEmpty()) return;

            int i = 0;
            foreach (var value in items)
            {
                await func(value);
                i++;
                if ((i % LOOP_DELAY_COUNT) == 0)
                {
                    await Task.Delay(LOOP_SLEEP_MS);
                }
            }
        }

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<int, T> action)
        {
            var i = 0;
            iterator.xForEach(item =>
            {
                action(i, item);
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

        public static void xForEach<T>(this IEnumerable<T> iterator, Func<int, T, bool> func)
        {
            var i = 0;
            iterator.xForEach(item =>
            {
                var isBreak = !func(i, item);
                i += 1;
                if (isBreak) return false;
                return true;
            });
        }

        public static void xForEach(this ValueTuple<DateTime, DateTime> dateRange, Action<DateTime> action)
        {
            var idx = 0;
            for (var i = dateRange.Item1; i<dateRange.Item2; i = i.AddDays(1))
            {
                action(i);
                idx += 1;
                if ((idx % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);
                }
            }
        }

        public static void xForEach(this ValueTuple<DateTime, DateTime> dateRange, Func<DateTime, bool> func)
        {
            dateRange.xForEach((index, item) => { return func(item); });
        }

        public static void xForEach(this ValueTuple<DateTime, DateTime> dateRange, Func<int, DateTime, bool> func)
        {
            var idx = 0;
            for (var i = dateRange.Item1; i < dateRange.Item2; i = i.AddDays(1))
            {
                var @continue = func(idx, i);
                if (@continue.xIsFalse()) break;

                idx += 1;
                if ((idx % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);
                }
            }
        }

        public static void xForEach(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            var idx = 0;
            for (var i = fromTo.Item1; i <= fromTo.Item2; i++) { 
                action(i);

                idx += 1;
                if ((idx % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);
                }
            }
        }

        public static void xForEach<T>(this ValueTuple<T, T> fromTo, Func<int, bool> func)
            where T : INumber<T>
        {
            fromTo.xForEach<T>((index, item) =>
            {
                return func(index);
            });
        }

        public static void xForEach<T>(this ValueTuple<T, T> fromTo, Func<int, T, bool> func)
            where T : INumber<T>
        {
            var idx = 0;
            for (var i = fromTo.Item1; i <= fromTo.Item2; i++)
            {
                var @continue = func(idx, i);
                if (@continue.xIsFalse()) break;

                idx += 1;
                if ((idx % LOOP_DELAY_COUNT) == 0)
                {
                    Thread.Sleep(LOOP_SLEEP_MS);
                }
            }
        }

        #endregion [정규목록]
        
        public static IEnumerable<T[]> xBatch<T>(this IEnumerable<T> valaus, int batchSize)
        {
            var result = new List<T[]>();
            var array = valaus.ToArray();
            for (int i = 0; i < array.Length; i += batchSize)
            {
                var batch = array.Skip(i).Take(batchSize).ToArray();
                result.Add(batch);
            }

            return result;
        }

        /// <summary>
        /// Parellel ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <param name="options">ParallelOptions</param>
        /// <returns></returns>
        public static async Task<bool> xParallelForEachAsync<T>(this IEnumerable<T> items, Func<T, CancellationToken, Task> func, ParallelOptions options = null)
        {
            if (options.xIsEmpty())
            {
                options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,                    
                };
            }

            var count = 0;
            var canceled = false;

            //need to be sure this is the correct way to do it.
            try
            {
                await Parallel.ForEachAsync(items, options, async (item, token) =>
                {
                    token.ThrowIfCancellationRequested();

                    await func(item, token);
                    count++;
                    if (count % LOOP_DELAY_COUNT == 0)
                    {
                        await Task.Delay(LOOP_SLEEP_MS, token);
                    }
                });
            }
            catch (OperationCanceledException) 
            { 
                canceled = true;
            }

            return canceled;
        }
    }
}