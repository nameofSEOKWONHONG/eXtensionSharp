using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XEmtpyTest
{
    [Test]
    public void is_empty_test()
    {
        var str1 = string.Empty;
        string str2 = null;
        DateTime dt = DateTime.MinValue;
        Assert.Multiple(() =>
        {
            Assert.That(str1.xIsEmpty(), Is.True);
            Assert.That(str2.xIsEmpty(), Is.True);
            Assert.That(dt.xIsEmpty(), Is.True);
        });
    }

    [Test]
    public void is_not_empty_test()
    {
        var str1 = "test";
        DateTime dt = DateTime.Now;
        var n = 1;
        double n1 = 20.0;
        float n2 = 1.5f;
        long n3 = 20_000;
        Assert.Multiple(() =>
        {
            Assert.That(str1.xIsEmpty(), Is.False);
            Assert.That(dt.xIsEmpty(), Is.False);
            Assert.That(n.xIsEmpty(), Is.False);
            
            Assert.That(n1.xIsEmpty(), Is.False);
            Assert.That(n2.xIsEmpty(), Is.False);
            Assert.That(n3.xIsEmpty(), Is.False);
        });
    }

    [Test]
    public void collection_is_empty_test()
    {
        var list1 = new System.Collections.Generic.List<string>();
        var map = new Dictionary<string, string>();
        var hashset = new HashSet<string>();
        var hashtable = new Hashtable();
        Assert.Multiple(() =>
        {
            Assert.That(list1.xIsEmpty(), Is.True);
            Assert.That(map.xIsEmpty(), Is.True);
            Assert.That(hashset.xIsEmpty(), Is.True);
            Assert.That(hashtable.xIsEmpty(), Is.True);
        });
        
        list1.Add(string.Empty);
        map.Add("A", "A");
        hashset.Add("test");
        hashtable.Add("A", "A");
    }

    [Test]
    public void collection_is_not_empty_test()
    {
        var list1 = new System.Collections.Generic.List<string>();
        var map = new Dictionary<string, string>();
        var hashset = new HashSet<string>();
        var hashtable = new Hashtable();
        
        list1.Add(string.Empty);
        map.Add("A", "A");
        hashset.Add("test");
        hashtable.Add("A", "A");
        
        Assert.Multiple(() =>
        {
            Assert.That(list1.xIsEmpty(), Is.False);
            Assert.That(map.xIsEmpty(), Is.False);
            Assert.That(hashset.xIsEmpty(), Is.False);
            Assert.That(hashtable.xIsEmpty(), Is.False);
        });
    }  
}