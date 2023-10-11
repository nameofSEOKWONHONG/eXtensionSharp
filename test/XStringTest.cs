using System;
using NUnit.Framework;

namespace eXtensionSharp.test {
    public class XStringTest {
        [Test]
        public void split_test() {
            var chars ="a,b,c,d,e".xToSplit(",");
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
    }
}