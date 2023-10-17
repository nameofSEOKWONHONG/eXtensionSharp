using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XStringTest {
        [Test]
        public void split_test() {
            var chars ="a,b,c,d,e".xSplit(",");
            Assert.AreEqual(chars.xFirst(), "a");
            Assert.AreEqual(chars.xLast(), "e");
        }

        [Test]
        public void string_hash_test()
        {
            var text1 = "abcdef";
            var text2 = "abcdef";

            Assert.AreEqual(text1.xGetHashCode(), text2.xGetHashCode());
        }

        [Test]
        public void string_find_word_test()
        {
            var text = "hello world";
            var splits = text.xDistinct().ToCharArray();
            splits.xForEach(c =>
            {
                Console.Write(c);
                Console.WriteLine(text.xCount(c));    
            });
        }

        [Test]
        public void hidden_case_test()
        {
            var name = "아무개";
            var expected = "아*개";
            var result = name.xHiddenText('*', 1);
            Assert.AreEqual(expected, result);

            expected = "아**";
            result = name.xHiddenText('*', 1, 1);
            Assert.AreEqual(expected, result);

            var name2 = "John Down";
            var expected2 = "J********";
            var result2 = name2.xHiddenText('*', 1, 7);
            Assert.AreEqual(expected2, result2);

        }

        [Test]
        public void jsonnode_to_value_test()
        {
            using var doc = JsonDocument.Parse(
                "[\n    {\n        \"Name\" : \"Test 2\",\n        \"NumberOfComponents\" : 1,\n        \"IsActive\" : true,\n        \"CreatedBy\" : \"bsharma\"\n    },\n    {\n        \"Name\" : \"Test 2\",\n        \"NumberOfComponents\" : 1,\n        \"IsActive\" : true,\n        \"CreatedBy\" : \"bsharma\"\n    }\n]");
            
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                TestContext.Out.WriteLine(element.GetProperty("Name").GetString());
            }
        }
    }
}