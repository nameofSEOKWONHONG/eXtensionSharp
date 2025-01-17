namespace eXtensionSharp;

public class XThread
{
    public static async Task SetTimeout(Func<Task> func, int interval = 1000, CancellationToken cancellationToken = default)
    {
        if (func.xIsEmpty()) throw new ArgumentException(nameof(func));
        
        await Task.Delay(interval, cancellationToken)
            .ContinueWith(async (t) => 
            {
                await func();
            }, cancellationToken);   
    }
}