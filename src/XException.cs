using System;

namespace eXtensionSharp;

public class XException
{
    public static void xException(Action action, Action<Exception> expAction)
    {
        try
        {
            action();
        }
        catch(Exception e)
        {
            expAction(e);
        }
    }

    public static void xException<TException>(Action action, Action<TException> expAction) where TException : Exception, new()
    {
        try
        {
            action();
        }
        catch (TException e)
        {
            expAction(e);
        }
    }
}