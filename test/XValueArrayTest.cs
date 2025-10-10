using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XValueArrayTest
{
    [Test]
    public void int_array_test()
    {
        var arr = new[] { 1, 2, 3, 4 };
        Assert.That(arr.xGetSafe(0), Is.EqualTo(1));
        Assert.That(arr.xGetSafe(3), Is.EqualTo(4));
        Assert.That(arr.xGetSafe(4), Is.EqualTo(0));
    }

    [Test]
    public void guid_array_test()
    {
        var arr = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        Assert.That(arr.xGetSafe(0), Is.EqualTo(arr[0]));
        Assert.That(arr.xGetSafe(2), Is.EqualTo(arr[2]));
        Assert.That(arr.xGetSafe(3), Is.EqualTo(Guid.Empty));
    }

    [Test]
    public void class_array_test()
    {
        var arr = new[] { new TestObject(), new TestObject(), new TestObject() };
        Assert.That(arr.xGetSafe(0), Is.EqualTo(arr[0]));
        Assert.That(arr.xGetSafe(2), Is.EqualTo(arr[2]));
        Assert.That(arr.xGetSafe(3), Is.Null);
    }
}