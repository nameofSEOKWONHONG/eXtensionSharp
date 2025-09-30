// using System;
// using System.ComponentModel;
// using System.Globalization;
// using System.Text.Json;
// using NUnit.Framework;

// namespace eXtensionSharp.test
// {
//     public class XValueV2Test
//     {
//         // 공용 옵션: 문화권/숫자 스타일 등
//         private readonly ConvertOptions _optionsInvariant = new()
//         {
//             Culture = CultureInfo.InvariantCulture,
//             NumberStyles = NumberStyles.Float | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
//             EmptyStringIsNull = true
//         };

//         private readonly ConvertOptions _optionsKr = new()
//         {
//             Culture = new CultureInfo("ko-KR"), // 1.234,56 형태
//             NumberStyles = NumberStyles.Float | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
//             EmptyStringIsNull = false
//         };

//         #region Nullable & 기본값 Fallback

//         [Test]
//         public void Nullable_int_from_empty_string_returns_null_when_EmptyStringIsNull_true()
//         {
//             string s = "   ";
//             int? v = s.xValue<int?>(options: _optionsInvariant);
//             Assert.That(v, Is.Null);
//         }

//         [Test]
//         public void Fallback_default_is_used_when_conversion_fails()
//         {
//             string s = "not-a-number";
//             var v = s.xValue<int>(@default: 8080, options: _optionsInvariant);
//             Assert.That(v, Is.EqualTo(8080));
//         }

//         [Test]
//         public void DBNull_becomes_defaultT()
//         {
//             object dbnull = DBNull.Value;
//             var v = dbnull.xValue<int>();
//             Assert.That(v, Is.EqualTo(default(int)));
//         }

//         #endregion

//         #region 숫자 & 문화권

//         [TestCase("1,234.56", 1234.56, TestName = "Invariant_decimal_parse")]
//         public void Number_parse_invariant_decimal(string s, double expected)
//         {
//             var v = s.xValue<double>(options: _optionsInvariant);
//             Assert.That(v, Is.EqualTo(expected).Within(1e-9));
//         }

//         [TestCase("1,234", 1234, TestName = "ko-KR_decimal_parse")]
//         public void Number_parse_de_decimal(string s, double expected)
//         {
//             var v = s.xValue<double>(options: _optionsKr);
//             Assert.That(v, Is.EqualTo(expected).Within(1e-9));
//         }

//         [Test]
//         public void Numeric_cross_widening()
//         {
//             int a = int.MaxValue;
//             var d = a.xValue<double>(options: _optionsInvariant);
//             Assert.That(d, Is.GreaterThan(0d));
//         }

//         #endregion

//         #region Boolean (다형 입력)

//         [TestCase("true", true)]
//         [TestCase("false", false)]
//         [TestCase("Y", true)]
//         [TestCase("N", false)]
//         [TestCase("1", true)]
//         [TestCase("0", false)]
//         [TestCase("on", true)]
//         [TestCase("off", false)]
//         public void Boolean_polymorphic_inputs(string s, bool expected)
//         {
//             var b = s.xValue<bool>(options: _optionsInvariant);
//             Assert.That(b, Is.EqualTo(expected));
//         }

//         [TestCase(1, true)]
//         [TestCase(0, false)]
//         [TestCase(2, true)]
//         public void Boolean_from_numeric(object src, bool expected)
//         {
//             var b = src.xValue<bool>(options: _optionsInvariant);
//             Assert.That(b, Is.EqualTo(expected));
//         }

//         #endregion

//         #region Guid

//         [Test]
//         public void Guid_from_string_roundtrip()
//         {
//             var g = Guid.NewGuid();
//             var g2 = g.ToString().xValue<Guid>();
//             Assert.That(g2, Is.EqualTo(g));
//         }

//         [Test]
//         public void Guid_invalid_uses_default()
//         {
//             var g = "not-a-guid".xValue<Guid>(@default: Guid.Empty);
//             Assert.That(g, Is.EqualTo(Guid.Empty));
//         }

//         #endregion

//         #region Enum & Flags

//         [Flags]
//         private enum FilePerm
//         {
//             None = 0,
//             Read = 1,
//             Write = 2,
//             Execute = 4
//         }

//         private enum Color2
//         {
//             Red = 1, Green = 2, Blue = 3
//         }

//         [Test]
//         public void Enum_from_name_ignore_case()
//         {
//             var c = "green".xValue<Color2>();
//             Assert.That(c, Is.EqualTo(Color2.Green));
//         }

//         [Test]
//         public void Enum_from_numeric_string()
//         {
//             var c = "3".xValue<Color2>();
//             Assert.That(c, Is.EqualTo(Color2.Blue));
//         }

//         [Test]
//         public void Flags_enum_from_numeric()
//         {
//             var p = "3".xValue<FilePerm>(); // Read(1) | Write(2)
//             Assert.That(p.HasFlag(FilePerm.Read));
//             Assert.That(p.HasFlag(FilePerm.Write));
//             Assert.That(!p.HasFlag(FilePerm.Execute));
//         }

//         #endregion

//         #region DateTime / DateTimeOffset / TimeSpan

//         [TestCase("2025-08-27 13:05:00")]
//         [TestCase("2025-08-27T13:05:00Z")]
//         public void DateTime_parses_common_formats(string s)
//         {
//             var dt = s.xValue<DateTime>(options: new ConvertOptions
//             {
//                 Culture = CultureInfo.InvariantCulture,
//                 DateTimeFormats = new[] { "yyyy-MM-dd HH:mm:ss", "O", "s" }
//             });
//             Assert.That(dt, Is.Not.EqualTo(default(DateTime)));
//         }

//         [TestCase("2025-08-27T13:05:00+09:00")]
//         public void DateTimeOffset_parses_iso(string s)
//         {
//             var dto = s.xValue<DateTimeOffset>(options: _optionsInvariant);
//             Assert.That(dto.Offset, Is.EqualTo(TimeSpan.FromHours(9)));
//         }

//         [TestCase("01:30:00", 5400)]
//         [TestCase("1.01:00:00", 90000)] // 1 day + 1 hour
//         public void TimeSpan_parses_multiple_formats(string s, int expectedSeconds)
//         {
//             var ts = s.xValue<TimeSpan>(options: new ConvertOptions
//             {
//                 Culture = CultureInfo.InvariantCulture,
//                 TimeSpanFormats = new[] { "c", @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }
//             });
//             Assert.That((int)ts.TotalSeconds, Is.EqualTo(expectedSeconds));
//         }

//         #endregion

//         #region JsonElement 입력

//         [Test]
//         public void JsonElement_number_to_int()
//         {
//             var je = JsonSerializer.SerializeToElement(123);
//             var v = je.xValue<int>();
//             Assert.That(v, Is.EqualTo(123));
//         }

//         [Test]
//         public void JsonElement_string_guid()
//         {
//             var g = Guid.NewGuid();
//             var je = JsonSerializer.SerializeToElement(g.ToString());
//             var v = je.xValue<Guid>();
//             Assert.That(v, Is.EqualTo(g));
//         }

//         [Test]
//         public void JsonElement_complex_to_poco_via_deserialize()
//         {
//             var je = JsonSerializer.SerializeToElement(new { Name = "abc", Age = 10 });
//             var p = je.xValue<Person>();
//             Assert.That(p, Is.Not.Null);
//             Assert.That(p.Name, Is.EqualTo("abc"));
//             Assert.That(p.Age, Is.EqualTo(10));
//         }

//         public class Person
//         {
//             public string Name { get; set; }
//             public int Age { get; set; }
//         }

//         #endregion

//         #region TypeConverter 기반 타입

//         [TypeConverter(typeof(UriTypeConverter))]
//         private class UriWrapper
//         {
//             public Uri Value { get; }
//             public UriWrapper(Uri value) => Value = value;
//             public static implicit operator Uri(UriWrapper w) => w.Value;
//             public static implicit operator UriWrapper(Uri u) => new(u);
//         }

//         [Test]
//         public void Uri_converter_via_TypeConverter()
//         {
//             var u = "https://example.com".xValue<Uri>();
//             Assert.That(u, Is.Not.Null);
//             Assert.That(u.Host, Is.EqualTo("example.com"));
//         }

//         #endregion

//         #region Same-type & No-op

//         [Test]
//         public void Same_type_returns_as_is()
//         {
//             var now = DateTime.Now;
//             var v = now.xValue<DateTime>();
//             Assert.That(v, Is.EqualTo(now));
//         }

//         #endregion

//         [Test]
//         public void string_null_or_empty_default_value_test()
//         {
//             string a = null;
//             var actual = a.xValue<string>(string.Empty);
//             Assert.That(actual, Is.EqualTo(string.Empty));
//         }

//         [Test]
//         public void xvalue_typename_to_value_test()
//         {
//             var a = "1234567890".xValueWithTypeName(nameof(Int32));
//             Assert.That(a, Is.EqualTo(1234567890));
            
//             var b = "2025-08-27T13:05:00+09:00".xValueWithTypeName(nameof(DateTimeOffset));
//             Assert.That(b, Is.EqualTo(new DateTimeOffset(2025, 8, 27, 13, 5, 0, TimeSpan.FromHours(9))));
            
//             var c = "01:30:00".xValueWithTypeName(nameof(TimeSpan));
//             Assert.That(c, Is.EqualTo(new TimeSpan(1, 30, 0)));
            
//             var f = "1234567890".xValueWithTypeName(nameof(Int64));
//             Assert.That(f, Is.EqualTo(1234567890));
            
//             TestContext.Out.WriteLine(JsonSerializer.Serialize(new
//             {
//                 a,
//                 b,
//                 c,
//                 f
//             }));
//         }
//     }
// }
