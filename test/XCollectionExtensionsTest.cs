using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace eXtensionSharp.test;

// Sample models for mapping tests
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ReadOnlyMirror => Age; // getter only
}

public class PersonDto
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public DateTime? CreatedAt { get; set; }
}
    

public class XCollectionExtensionsTest
{
    [Test]
    public void dictionary_performance_improvement_test1()
    {
        var values = new Dictionary<string, int>
        {
            { "test", 12345 }
        };
        var val = values.xGetOrAdd("test1", 1827387);
        Assert.That(val, Is.EqualTo(1827387));

        var updated = values.xTryUpdate("test1", 0);
        Assert.That(updated, Is.True);

        var json = values.xSerialize();
        Assert.That(json, Is.EqualTo(@"{""test"":12345,""test1"":0}"));
    }

    [Test]
    public void xValueChange_CopiesWritableProperties_Only()
    {
        var src = new Person { Name = "A", Age = 10, CreatedAt = new DateTime(2024, 1, 1) };
        var dest = new Person { Name = "B", Age = 99, CreatedAt = new DateTime(2000, 1, 1) };

        src.xValueChange(dest);

        Assert.That(dest.Name, Is.EqualTo("A"));
        Assert.That(dest.Age, Is.EqualTo(10));
        Assert.That(dest.CreatedAt, Is.EqualTo(new DateTime(2024, 1, 1)));
        Assert.That(dest.ReadOnlyMirror, Is.EqualTo(10));
    }

    [Test]
    public void xMapping_T1T1_MapsPrimitiveLikeTypes_And_SkipsExclusions()
    {
        var src  = new Person { Name = "A", Age = 10, CreatedAt = new DateTime(2024, 1, 1) };
        var dest = new Person { Name = "B", Age = 99, CreatedAt = new DateTime(2000, 1, 1) };

        src.xMapping<Person, Person>(dest, new[] { nameof(Person.Age) });

        Assert.That(dest.Name, Is.EqualTo("A"));
        Assert.That(dest.Age, Is.EqualTo(99)); // excluded
        Assert.That(dest.CreatedAt, Is.EqualTo(new DateTime(2024, 1, 1)));
    }

    [Test]
    public void xMapping_T1T2_MapsByMatchingNames()
    {
        var src  = new Person { Name = "A", Age = 10, CreatedAt = new DateTime(2024, 1, 1) };
        var dest = new PersonDto { Name = "B", Age = 99, CreatedAt = null };

        src.xMapping<Person, PersonDto>(dest);

        Assert.That(dest.Name, Is.EqualTo("A"));
        Assert.That(dest.Age, Is.EqualTo(10));
        Assert.That(dest.CreatedAt, Is.EqualTo(new DateTime(2024, 1, 1)));
    }

    [Test]
    public void xToDictionary_ConvertsObjectToDictionary_IncludingReadOnlyProperties()
    {
        var p = new Person { Name = "A", Age = 10, CreatedAt = new DateTime(2024, 1, 1) };

        var dict = p.xToDictionary();

        Assert.That(dict["Name"], Is.EqualTo("A"));
        Assert.That(dict["Age"], Is.EqualTo(10));
        Assert.That(dict["CreatedAt"], Is.EqualTo(new DateTime(2024, 1, 1)));
        Assert.That(dict.ContainsKey(nameof(Person.ReadOnlyMirror)), Is.True);
    }

    [Test]
    public void xToDictionaries_ConvertsPocoSequence()
    {
        var list = new[]
        {
            new Person { Name = "A", Age = 1, CreatedAt = new DateTime(2024,1,1) },
            new Person { Name = "B", Age = 2, CreatedAt = new DateTime(2024,1,2) }
        };

        var dicts = list.xToDictionaries().ToList();

        Assert.That(dicts.Count, Is.EqualTo(2));
        Assert.That(dicts[0]["Name"], Is.EqualTo("A"));
        Assert.That(dicts[1]["Age"], Is.EqualTo(2));
    }

    [Test]
    public void xDataTableToDictionaries_ConvertsDataTableRows()
    {
        var dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Rows.Add(1, "A");
        dt.Rows.Add(2, "B");

        var dicts = dt.xDataTableToDictionaries().ToList();

        Assert.That(dicts.Count, Is.EqualTo(2));
        Assert.That(dicts[0]["Id"], Is.EqualTo(1));
        Assert.That(dicts[1]["Name"], Is.EqualTo("B"));
    }

    [Test]
    public void xIsBetween_Number_IsInclusive()
    {
        Assert.That(5.xIsBetween(1, 10), Is.True);
        Assert.That(0.xIsBetween(1, 10), Is.False);
    }

    [Test]
    public void xStringToBytes_ProducesJsonSerializedBytes()
    {
        var s = "hello";
        var bytes = s.xStringToBytes();

        var json = Encoding.UTF8.GetString(bytes);
        Assert.That(json, Is.EqualTo("\"hello\""));
    }

    [Test]
    public void xGetOrAdd_ReturnsExistingOrAddsNew()
    {
        var dict = new Dictionary<string, int>();
        var v1 = dict.xGetOrAdd("A", 1);
        var v2 = dict.xGetOrAdd("A", 999);

        Assert.That(v1, Is.EqualTo(1));
        Assert.That(v2, Is.EqualTo(1));
        Assert.That(dict["A"], Is.EqualTo(1));
    }
}