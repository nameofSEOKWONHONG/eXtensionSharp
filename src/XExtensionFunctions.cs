namespace eXtensionSharp;

public class XExtensionFunctions
{
    public async Task SetTimeout(Func<Task> func, int interval = 1000, CancellationToken cancellationToken = new())
    {
        await Task.Delay(interval, cancellationToken)
            .ContinueWith(async (t) => 
            {
                await func();
            }, cancellationToken);   
    }
}