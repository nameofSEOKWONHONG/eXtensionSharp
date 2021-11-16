using System;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptAES256
    {
        [Obsolete("no more use method", true)]
        public static string xToEncAES256(this string plainText, string cipherKey, string cipherIV,
            CipherMode cipherMode, PaddingMode paddingMode)
        {
            var byteKey = Encoding.UTF8.GetBytes(cipherKey);
            var byteIV = Encoding.UTF8.GetBytes(cipherIV);

            var strEncode = string.Empty;
            var bytePlainText = Encoding.UTF8.GetBytes(plainText);

            using (var aesCryptoProvider = new AesCryptoServiceProvider())
            {
                try
                {
                    aesCryptoProvider.Mode = cipherMode;
                    aesCryptoProvider.Padding = paddingMode;
                    var result = aesCryptoProvider.CreateEncryptor(byteKey, byteIV)
                        .TransformFinalBlock(bytePlainText, 0, bytePlainText.Length);
                    return result.fromHexToString();
                }
                finally
                {
                    aesCryptoProvider.Dispose();
                }
            }
        }

        [Obsolete("no more use method", true)]
        public static string xToDecAES256(this string cipherText, string cipherKey, string cipherIV,
            CipherMode cipherMode, PaddingMode paddingMode, DeconvertCipherFormat format)
        {
            var byteKey = Encoding.UTF8.GetBytes(cipherKey);
            var byteIV = Encoding.UTF8.GetBytes(cipherIV);
            var byteBuff = cipherText.xToHMAC(format);
            using (var aesCrytoProvider = new AesCryptoServiceProvider())
            {
                aesCrytoProvider.Mode = cipherMode;
                aesCrytoProvider.Padding = paddingMode;
                var dec = aesCrytoProvider.CreateDecryptor(byteKey, byteIV)
                    .TransformFinalBlock(byteBuff, 0, byteBuff.Length);
                return Encoding.UTF8.GetString(dec);
            }
        }
    }
}