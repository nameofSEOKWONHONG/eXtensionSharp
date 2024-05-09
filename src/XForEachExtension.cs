using System.Numerics;

namespace eXtensionSharp
{
    /// <summary>
    /// 2024.05.09 - remove thread.sleep
    ///            - remove number type from to xforeach, use Enumerable.Range(from, to)
    ///            - change xforeach func item priority. (int Index, Generic T Item)
    /// caution : will happen object null exception.
    /// case 1 : use AOT, happen exception after serialize object. 
    /// </summary>
    public static class XForEachExtensions
    {
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
            if (iterator.xIsEmpty()) return;
            
            int i = 0;
            foreach (var item in iterator)
            {
                action(i, item);
                i++;
            }
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
        
        public static async Task xForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            if (items.xIsEmpty()) return;

            foreach (var value in items)
            {
                await func(value);
            }
        }
        
        public static async Task xForEachAsync<T>(this IEnumerable<T> items, Func<int, T, Task> func)
        {
            if (items.xIsEmpty()) return;

            int i = 0;            
            foreach (var value in items)
            {
                await func(i, value);
                i++;
            }
        }        

        public static void xForEach(this ValueTuple<DateTime, DateTime> dateRange, Action<DateTime> action)
        {
            for (var i = dateRange.Item1; i<dateRange.Item2; i = i.AddDays(1))
            {
                action(i);
            }
        }
        
        public static void xForEach(this ValueTuple<DateTime, DateTime> dateRange, Func<int, DateTime, bool> func)
        {
            var idx = 0;
            for (var i = dateRange.Item1; i < dateRange.Item2; i = i.AddDays(1))
            {
                var @continue = func(idx, i);
                if (@continue.xIsFalse()) break;

                idx += 1;
            }
        }        

        public static void xForEach(this ValueTuple<DateTime, DateTime> dateRange, Func<DateTime, bool> func)
        {
            dateRange.xForEach((index, item) => func(item));
        }

        #endregion [정규목록]
        
        public static IEnumerable<T[]> xBatch<T>(this IEnumerable<T> valaus, int batchSize)
        {
            var array = valaus.ToArray();
            for (int i = 0; i < array.Length; i += batchSize)
            {
                var batch = array[i..(i+batchSize)];
                yield return batch;
            }
        }

        /// <summary>
        /// Parellel ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <param name="options">ParallelOptions</param>
        /// <returns></returns>
        public static async Task<bool> xParallelForEachAsync<T>(this IEnumerable<T> items, ParallelOptions options, Func<T, CancellationToken, Task> func)
        {
            if (options.xIsEmpty())
            {
                options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount),
                    CancellationToken = default
                };
            }

            var isCancel = false;

            //need to be sure this is the correct way to do it.
            try
            {
                await Parallel.ForEachAsync(items, options, async (item, token) =>
                {
                    token.ThrowIfCancellationRequested();

                    await func(item, token);
                });
            }
            catch (OperationCanceledException) 
            { 
                isCancel = true;
            }

            return isCancel;
        }
    }
}