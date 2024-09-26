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
        var a = "a";
        a.xIf("b", () => Assert.AreEqual(expected, true), () => Assert.AreEqual(expected, false));
    }

    [Test]
    public void xis_if_test2()
    {
        var expected = false;
        var a = 1;
        var result = 0;
        a.xIf(2, () => result = 1, () => result = 2);
        Assert.AreNotEqual(a, result);
    }
}