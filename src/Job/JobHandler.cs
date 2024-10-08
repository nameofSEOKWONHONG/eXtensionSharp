using System.Collections.Concurrent;

namespace eXtensionSharp.Job;

public class JobHandler<T>
{
    private static Lazy<JobHandler<T>> _instance = new Lazy<JobHandler<T>>(() => new JobHandler<T>());
    public static JobHandler<T> Instance => _instance.Value;
    
    private readonly ConcurrentQueue<T> _queue;
    private JobHandler()
    {
        _queue = new ConcurrentQueue<T>();
    }
    
    public void Enqueue(T item) => _queue.Enqueue(item);
    public T Dequeue() => _queue.TryDequeue(out var result) ? result : default;
}

public class JobProcessor<T>
{
    private static Lazy<JobProcessor<T>> _instance = new Lazy<JobProcessor<T>>(() => new JobProcessor<T>());
    public static JobProcessor<T> Instance => _instance.Value;


    private JobHandler<T> _handler;
    private Action<T> _action;
    private bool _isRunning;
    
    private Thread _thread;
    private JobProcessor()
    {
        _thread = new Thread(Start);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void SetProcessor(JobHandler<T> jobHandler, Action<T> callback)
    {
        _handler = jobHandler;
        _action = callback;
    }

    private void Start()
    {
        _isRunning = true;
        
        while (_isRunning)
        {
            try
            {
                if (_handler.xIsNotEmpty())
                {
                    if (_action.xIsNotEmpty())
                    {
                        var item = _handler.Dequeue();
                        if (item.xIsNotEmpty())
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
        _isRunning = false;
    }
    
    internal class ProcessItem
    {
        public JobHandler<T> JobHandler { get; set; }
        public Action<T> Callback { get; set; }
    }
}