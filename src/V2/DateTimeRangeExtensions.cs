using System;
using System.Globalization;

namespace eXtensionSharp;

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

        public void xForEach(Func<DateTime, int, bool> func)
        {
            var idx = 0;
            for (var i = source.Item1; i <= source.Item2; i = i.AddDays(1))
            {
                var @break = func(i, idx);
                if (!@break) break;

                idx += 1;
            }
        }        

        public void xForEach(Func<DateTime, bool> func)
        {
            source.xForEach((item, index) => func(item));
        }

  
    }
}

