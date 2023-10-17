using System;
using System.Collections.Generic;
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
}