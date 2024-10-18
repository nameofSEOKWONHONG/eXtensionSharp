using NUnit.Framework;

namespace eXtensionSharp.test;

public class CircleListTest
{
    [Test]
    public void next_test()
    {
        var clist = new CircleList<int>();
        clist.Add(1);
        clist.Add(2);
        clist.Add(3);
        clist.Add(4);
        clist.Add(5);
        clist.Add(6);
        
        Assert.That(clist.Next(), Is.EqualTo(1));
        Assert.That(clist.Next(), Is.EqualTo(2));
        Assert.That(clist.Next(), Is.EqualTo(3));
        Assert.That(clist.Next(), Is.EqualTo(4));
        Assert.That(clist.Next(), Is.EqualTo(5));
        Assert.That(clist.Next(), Is.EqualTo(6));
        Assert.That(clist.Next(), Is.EqualTo(1));
    }

    [Test]
    public void prev_test()
    {
        var clist = new CircleList<int>();
        clist.Add(1);
        clist.Add(2);
        clist.Add(3);
        clist.Add(4);
        clist.Add(5);
        clist.Add(6);
        
        Assert.That(clist.Previous(), Is.EqualTo(6));
        Assert.That(clist.Previous(), Is.EqualTo(5));
        Assert.That(clist.Previous(), Is.EqualTo(4));
        Assert.That(clist.Previous(), Is.EqualTo(3));
        Assert.That(clist.Previous(), Is.EqualTo(2));
        Assert.That(clist.Previous(), Is.EqualTo(1));
        Assert.That(clist.Previous(), Is.EqualTo(6));        
    }

    [Test]
    public void complex_test()
    {
        var clist = new CircleList<int>();
        clist.Add(1);
        clist.Add(2);
        clist.Add(3);
        clist.Add(4);
        clist.Add(5);
        clist.Add(6);
        
        Assert.That(clist.Next(), Is.EqualTo(1));
        Assert.That(clist.Next(), Is.EqualTo(2));
        Assert.That(clist.Previous(), Is.EqualTo(1));
        Assert.That(clist.Previous(), Is.EqualTo(6));
        Assert.That(clist.Previous(), Is.EqualTo(5));
        Assert.That(clist.Next(), Is.EqualTo(6));
        Assert.That(clist.Next(), Is.EqualTo(1));         
    }

    [Test]
    public void complex_test2()
    {
        var clist = new CircleList<int>();
        clist.Add(1);
        clist.Add(2);
        clist.Add(3);
        clist.Add(4);
        clist.Add(5);
        clist.Add(6);
        
        Assert.That(clist.Next(), Is.EqualTo(1));
        Assert.That(clist.Next(), Is.EqualTo(2));
        Assert.That(clist.Previous(), Is.EqualTo(1));
        Assert.That(clist.Previous(), Is.EqualTo(6));
        Assert.That(clist.Previous(), Is.EqualTo(5));
        clist.Remove(6);
        Assert.That(clist.Next(), Is.EqualTo(1));
        Assert.That(clist.Next(), Is.EqualTo(2));
    }
}
