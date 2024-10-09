using System.Collections.Concurrent;
using System.Security.Claims;

namespace eXtensionSharp.Job;

public class JobHandler<T> where T : class
{
    private static Lazy<JobHandler<T>> _instance = new Lazy<JobHandler<T>>(() => new JobHandler<T>());
    public static JobHandler<T> Instance => _instance.Value;
    
    private readonly ConcurrentQueue<T> _queue;
    private JobHandler()
    {
        _queue = new ConcurrentQueue<T>();
    }
    
    public void Enqueue(T item) => _queue.Enqueue(item);

    public T Dequeue()
    {
        if (_queue.TryDequeue(out var result))
        {
            return result;
        }

        return null;
    }
}

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
    
    internal class ProcessItem
    {
        public JobHandler<T> JobHandler { get; set; }
        public Action<T> Callback { get; set; }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}

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

    internal class ProcessItem
    {
        public JobHandler<T> JobHandler { get; set; }
        public Action<T> Callback { get; set; }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _task?.Wait();
        
        _cts?.Dispose();
        _task?.Dispose();
    }
}