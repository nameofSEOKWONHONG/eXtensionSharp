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
            Assert.IsTrue(a.xIsEquals("A"));
        }

        [Test]
        public void string_collection_match() {
            var alist = new string[] {"A", "B", "C"};
            Assert.IsTrue(alist.xIsEquals("A"));
        }

        [Test]
        public void string_collection_match2() {
            var a = "B";
            Assert.IsTrue(a.xIsEquals(new[]{"A", "B", "C"}));
        }

        [Test]
        public void value_test() {
            var a = "A";
            Assert.AreEqual("".xSafe(a), a);
        }

        [Test]
        public void value_test2() {
            Assert.AreEqual(123.xSafe(), "123");
        }

        [Test]
        public void value_test3() {
            var s = DateTime.Now.xSafe();
            Console.WriteLine(s);
            Assert.AreEqual(s.GetType(), typeof(string));
        }

        [Test]
        public void value_test4() {
            var x1 = ENUM_DATE_FORMAT.DEFAULT.xSafe(ENUM_DATE_FORMAT.HHMMSS);
            var x2 = ENUM_DATE_FORMAT.DEFAULT.Value;
            Assert.AreEqual(x1, x2);

            var x3 = "HHmmss".xSafe<ENUM_DATE_FORMAT>(ENUM_DATE_FORMAT.HHMMSS);
            var x4 = ENUM_DATE_FORMAT.HHMMSS;
            Assert.AreEqual(x3, x4);
        }

        [Test]
        public void xos_test() {
            Assert.IsTrue(XOS.xIsWindows());
            Assert.IsFalse(XOS.xIsLinux());
        }
        
    }
}