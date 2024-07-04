using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XStringTest {
        [Test]
        public void split_test() {
            var chars ="a,b,c,d,e".xSplit(",");
            Assert.That("a", Is.EqualTo(chars.xFirst()));
            Assert.That("e", Is.EqualTo(chars.xLast()));
        }

        [Test]
        public void string_hash_test()
        {
            var text1 = "abcdef";
            var text2 = "abcdef";

            Assert.That(text2.xGetHashCode(), Is.EqualTo(text1.xGetHashCode()));
        }

        [Test]
        public void string_find_word_test()
        {
            var text = "hello world";
            var expected = "helo wrd";
            var distinct = text.xDistinct();

            Assert.That(distinct, Is.EqualTo(expected));
        }

        [Test]
        public void hidden_case_test()
        {
            var name = "아무개";
            var expected = "아*개";
            var result = name.xReplace('*', 1);
            Assert.That(result, Is.EqualTo(expected));

            expected = "아**";
            result = name.xReplace('*', 1, 1);
            Assert.That(result, Is.EqualTo(expected));

            var name2 = "John Down";
            var expected2 = "J********";
            var result2 = name2.xReplace('*', 1, 7);
            Assert.That(result2, Is.EqualTo(expected2));

        }

        [Test]
        public void jsonnode_to_value_test()
        {
            using var doc = JsonDocument.Parse(
                "[\n    {\n        \"Name\" : \"Test 2\",\n        \"NumberOfComponents\" : 1,\n        \"IsActive\" : true,\n        \"CreatedBy\" : \"bsharma\"\n    },\n    {\n        \"Name\" : \"Test 2\",\n        \"NumberOfComponents\" : 1,\n        \"IsActive\" : true,\n        \"CreatedBy\" : \"bsharma\"\n    }\n]");

            var expected = "Test 2";
            var selectedValue = string.Empty;
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                if(element.GetProperty("Name").GetString() == expected)
                {
                    selectedValue = element.GetProperty("Name").GetString();
                }
            }

            Assert.That(selectedValue, Is.EqualTo(expected));
        }

        [Test]
        public void string_compress_test()
        {
			string text = "To work with BenchmarkDotNet you must install the BenchmarkDotNet package. " +
			"You can do this either via the NuGet Package Manager inside the Visual Studio 2019 IDE, " +
			"or by executing the Install-Package BenchmarkDotNet command at the NuGet Package Manager Console";
			var bytes = text.xCompress( System.IO.Compression.CompressionLevel.Optimal);
            var decompressTest = bytes.xUnCompress();

			TestContext.WriteLine(text.Length);
			TestContext.WriteLine(bytes.Length);
			
			Assert.That(text, Is.EqualTo(decompressTest));
        }

        [Test]
        public async Task string_compress_async_test()
        {
			string text = "To work with BenchmarkDotNet you must install the BenchmarkDotNet package. " +
			"You can do this either via the NuGet Package Manager inside the Visual Studio 2019 IDE, " +
			"or by executing the Install-Package BenchmarkDotNet command at the NuGet Package Manager Console";
			var bytes = await text.xCompressAsync(System.IO.Compression.CompressionLevel.Optimal);
			var decompressTest = await bytes.xUnCompressAsync();

			TestContext.WriteLine(text.Length);
			TestContext.WriteLine(bytes.Length);

			Assert.That(text, Is.EqualTo(decompressTest));
		}



        [Test]
        public void xjoin_test()
        {
            var array = new[] { "hello", "world" };
            var str = array.xJoin();
            Assert.That(str, Is.EqualTo("hello,world"));

            var empty_array = Array.Empty<string>();
            var empty_str = string.Empty;
            Assert.That(empty_str, Is.EqualTo(string.Empty));

        }
    }
}