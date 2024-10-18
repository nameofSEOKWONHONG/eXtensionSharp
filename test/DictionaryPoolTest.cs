using System.Linq;
using System.Threading.Tasks;
using Collections.Pooled;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class DictionaryPoolTest
{
    [Test]
    public void test1()
    {
        PooledDictionary<string, int> dictionaryPool = new PooledDictionary<string, int>();
        Parallel.ForEach(Enumerable.Range(1, 5000).ToList(), item =>
        {
            dictionaryPool.Add(item.ToString(), 1);
        });
        Assert.That(dictionaryPool.Count, Is.EqualTo(5000));
        Assert.That(dictionaryPool["1"], Is.EqualTo(1));
        dictionaryPool.Dispose();
    }
}