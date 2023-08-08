// using System;
// using NUnit.Framework;
//
// namespace eXtensionSharp.test;
//
// public class EncTest
// {
//     [Test]
//     public void Aes256Test()
//     {
//         var text = "Hello§world";
//         var key = Guid.NewGuid().ToString("N");
//         var encText = text.xToAES256Encrypt(key);
//         Console.WriteLine(encText);
//         var decText = encText.xToAES256Decrypt(key);
//         Assert.AreEqual(text, decText);
//     }
// }