using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XIsIfExtensionTest
{
    [Test]
    public void xis_if_test()
    {
        var expected = false;
        const string a = "a";
        a.xIf(m => m == a, 
            () => Assert.That(expected, Is.True), 
            () => Assert.That(expected, Is.False));
    }

    [Test]
    public void xis_if_test2()
    {
        var a = 1;
        var result = 0;
        a.xIf(m => m > 2, 
            () => result = 1, 
            () => result = 2);
        
        Assert.That(a, Is.Not.EqualTo(result));
    }
}