using System;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class XDateTimeTest
{
    [Test]
    public void datetime_parse_test()
    {
        var isConverted = "2022-12-31".xTryDateParse(out var date);
        Assert.That(isConverted, Is.True);
        Assert.That(DateTime.Parse("2022-12-31"), Is.EqualTo(date));
    }
    
    [Test]
    public void datetime_parse_format_test()
    {
        var format = "yyyy-MM-dd";
        var isConverted = "2022-12-31".xTryDateParse(format, out var date);
        Assert.That(isConverted, Is.True);
        Assert.That(DateTime.Parse("2022-12-31"), Is.EqualTo(date));
    }
}