using System;
using System.Threading.Tasks;

namespace eXtensionSharp;

public static class XException
{
    public static void xThen<TSelf>(this TSelf self, Action<TSelf> execute)
    {
        execute(self);
    }

    public static TSelf xThen<TSelf>(this TSelf self, Func<TSelf, TSelf> execute)
    {
        return execute(self);
    }
    
    public static void xThen<TSelf>(this TSelf self, Action<TSelf> execute, Action<Exception> failed)
    {
        try
        {
            execute(self);
        }
        catch(Exception e)
        {
            failed(e);
        }
    }

    public static void xThen<TSelf, TException>(this TSelf self, Action<TSelf> execute, Action<TException> failed) where TException : Exception, new()
    {
        try
        {
            execute(self);
        }
        catch (TException e)
        {
            failed(e);
        }
    }
    
    public static async Task xThenAsync<TSelf>(this TSelf self, Func<TSelf, Task> execute, Func<Exception, Task> failed)
    {
        try
        {
            await execute(self);
        }
        catch(Exception e)
        {
            await failed(e);
        }
    }    
}