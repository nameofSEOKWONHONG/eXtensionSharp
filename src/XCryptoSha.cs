using System.Security.Cryptography;
using System.Text;

namespace eXtensionSharp
{
    public static class XCryptoSha256
    {
        /// <summary>
        /// Computes the SHA-256 hash of the input string and returns the result as a hexadecimal string.
        /// </summary>
        /// <param name="input">The input string to be hashed.</param>
        /// <returns>The hexadecimal representation of the SHA-256 hash of the input string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the hashing operation fails.</exception>
        public static string xComputeHash256(this string input)
        {
            ReadOnlySpan<byte> inputBytes = Encoding.UTF8.GetBytes(input);

            Span<byte> hash = stackalloc byte[32]; // SHA-256 = 32바이트
            if (!SHA256.TryHashData(inputBytes, hash, out int written) || written != 32)
                throw new InvalidOperationException("SHA256 해시 실패");

            return ToHex(hash);
        }
        
        // Byte -> Hex 변환 (allocation-free)
        private static string ToHex(ReadOnlySpan<byte> bytes)
        {
            Span<char> result = stackalloc char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                result[i * 2] = GetHexChar(b >> 4);
                result[i * 2 + 1] = GetHexChar(b & 0xF);
            }
            return new string(result);
        }
        
        
        /// <summary>
        /// Computes the SHA-512 hash of the input string and returns the hash as a hexadecimal string.
        /// </summary>
        /// <param name="input">The input string to be hashed.</param>
        /// <returns>A string containing the hexadecimal representation of the SHA-512 hash.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the SHA-512 hash calculation fails.</exception>
        public static string xComputeHash512(this string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Span<byte> hashBytes = stackalloc byte[64]; // SHA-512: 64 bytes

            if (!SHA512.TryHashData(inputBytes, hashBytes, out int written) || written != 64)
                throw new InvalidOperationException("SHA512 해시 계산 실패");

            return ConvertToHex(hashBytes);
        }
        
        private static string ConvertToHex(ReadOnlySpan<byte> bytes)
        {
            Span<char> result = stackalloc char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                result[i * 2] = GetHexChar(b >> 4);
                result[i * 2 + 1] = GetHexChar(b & 0xF);
            }
            return new string(result);
        }
        
        private static char GetHexChar(int value)
            => (char)(value < 10 ? '0' + value : 'a' + (value - 10));
    }
}