using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XCollectionExtensionsTest
{
    [Test]
    public void dictionary_performance_improvement_test1()
    {
        var values = new Dictionary<string, int>
        {
            { "test", 12345 }
        };
        var val = values.xGetOrAdd("test1", 1827387);
        Assert.That(val, Is.EqualTo(1827387));

        var updated = values.xTryUpdate("test1", 0);
        Assert.That(updated, Is.True);

        var json = values.xSerialize();
        Assert.That(json, Is.EqualTo(@"{""test"":12345,""test1"":0}"));
    }
}