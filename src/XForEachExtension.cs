using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XForEachExtensions
    {
        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<T> action)
        {
            if (iterator.xIsEmpty()) return;
            var enumerable = iterator as T[] ?? iterator.ToArray();
            foreach (var item in enumerable)
            {
                action(item);
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
            var index = 0;
            iterator.xForEach(item =>
            {
                action(item, index);
                index++;
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
            var enumerable = iterator as T[] ?? iterator.ToArray();
            foreach (var item in enumerable)
            {
                var isBreak = !func(item);
                if (isBreak) break;
            }
        }
        
        public static void xForEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func)
        {
            var index = 0;
            iterator.xForEach(item =>
            {
                var isBreak = !func(item, index);
                if (isBreak) return false;
                return true;
            });
        }
        
        public static void xForEach<T>(this T[] enumerable, Action<T> action)
        {
            if (enumerable.xIsEmpty()) return;
            if(enumerable.xCount() > XConst.LOOP_WARNING_COUNT) Debug.WriteLine($"Too much items : ({XConst.LOOP_WARNING_COUNT})");

            enumerable.AsEnumerable().xForEach(item =>
            {
                action(item);
            });
        }
        
        public static void xForEach(this (DateTime from, DateTime to) fromToDate, ENUM_DATETIME_FOREACH_TYPE type,
            Action<DateTime> action)
        {
            var states = new Dictionary<ENUM_DATETIME_FOREACH_TYPE, Action<DateTime, DateTime>>()
            {
                {
                    ENUM_DATETIME_FOREACH_TYPE.DAY, (from, to) =>
                    {
                        for (var i = from; i <= to; i = i.AddDays(1))
                            action(i);
                    }
                },
                {
                    ENUM_DATETIME_FOREACH_TYPE.MONTH, (from, to) =>
                    {
                        for (var i = from; i <= to; i = i.AddMonths(1))
                            action(i);
                    }
                },
                {
                    ENUM_DATETIME_FOREACH_TYPE.YEAR, (from, to) =>
                    {
                        for (var i = from; i <= to; i = i.AddYears(1))
                            action(i);
                    }
                }
            };

            states[type](fromToDate.from, fromToDate.to);
        }

        public static void xForEach(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            for (var i = fromTo.Item1; i <= fromTo.Item2; i++) action(i);
        }

        public static void xForEachReverse(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            for (var i = fromTo.Item2; i >= fromTo.Item1; i--) action(i);
        }

        public static void xForEachReverse<T>(this IEnumerable<T> itorator, Action<T> action)
        {
            itorator.Reverse().xForEach(item =>
            {
                action(item);
            });
        }

        public static void xForEachReverse<T>(this IEnumerable<T> itorator, Func<T, bool> func)
        {
            itorator.Reverse().xForEach(item =>
            {
                return func(item);
            });
        }

        public static void xForEachParallel<T>(this IEnumerable<T> items, Action<T> action, ParallelOptions parallelOptions = null)
        {
            Parallel.ForEach(items, parallelOptions, action);
        }

        public static void xForEachParallel<T>(this IEnumerable<T> items,
            Func<IEnumerable<T>, IEnumerable<IGrouping<string, T>>> groupby,
            Func<string, IEnumerable<T>, IEnumerable<T>> filter,
            Action<T, int> action) where T : class
        {
            var maps = new Dictionary<string, IEnumerable<T>>();
            var groups = groupby(items);
            groups.xForEach(group =>
            {
                var filterResult = filter(group.Key, items);
                maps.Add(group.Key, filterResult);
            });
            
            maps.xForEachParallel(item =>
            {
                var i = 0;
                item.Value.xForEach(item2 =>
                {
                    action(item2, i);
                    i++;
                });
            });
        }

        public static async Task xForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            foreach (var value in items) await func(value);
        }
        
        public static async Task xForEachParallelAsync<T>(this IEnumerable<T> items,  Func<T, CancellationToken, Task> func, ParallelOptions parallelOptions = null)
        {
            if (parallelOptions.xIsEmpty()) parallelOptions = new ParallelOptions();
            await Parallel.ForEachAsync(items, parallelOptions, async (item, token) =>
            {
                await func(item, token);
            });
        }
    }
    
    public class ENUM_DATETIME_FOREACH_TYPE : XEnumBase<ENUM_DATETIME_FOREACH_TYPE>
    {
        public static readonly ENUM_DATETIME_FOREACH_TYPE DAY = Define("DAY");
        public static readonly ENUM_DATETIME_FOREACH_TYPE MONTH = Define("MONTH");
        public static readonly ENUM_DATETIME_FOREACH_TYPE YEAR = Define("YEAR");
    }
}