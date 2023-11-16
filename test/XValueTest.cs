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
        var dmin = double.MinValue;
        Assert.IsTrue(dmin.xIsEmpty());

        var nzero = 0;
        Assert.IsTrue(nzero.xIsEmpty());
        Assert.IsFalse(nzero.xIsNotEmpty());
    }

    [Test]
    public void number_is_or_test()
    {
        var n = 4;
        Assert.Multiple(() =>
        {
            Assert.That(n.xIsNumber(), Is.True);
            Assert.That(n.xValue<Int32>() == 4, Is.True);
        });
    }

    [Test]
    public void like_test()
    {
        var a1 = new[] { "1", "2", "3", "11" };
        var a2 = new[] { "1", "2" };
        var exist = a1.FirstOrDefault(m => a2.Contains(m));
        Assert.NotNull(exist);
        TestContext.Out.WriteLine(exist);

        var contains = a1.xLike(a2);
        Assert.Greater(contains.Count(), 1);

        var item = a1.xLikeFirst(a2);
        Assert.AreEqual(item, "1");
    }

    [Test]
    public void duplicate_test()
    {
        var a1 = new[] { "1", "3", "2", "3", "11" };

        if (a1.xTryDuplicate(out var d))
        {
            TestContext.Out.WriteLine(d);
            Assert.AreEqual("3", d);
        }
    }
}