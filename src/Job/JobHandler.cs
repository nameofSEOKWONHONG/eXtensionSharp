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

public class JobProsessor<T>
{
    private static Lazy<JobProsessor<T>> _instance = new Lazy<JobProsessor<T>>(() => new JobProsessor<T>());
    public static JobProsessor<T> Instance => _instance.Value;


    private ConcurrentDictionary<JobHandler<T>, Action<T>> _concurrentDictionary;
    
    private Thread _thread;
    private JobProsessor()
    {
        _concurrentDictionary = new();
        _thread = new Thread(StartProess);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public void SetProessor(JobHandler<T> jobHandler, Action<T> callback)
    {
        _concurrentDictionary.TryAdd(jobHandler, callback);
    }

    public void StartProess()
    {
        while (true)
        {
            try
            {
                if (_concurrentDictionary.TryGetValue(out var hander))
                {
                    var item = hander.Dequeue();
                    if (item.xAs<int>() > 0)
                    {
                        Console.WriteLine(item.xAs<int>());        
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                break;
            }
        }
    }
}