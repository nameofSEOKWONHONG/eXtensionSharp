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