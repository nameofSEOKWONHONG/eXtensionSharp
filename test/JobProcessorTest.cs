using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp.Job;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class JobProcessorTest
{
    [Test]
    public async Task job_processor_test()
    {
        JobProcessor<string>.Instance.SetProcessor(JobHandler<string>.Instance, item =>
        {
            if(item.xIsNotEmpty()) TestContext.WriteLine(item);
        });
        JobProcessor<int>.Instance.SetProcessor(JobHandler<int>.Instance, item =>
        {
            if(item > 0) TestContext.WriteLine(item);
        });
            
        var texts = "hello world";
        var numbers = new[] { 1, 2, 3, 4, 5 };

        Parallel.ForEach(texts.ToArray(), item =>
        {
            JobHandler<string>.Instance.Enqueue(item.ToString());
        });
        Parallel.ForEach(numbers, item =>
        {
            JobHandler<int>.Instance.Enqueue(item);
        });

        await Task.Delay(5000);
            
        JobProcessor<int>.Instance.Stop();
    }
}