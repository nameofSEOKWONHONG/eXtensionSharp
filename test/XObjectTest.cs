using NUnit.Framework;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace eXtensionSharp.test {
    public class XObjectTest {
        [SetUp]
        public void Setup() {
            
        }

        [Test]
        public void string_match() {
            var a = "A";
            Assert.That(a.xIsSame("A"), Is.True);
        }

        [Test]
        public void string_collection_match() {
            var alist = new string[] {"A", "B", "C"};
            Assert.That("A".xContains(alist), Is.True);
        }

        [Test]
        public void string_collection_match2() {
            var a = "B";
            Assert.That(a.xContains(["A", "B", "C"]), Is.True);
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

        // [Test]
        // public void xos_test() {
        //     Assert.IsTrue(XEnvExtensions.xIsWindows());
        //     Assert.IsFalse(XEnvExtensions.xIsLinux());
        // }

        [Test]
        public void class_to_dictionary_test()
        {
            var test = new Test();
            test.Id = 1;
            test.Name = "test";
            test.Next = test;

            var result = test.xToDictionary();
            Assert.That(result["Id"], Is.EqualTo(1));
            Assert.That(result["Name"], Is.EqualTo("test"));

            var list = new List<Test>();
            list.Add(test);
            var maps = list.xToDictionaries().ToList();
            Assert.That(maps[0]["Name"].xValue<string>(), Is.EqualTo("test"));
        }

        [Test]
        public void dynamic_list_to_dynamic_dictionary() {
            var list = new List<ExpandoObject>();
            dynamic eo = new ExpandoObject();
            eo.Name = "test";
            eo.Age = 10;
            eo.Blood = "B";
            list.Add(eo);

            var dylist = list.xToDictionaries().ToList();
            Assert.Multiple(() =>
            {
                Assert.That(dylist[0]["Name"].xValue<string>(), Is.EqualTo("test"));
                Assert.That(dylist[0]["Age"].xValue<int>(), Is.EqualTo(10));
            });
        }
    }
    

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Test Next { get; set; }
    }
}