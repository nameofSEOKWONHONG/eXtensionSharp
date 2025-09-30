using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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

        var c = 0.1f;
        Assert.That(c.xValue<int>(), Is.EqualTo(0));
    }

    [Test]
    public void xis_number_test()
    {
        var s = "10";
        var s1 = "0f";
        Assert.That(s.xIsNumber(), Is.True);
        Assert.That(s1.xIsNumber(), Is.False);
    }
    
    [Test] 
    public void guid_test() 
    {
        Guid? guid = Guid.NewGuid();
        Assert.That(guid.xValue<Guid>(), Is.EqualTo(guid));

        var guid2 = Guid.NewGuid().ToString();
        Assert.That(guid2.xValue<Guid>(), Is.EqualTo(Guid.Parse(guid2)));
    }

    [Test]
    public void object_to_class_test()
    {
        object test = new Test() {Name = "test"};
        var test2 = test.xValue<Test>();
        Assert.That(test2.Name, Is.EqualTo("test"));
        
        var test3 = new Test2() {Name = "test", Age = "10"};
        var test4 = test3.xAs<Test>();
        Assert.That(test4.Name, Is.EqualTo("test"));
    }

    [Test]
    public void array_to_safe_value()
    {
        var array1 = new int[] {1, 2, 3};
        var real = array1.xValueOfArray(0);
        Assert.That(real, Is.EqualTo(1));
        
        var array2 = new string[] {"1", "2", "3"};
        var real2 = array2.xValueOfArray(0);
        Assert.That(real2, Is.EqualTo("1"));

        var array3 = Array.Empty<string>();
        var real3 = array3.xValueOfArray(0);
        Assert.That(real3, Is.Null);
        
        var array4 = new Test[] {new Test() {Name = "test"}, new Test() {Name = "test2"}};
        var real4 = array4.xValueOfArray(0);
        Assert.That(real4.Name, Is.EqualTo("test"));

        var real5 = array4.xValueOfArray(3);
        Assert.That(real5, Is.Null);
    }

    [Test]
    public void string_to_int_test()
    {
        var str = "123123";
        Assert.That(str.xValue<int>(), Is.EqualTo(123123));
    }

    [Test]
    public void int_to_string_test()
    {
        var number = 123123;
        Assert.That(number.xValue<string>(), Is.EqualTo("123123"));
    }

    class Test
    {
        public string Name { get; set; }
    }

    class Test2 : Test
    {
        public string Age { get; set; }
    }
}

