namespace eXtensionSharp.Job;

public class JobProcessor<T> : IDisposable
    where T : class
{
    private JobHandler<T> _handler;
    private Action<T> _action;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    
    private Thread _thread;
    public JobProcessor()
    {
        _thread = new Thread(Start);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void SetProcess(JobHandler<T> jobHandler, Action<T> callback)
    {
        _handler = jobHandler;
        _action = callback;
    }

    private void Start()
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            try
            {
                if (_handler.xIsNotEmpty())
                {
                    if (_action.xIsNotEmpty())
                    {
                        var item = _handler.Dequeue();
                        if (item.xIsNotNull())
                        {
                            _action(item);    
                        }
                    }
                }
                Thread.Sleep(10);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                break;
            }
        }
    }
    
    public void Stop()
    {
        _cts.Cancel();
    }
    
    public void Dispose()
    {
        _cts?.Cancel();
    }
}