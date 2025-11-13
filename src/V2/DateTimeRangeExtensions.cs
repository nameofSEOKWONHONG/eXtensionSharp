using System;

namespace eXtensionSharp.V2;

public static class DateTimeRangeExtensions
{
    extension(ValueTuple<DateTime, DateTime> source)
    {
        public void xForEach(Action<DateTime> action)
        {
            for (var i = source.Item1; i<source.Item2; i = i.AddDays(1))
            {
                action(i);
            }
        }

        public void xForEach(Func<int, DateTime, bool> func)
        {
            var idx = 0;
            for (var i = source.Item1; i < source.Item2; i = i.AddDays(1))
            {
                var @break = func(idx, i);
                if (@break) break;

                idx += 1;
            }
        }        

        public void xForEach(Func<DateTime, bool> func)
        {
            source.xForEach((index, item) => func(item));
        }
    }
}