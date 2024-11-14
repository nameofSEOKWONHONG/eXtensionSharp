using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace eXtensionSharp.test;

public class XSerializeTest
{
    public static string _data = @"
{
    ""id"": 1,
    ""city"": ""Georgia"",
    ""date"": ""2024-05-18T14:57:38.907"",
    ""summary"": ""test"",
    ""temperatureC"": 39,
    ""temperatureF"": 102,
    ""createdName"": ""68B38E6A793298C6251DCD0C73BE5712"",
    ""lastModifiedName"": """",
    ""mTag"": null
}
";

    [Test]
    public void from_deserialize_test()
    {
        var converted = _data.xDeserialize<WeatherForecast>();
        converted.xSerialize();
        Assert.That(converted, Is.Not.Null);
        Assert.That(converted.Id, Is.EqualTo(1));
    }

    [Test]
    public void from_deserialize_to_serialize_test()
    {
        var converted = _data.xDeserialize<WeatherForecast>();
        var json = converted.xSerialize();
        Assert.Multiple(() =>
        {
            Assert.That(json, Is.Not.Empty);
            Assert.That(json, Does.Contain("Georgia"));
        });
    }

    [Test]
    public void to_serialize_test()
    {
        var items = new List<WeatherForecast>();
        items.Add(new WeatherForecast() { Id = 1, City = "Georgia", Date = DateTime.Now, Summary = "test", TemperatureC  = 32});
        items.Add(new WeatherForecast() { Id = 2, City = "Georgia", Date = DateTime.Now, Summary = "test", TemperatureC  = 32});
        var json = items.xSerialize();
        Assert.That(json, Is.Not.Empty);
        Assert.That(json, Does.Contain("Summary"));
    }
}

public class WeatherForecast
{
    public long Id { get; set; }
    public string City { get; set; }
    public DateTime? Date { get; set; }
    public string Summary { get; set; }
    public int? TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string CreatedName { get; set; }    
}
