﻿using System;
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

    //xisempty dose not support xisnumber
    // [Test]
    // public void value_empty_not_empty_test()
    // {
    //     var dmin = double.MinValue;
    //     Assert.IsTrue(dmin.xIsEmpty());
    //
    //     var nzero = 0;
    //     Assert.IsTrue(nzero.xIsEmpty());
    //     Assert.IsFalse(nzero.xIsNotEmpty());
    // }

    [Test]
    public void number_is_or_test()
    {
        var n = 4;
        Assert.Multiple(() =>
        {
            //Assert.That(n.xIsNumber(), Is.True);
            Assert.That(n.xValue<Int32>() == 4, Is.True);
        });
    }

    [Test]
    public void duplicate_test()
    {
        var a1 = new[] { "1", "3", "2", "3", "11" };

        if (a1.xTryDuplicate(out var d))
        {
            TestContext.Out.WriteLine(d);
            Assert.That(d, Is.EqualTo("3"));
        }
    }

    [Test]
    public void xvalue_number_convert_test()
    {
        int a = 32;
        var r = a.xValue<Int64>();
        Assert.That(a.xValue<Int64>(), Is.GreaterThan(0));
    }

    [Test]
    public void xvalue_number_convert_test2()
    {
        var a = int.MaxValue;
        var r = a.xValue<double>();
        Assert.That(a.xValue<double>(), Is.GreaterThan(0));

        var b = float.MaxValue;
        var b2 = float.MinValue;
        Assert.That(b.xValue<double>(), Is.GreaterThan(0));
    }

    [Test]
    public void xis_number_test()
    {
        var s = "10";
        var s1 = "0f";
        Assert.That(s.xIsNumber(), Is.True);
        Assert.That(s1.xIsNumber(), Is.False);
    }
}