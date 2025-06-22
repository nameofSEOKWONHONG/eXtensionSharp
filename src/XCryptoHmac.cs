using System;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptoHmac
    {
        public static string xHmacEncode(this string input, string key, DeconvertCipherFormat format)
        {
            var encoding = Encoding.UTF8;
            var keyBytes = encoding.GetBytes(key);
            var dataBytes = encoding.GetBytes(input);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(dataBytes);
            return hash.ToHexString();
        }

        public static byte[] xHmacDecode(this string input, DeconvertCipherFormat format) => format switch
        {
            DeconvertCipherFormat.HEX => input.FromHexString(),
            DeconvertCipherFormat.Base64 => Convert.FromBase64String(input),
            _ => throw new NotSupportedException("Unsupported format")
        };


        internal static byte[] FromHexString(this string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }


        internal static string ToHexString(this byte[] data)
        {
            var sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }

    public enum DeconvertCipherFormat
    {
        Base64,
        HEX
    }
}