using System;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptHMAC
    {
        public static string xToHMAC(this string encData, string encKey,
            DeconvertCipherFormat deconvertCipherFormat)
        {
            var encoding = new UTF8Encoding();
            var keyBuff = encoding.GetBytes(encKey);
            byte[] hashMessage = null;

            using (var hmacsha256 = new HMACSHA256(keyBuff))
            {
                var dataBytes = encoding.GetBytes(encData);
                hashMessage = hmacsha256.ComputeHash(dataBytes);
            }

            return hashMessage.fromHexToString();
        }

        public static byte[] xToHMAC(this string cipherText, DeconvertCipherFormat outputFormat)
        {
            byte[] decodeText = null;
            switch (outputFormat)
            {
                case DeconvertCipherFormat.HEX:
                    decodeText = fromHexToByte(cipherText);
                    break;

                case DeconvertCipherFormat.Base64:
                    decodeText = Convert.FromBase64String(cipherText);
                    break;

                default:
                    throw new Exception("not implement exception.");
            }

            return decodeText;
        }

        internal static byte[] fromHexToByte(this string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            bytes.xForEach((i, v) => bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16));
            return bytes;
        }

        internal static string fromHexToString(this byte[] hashMessage)
        {
            var sbinary = string.Empty;
            hashMessage.xForEach((i, v) => sbinary += v.ToString("X2"));
            return sbinary;
        }
    }

    public enum DeconvertCipherFormat
    {
        Base64,
        HEX
    }
}