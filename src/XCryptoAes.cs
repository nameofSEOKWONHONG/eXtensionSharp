using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptoAes
    {
        public static byte[] xEncAes256(this string plainText,  byte[] key, byte[] iv)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.BlockSize = 128;
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using var encryptor = aesAlg.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            return encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        }

        public static string xDecAes256(this byte[] cipherText, byte[] key, byte[] iv)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.BlockSize = 128;
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using var decryptor = aesAlg.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        
        public static string xEncAes256Gcm(this string plainText, byte[] key)
        {
            byte[] nonce = RandomNumberGenerator.GetBytes(12); // 12B 권장
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[16]; // 128-bit 인증 태그

            using var aesGcm = new AesGcm(key);
            aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // 암호화 결과: nonce + cipher + tag 순서로 연결
            byte[] result = new byte[nonce.Length + cipherBytes.Length + tag.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, nonce.Length, cipherBytes.Length);
            Buffer.BlockCopy(tag, 0, result, nonce.Length + cipherBytes.Length, tag.Length);

            return Convert.ToBase64String(result);
        }

        // Decrypt: Base64(cipherText + tag + nonce) → plainText
        public static string xDecAes256Gcm(this string base64CipherText, byte[] key)
        {
            byte[] combined = Convert.FromBase64String(base64CipherText);

            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            byte[] cipher = new byte[combined.Length - nonce.Length - tag.Length];

            Buffer.BlockCopy(combined, 0, nonce, 0, nonce.Length);
            Buffer.BlockCopy(combined, nonce.Length, cipher, 0, cipher.Length);
            Buffer.BlockCopy(combined, nonce.Length + cipher.Length, tag, 0, tag.Length);

            byte[] plainBytes = new byte[cipher.Length];
            using var aesGcm = new AesGcm(key);
            aesGcm.Decrypt(nonce, cipher, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}