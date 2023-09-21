using System;
using System.Globalization;
using NUnit.Framework;
using eXtensionSharp;

namespace eXtensionSharp.test {
    public class XObjectTest {
        [SetUp]
        public void Setup() {
            
        }

        [Test]
        public void string_match() {
            var a = "A";
            Assert.IsTrue(a.xIsSame("A"));
        }

        [Test]
        public void string_collection_match() {
            var alist = new string[] {"A", "B", "C"};
            Assert.IsTrue("A".xContains(alist));
        }

        [Test]
        public void string_collection_match2() {
            var a = "B";
            Assert.IsTrue(a.xContains(new[]{"A", "B", "C"}));
        }

        [Test]
        public void value_test() {
            // var a = "A";
            // Assert.AreEqual("".xGetValue(a), a);
        }

        [Test]
        public void value_test2() {
            // Assert.AreEqual(123.xGetValue(), "123");
        }

        [Test]
        public void value_test3() {
            // var s = DateTime.Now.xGetValue();
            // Console.WriteLine(s);
            // Assert.AreEqual(s.GetType(), typeof(string));
        }

        [Test]
        public void value_test4() {
            // var x1 = ENUM_DATE_FORMAT.DEFAULT(ENUM_DATE_FORMAT.HHMMSS);
            // var x2 = ENUM_DATE_FORMAT.DEFAULT.ToString();
            // Assert.AreEqual(x1, x2);
            //
            // var x3 = "HHmmss".xGetValue<ENUM_DATE_FORMAT>(ENUM_DATE_FORMAT.HHMMSS);
            // var x4 = ENUM_DATE_FORMAT.HHMMSS;
            // Assert.AreEqual(x3, x4);
        }

        [Test]
        public void xos_test() {
            Assert.IsTrue(XEnvExtensions.xIsWindows());
            Assert.IsFalse(XEnvExtensions.xIsLinux());
        }

        [Test]
        public void object_to_dictionary_test()
        {
            var test = new Test();
            test.Id = 1;
            test.Name = "test";
            test.Next = test;

            var result = test.xToDictionary();
            Assert.AreEqual(result["Id"], 1);
            Assert.AreEqual(result["Name"], "test");
        }
    }

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Test Next { get; set; }
    }
}