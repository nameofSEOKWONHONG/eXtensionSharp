using System;
using System.IO.Compression;
using System.Text;

namespace eXtensionSharp;

public static class ByteExtensions
{
    extension(byte[] source)
    {
        /// <summary>
        /// gzip uncompression
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string xUnCompress()
        {
            using var memoryStream = new MemoryStream(source);
            using var outputStream = new MemoryStream();
            using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            decompressStream.CopyTo(outputStream);

            var outputBytes = outputStream.ToArray();
            return Encoding.Unicode.GetString(outputBytes);
        }

        /// <summary>
        /// gzip uncompression
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> xUnCompressAsync()
        {
            using var memoryStream = new MemoryStream(source);
            using var outputStream = new MemoryStream();
            await using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            await decompressStream.CopyToAsync(outputStream);

            var outputBytes = outputStream.ToArray();
            return Encoding.Unicode.GetString(outputBytes);
        }

        public string xToUtf8String()
        {
            return Encoding.UTF8.GetString(source);
        }
    }
}
