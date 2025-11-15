namespace eXtensionSharp.Job;

public class JobProcessorAsync<T> : IDisposable
    where T : class
{
    private JobHandler<T> _handler;
    private Func<T, Task> _func;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    private Task _task;

    public JobProcessorAsync()
    {
        _task = Task.Run(Start);
    }

    public void SetProcess(JobHandler<T> jobHandler, Func<T, Task> callback)
    {
        _handler = jobHandler;
        _func = callback;
    }

    private async Task Start()
    {       
        while (!_cts.Token.IsCancellationRequested)
        {
            try
            {
                if (_handler.xIsNotEmpty())
                {
                    if (_func.xIsNotEmpty())
                    {
                        var item = _handler.Dequeue();
                        if (item.xIsNotNull())
                        {
                            await _func(item);    
                        }
                    }
                }
                // Instead of Thread.Sleep, we use Task.Delay to make this asynchronous
                await Task.Delay(10);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                break;
            }
        }
    }

    public void Stop()
    {
        _cts.Cancel();
        _task.Wait(); // Wait for the task to finish gracefully
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _task?.Wait();
    }
}