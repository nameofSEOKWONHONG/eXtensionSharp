using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptionSha256
    {
        /// <summary>
        ///     SHA256 Encrypt (Decrypt is not support.)
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns></returns>
        public static string xToSha256(this string encryptText)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(encryptText));

                var stringBuilder = new StringBuilder();

                foreach (var b in hash) stringBuilder.AppendFormat("{0:x2}", b);

                return stringBuilder.ToString();
            }
        }
    }
}