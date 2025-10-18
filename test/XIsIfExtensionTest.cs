using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XIsIfExtensionTest
{
    [Test]
    public void xif_test()
    {
        var expected = true;
        const string a = "a";
        a.xIf(m => m == "a", 
         _ => Assert.That(expected, Is.True), 
         _ => Assert.That(expected, Is.False));
    }

    [Test]
    public void xif_test2()
    {
        var a = 1;
        var result = 0;
        a.xIf(m => m > 2,
            _ => result = 1,
            _ => result = 2);

        Assert.That(a, Is.Not.EqualTo(result));
    }

    [Test]
    public void xif_test3()
    {
        TestObject obj = new TestObject() { Id = 1, Name = "test", Next = null };
        obj.xIf(m => m.Next.xIsNull(),
            m => m.Next = new TestObject() { Id = 2, Name = "next", Next = null },
            m => m.Next.Next = new TestObject() { Id = 3, Name = "else", Next = null });

        Assert.That(obj.Next.Id, Is.EqualTo(2));
        //Assert.That(obj.Next.Next.Id, Is.EqualTo(3));
    }
}