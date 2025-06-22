using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace eXtensionSharp.test;

public class EncTest
{
    [Test]
    public void SHA256_test()
    {
        var text = "Hello§world";
        var encSha256 = text.xComputeHash256();
        TestContext.Out.WriteLine(encSha256);
        Assert.That(encSha256, Is.Not.Null);
    }

    [Test]
    public void Aes256_test()
    {
        string plainText = "Seokwon AES256 CBC Test";

        // 256-bit Key (32 bytes)
        byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes("4Dk1kLZdVGq+vjMvM+NciFMYcJZBg7Odq0T/8Sm4MFk="));
        
        // 128-bit IV (16 bytes)
        byte[] iv = RandomNumberGenerator.GetBytes(16);

        // 암호화
        byte[] encrypted = plainText.xEncAes256(key, iv);
        string encryptedBase64 = Convert.ToBase64String(encrypted);
        TestContext.Out.WriteLine(encryptedBase64);

        // 복호화
        string decrypted = encrypted.xDecAes256(key, iv);
        TestContext.Out.WriteLine(decrypted);

        Assert.That(decrypted == plainText, Is.True);
    }
    
    [Test]
    public void Aes256Gcm_test()
    {
        string text = "Hello, world!";
        byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes("4Dk1kLZdVGq+vjMvM+NciFMYcJZBg7Odq0T/8Sm4MFk=")); // 256-bit key

        // 암호화
        var result = text.xEncAes256Gcm(key);
        TestContext.Out.WriteLine(result);

        // 복호화
        string decrypted = result.xDecAes256Gcm(key);
        TestContext.Out.WriteLine(decrypted);

        Assert.That(decrypted == text, Is.True);
    }
}