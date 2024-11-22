using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptionAes256
    {
        public static string xToEncAES(this string plainText, string cipherKey, string cipherIV,
            CipherMode cipherMode, PaddingMode paddingMode)
        {
            var byteKey = Encoding.UTF8.GetBytes(cipherKey);
            var byteIV = Encoding.UTF8.GetBytes(cipherIV);

            var strEncode = string.Empty;
            var bytePlainText = Encoding.UTF8.GetBytes(plainText);

            using (var aesCryptoProvider = Aes.Create())
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

        public static string xToDecAES(this string cipherText, string cipherKey, string cipherIV,
            CipherMode cipherMode, PaddingMode paddingMode, DeconvertCipherFormat format)
        {
            var byteKey = Encoding.UTF8.GetBytes(cipherKey);
            var byteIV = Encoding.UTF8.GetBytes(cipherIV);
            var byteBuff = cipherText.xToHMAC(format);
            using (var aesCryptoProvider = Aes.Create())
            {
                aesCryptoProvider.Mode = cipherMode;
                aesCryptoProvider.Padding = paddingMode;
                var dec = aesCryptoProvider.CreateDecryptor(byteKey, byteIV)
                    .TransformFinalBlock(byteBuff, 0, byteBuff.Length);
                return Encoding.UTF8.GetString(dec);
            }
        }

        public static string xToEncryptString(this string plainText, [StringLength(32)] string key)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public static string xToEncryptNumber(this string encryptString, [StringLength(36)] string key)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                }, key.Length, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string xToDecryptString(this string cipherText, [StringLength(32)] string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);//I have already defined "Key" in the above EncryptString function
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string xToDecryptNumber(this string cipherText, [StringLength(36)] string key)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                }, key.Length, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        /// <summary>
        /// 고정키 암호화
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string xToAESEncrypt(this string text, string key, string iv)
        {
            return text.xToEncAES(key, iv, CipherMode.CBC, PaddingMode.PKCS7);
        }

        /// <summary>
        /// 고정키 복호화
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string xToAESDecrypt(this string text, string key, string iv)
        {
            return text.xToDecAES(key, iv, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);
        }

        public static string xToBase64(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string xFromBase64(this string input)
        {
            var bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }

        public static bool xIsBase64(this string input)
        {
            Span<byte> buffer = new Span<byte>(new byte[input.Length]);
            return Convert.TryFromBase64String(input, buffer, out int bytes);
        }

        /// <summary>
        /// base64 암호화 인코딩
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string xToEncBase64(this string str)
        {
            if (str.xIsEmpty()) return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(str);
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(0xff & (bytes[i] << 1));
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// base64 복호화 인코딩
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string xToDecBase64(this string str)
        {
            if (str.xIsEmpty()) return string.Empty;
            var bytes = Convert.FromBase64String(str);
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(0xff & (bytes[i] >> 1));
            }
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// base64 복호화 인코딩 (스트림용)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> xToDecBase64Async(this Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string base64 = await reader.ReadToEndAsync();
            return base64.xToDecBase64();
        }
    }
}