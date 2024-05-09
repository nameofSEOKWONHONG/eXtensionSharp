using System.Runtime.InteropServices;

namespace eXtensionSharp;

/// <summary>
/// CollectionsMarshal.AsSpan Loop
/// </summary>
public static class XForEachSpanExtensions
{
    public static void xForEachSpan<T>(this List<T> items, Action<T> action)
    {
        if (items.xIsEmpty()) return;

        int i = 0;
        var span = CollectionsMarshal.AsSpan(items);
        foreach (var item in span)
        {
            action(item);
            i++;
        }
    }
    
    public static void xForEachSpan<T>(this List<T> items, Action<int, T> action)
    {
        var i = 0;
        items.xForEachSpan(item =>
        {
            action(i, item);
            i += 1;
        });
    }
    
    public static void xForEachSpan<T>(this List<T> items, Func<T, bool> func)
    {
        if (items.xIsEmpty()) return;
            
        var i = 0;
        var span = CollectionsMarshal.AsSpan(items);
        foreach (var item in span)
        {
            var isBreak = !func(item);
            if (isBreak) break;
                
            i += 1;             
        }
    }
    
    public static void xForEachSpan<T>(this List<T> items, Func<int, T, bool> func)
    {
        var i = 0;
        items.xForEachSpan(item =>
        {
            var isBreak = !func(i, item);
            i += 1;
            if (isBreak) return false;
            return true;
        });
    }
}