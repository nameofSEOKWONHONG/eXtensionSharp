using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
        
        public static string xToEncryptString(this string plainText, [StringLength(32)]string key)
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
        
        public static string xToEncryptNumber(this string encryptString, [StringLength(36)]string key)
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
        
        public static string xToDecryptString(this string cipherText, [StringLength(32)]string key)
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
        
        public static string xToDecryptNumber(this string cipherText, [StringLength(36)]string key)
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
    }
}