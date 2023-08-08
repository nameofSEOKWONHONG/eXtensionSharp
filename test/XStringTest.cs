// using System;
// using NUnit.Framework;
//
// namespace eXtensionSharp.test {
//     public class XStringTest {
//         [Test]
//         public void split_test() {
//             var chars ="a,b,c,d,e".xSplit(',');
//             Assert.AreEqual(chars.xFirst(), "a");
//             Assert.AreEqual(chars.xLast(), "e");
//         }
//
//         [Test]
//         public void string_hash_test()
//         {
//             var text1 = "abcdef";
//             var text2 = "abcdef";
//
//             Assert.AreEqual(text1.xGetHashCode(), text2.xGetHashCode());
//         }
//
//         [Test]
//         public void string_find_word_test()
//         {
//             var text = "hello world";
//             var splits = text.xDistinct().ToCharArray();
//             splits.xForEach(c =>
//             {
//                 Console.Write(c);
//                 Console.WriteLine(text.xCount(c));    
//             });
//         }
//     }
// }