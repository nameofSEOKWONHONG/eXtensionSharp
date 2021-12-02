﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XForEach
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
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Debug.WriteLine($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator)
            {
                action(item);
                if (index % XConst.LOOP_WARNING_COUNT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);
                index++;
            }
        }
        
        /// <summary>
        ///     for loop
        /// </summary>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void xFor<T>(this IEnumerable<T> iterator, Action<T> action)
        {
            if (iterator.xIsEmpty()) return;
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++)
            {
                action(srcs[i]);
                if (i % XConst.LOOP_WARNING_COUNT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);
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
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator)
            {
                var isBreak = !func(item);
                if (isBreak) break;

                if (index % XConst.LOOP_WARNING_COUNT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);

                index++;
            }
        }
        
        public static void xForEach(this (DateTime from, DateTime to) fromToDate, ENUM_DATETIME_FOREACH_TYPE type,
            Action<DateTime> action)
        {
            if (type.xIsEquals(ENUM_DATETIME_FOREACH_TYPE.DAY))
                for (var i = fromToDate.@from; i <= fromToDate.to; i = i.AddDays(1))
                    action(i);
            else if (type.xIsEquals(ENUM_DATETIME_FOREACH_TYPE.MONTH))
                for (var i = fromToDate.@from; i <= fromToDate.to; i = i.AddMonths(1))
                    action(i);
            else
                for (var i = fromToDate.@from; i <= fromToDate.to; i = i.AddYears(1))
                    action(i);
        }
        
        public static void xForEach(this IEnumerable<string> itorator, 
            string split, 
            Action<string, string> action)
        {
            itorator.xForEach((item, i) =>
            {
                if (itorator.Count() - 1 == i) split = "";
                action(item, split);
            });
        }

        public static void xFor(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            for (var i = fromTo.Item1; i <= fromTo.Item2; i++) action(i);
        }

        public static void xReverseFor(this ValueTuple<int, int> fromTo, Action<int> action)
        {
            for (var i = fromTo.Item2; i >= fromTo.Item1; i--) action(i);
        }

        public static void xForEachReverse<T>(this IEnumerable<T> itorator, Action<T> action)
        {
            var reverseItems = itorator.Reverse();
            reverseItems.xForEach(item =>
            {
                action(item);
            });
        }

        public static void xForEachReverse<T>(this IEnumerable<T> itorator, Func<T, bool> func)
        {
            var reverseItems = itorator.Reverse();
            reverseItems.xForEach(item =>
            {
                return func(item);
            });
        }


        public static void xPararellForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            Parallel.ForEach(items, action);
        }

        public static void xPararellForEach<T>(this IEnumerable<T> items,
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

            Parallel.ForEach(maps, item =>
            {
                var i = 0;
                item.Value.xForEach(item2 =>
                {
                    action(item2, i);
                    i++;
                });
            });
        }

        /// <summary>
        ///     foreach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void xForEach<T>(this IEnumerable<T> iterator, Action<T, int> action)
        {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.xForEach(item =>
            {
                action(item, index);
                index++;
            });
        }

        public static void xFor<T>(this IEnumerable<T> iterator, Action<T, int> action)
        {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.xFor(item =>
            {
                action(item, index);
                index++;
            });
        }
        
        public static void xForEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func)
        {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.xForEach(item =>
            {
                var isBreak = !func(item, index);
                if (isBreak) return false;

                if (index % XConst.LOOP_WARNING_COUNT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);

                return true;
            });
        }

        public static void xFor<T>(this IEnumerable<T> iterator, Func<T, int, bool> func)
        {
            if (iterator.xCount() > XConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({XConst.LOOP_WARNING_COUNT})");

            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++)
            {
                var isBreak = !func(srcs[i], i);
                if (isBreak) break;

                if (i % XConst.LOOP_WARNING_COUNT == 0)
                    XConst.SetInterval(XConst.SLEEP_INTERVAL);
            }
        }

        public static async Task xForEachAsync<T>(this IEnumerable<T> iterator, Func<T, Task> func)
        {
            foreach (var value in iterator) await func(value);
        }
    }
}