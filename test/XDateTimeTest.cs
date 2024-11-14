using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XDateTimeTest
{
    [Test]
    public void date_from_to_test()
    {
        var from = DateTime.Now.AddDays(-10);
        var to = DateTime.Now;
        var v = new ValueTuple<DateTime, DateTime>(from, to);
        var list = new List<DateTime>();
        v.xForEach(dt =>
        {
            list.Add(dt);
        });
        
        Assert.That(list.Count, Is.EqualTo(11));
    }
    
    [Test]
    public void string_to_datetime_test()
    {
        var date = "2022-01-02";
        var convertedDate = date.xConvertToDate();
        Assert.That(convertedDate, Is.Not.Null);
        Assert.That($"{convertedDate.Value.xToYear()}-{convertedDate.Value.xToMonth()}-{convertedDate.Value.xToDay()}", Is.EqualTo(date));
    }

    [Test]
    public void datetime_from_to_test()
    {
        var items = new Dictionary<int, DateTime>();
        Enumerable.Range(1, 100).ToList().ForEach(i =>
        {
            items.Add(i, DateTime.Now.AddDays(i));
        });

        var now = DateTime.Now;
        var from = now.xFromDate().AddDays(1);
        var to = now.xToDate().AddDays(10);
        var selectedItems = items.Where(m => m.Value >= from && m.Value < to).ToList();
        Assert.That(selectedItems.Count, Is.EqualTo(10));
    }

    [Test]
    public void dateformat_culture_test()
    {
        var date = DateTime.Now;
        var convertedDate = date.xToDateFormat(new CultureInfo("en-US"), "m");
        TestContext.WriteLine(convertedDate);

        TestContext.WriteLine(date.xToDayOfWeek());
    }
}