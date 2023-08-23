using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptAES256
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
            using (var aesCrytoProvider = Aes.Create())
            {
                aesCrytoProvider.Mode = cipherMode;
                aesCrytoProvider.Padding = paddingMode;
                var dec = aesCrytoProvider.CreateDecryptor(byteKey, byteIV)
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
        /// SHA512 암호화, 블레이저 지원 안함.
        /// HASH로 인하여 동키값 암호화가 지원되지 않음.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string xToSHA512Encrypt(this string text, string key)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));

            var buffer = Encoding.UTF8.GetBytes(text);
            var hash = SHA512.Create();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(buffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    var result = resultStream.ToArray();
                    var combined = new byte[aes.IV.Length + result.Length];
                    Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                    Array.ConstrainedCopy(result, 0, combined, aes.IV.Length, result.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        /// <summary>
        /// SHA512 복호화, 블레이저 지원 안함.
        /// HASH로 인하여 동일값 암호화가 지원되지 않음.
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string xToSHA512Decrypt(this string encryptedText, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            var combined = Convert.FromBase64String(encryptedText);
            var buffer = new byte[combined.Length];
            var hash = SHA512.Create();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                var iv = new byte[aes.IV.Length];
                var ciphertext = new byte[buffer.Length - iv.Length];

                Array.ConstrainedCopy(combined, 0, iv, 0, iv.Length);
                Array.ConstrainedCopy(combined, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(ciphertext))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
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