using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XValueTest
{
    [Test]
    public void datetime_test()
    {
        object o = DateTime.Now;
        o.xValue<DateTime>();
        TestContext.WriteLine(o);
    }

    [Test]
    public void value_collection_test()
    {
        object o = new List<string>() { "1", "2", "3" };
        var list = o.xValue<List<string>>();
        TestContext.WriteLine(list[0]);
    }

    [Test]
    public void value_empty_not_empty_test()
    {
        var n1 = 0;
        Assert.IsTrue(n1.xIsEmpty());

        long n2 = 100;
        Assert.IsTrue(n2.xIsNotEmpty());
    }

    [Test]
    public void number_is_or_test()
    {
        var n = 4;

        var verify = n is 5 or 10;
        Assert.IsTrue(verify);
    }

    [Test]
    public void like_test()
    {
        var a1 = new[] { "1", "2", "3", "11" };
        var a2 = new[] { "1" };
        var exist = a1.FirstOrDefault(m => a2.Contains(m));
        Assert.NotNull(exist);
        TestContext.Out.WriteLine(exist);
    }
}