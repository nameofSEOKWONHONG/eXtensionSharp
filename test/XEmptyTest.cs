using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XEmptyTest
{
    [Test]
    public void number_empty_test()
    {
        var i = 0;
        var f = 1.2f;
        var d = 0.0d;
        var ii = 1;
        var ff = 0.0f;
        var dd = 1.0d;
        Assert.Multiple(() =>
        {
            Assert.That(i.xIsEmpty(), Is.True);
            Assert.That(f.xIsEmpty(), Is.False);
            Assert.That(d.xIsEmpty(), Is.True);
            
            Assert.That(ii.xIsNotEmpty(), Is.True);
            Assert.That(ff.xIsEmpty(), Is.True);
            Assert.That(dd.xIsNotEmpty(), Is.True);            
        });
    }

    [Test]
    public void string_empty_test()
    {
        var s = string.Empty;
        string ss = null;

        var sss = "hello";
        
        Assert.Multiple(() =>
        {
            Assert.That(s.xIsEmpty(), Is.True);
            Assert.That(ss.xIsEmpty(), Is.True);
            Assert.That(sss.xIsEmpty(), Is.False);
        });
    }

    [Test]
    public void reference_empty_test()
    {
        Dictionary<string, object> map = new()
        {
            { "A", "A" }
        };

        var t = new EmptyTest();

        List<string> list = new();
        
        Assert.Multiple(() =>
        {
            Assert.That(map.xIsEmpty(), Is.False);
            Assert.That(map.xIsNotEmpty(), Is.True);
            Assert.That(t.xIsEmpty(), Is.False);
            Assert.That(t.xIsNotEmpty(), Is.True);
            Assert.That(list.xIsNotEmpty(), Is.False);
            Assert.That(list.xIsEmpty(), Is.True);
        });
    }
}

public class EmptyTest
{
    public string Name { get; set; }
}