using System;
using System.IO.Compression;

namespace eXtensionSharp.V2;

public static class ArchiveExtensions
{
    extension(string source)
    {
        /// <summary>
        /// Compresses the source file using GZip compression and saves the result to the destination file.
        /// </summary>
        /// <param name="dest">The path where the compressed file will be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task xCompressByGzip(string dest, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            if (!source.xIsAllowFileExtension()) throw new NotSupportedException();
            
            await using var sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            await using var compressedStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
            await using var gzipStream = new GZipStream(compressedStream, compressionLevel);
            await sourceStream.CopyToAsync(gzipStream);
        }

        /// <summary>
        /// Decompresses the source GZip compressed file and saves the result to the destination file.
        /// </summary>
        /// <param name="dest">The path where the decompressed file will be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task xDecompressByGzip(string dest)
        {
            if (!source.xIsAllowFileExtension()) throw new NotSupportedException();
            
            await using var compressedStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            await using var decompressedStream = new FileStream(dest, FileMode.Create, FileAccess.Write);
            await using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            await gzipStream.CopyToAsync(decompressedStream);
        }

        /// <summary>
        /// Compresses the source file using Brotli compression and saves the result to the destination file.
        /// </summary>
        /// <param name="dest">The path where the compressed file will be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task xCompressByBrotli(string dest, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            if (!source.xIsAllowFileExtension()) throw new NotSupportedException();
            
            await using var input = new FileStream(source, FileMode.Open, FileAccess.Read);
            await using var output = new FileStream(dest, FileMode.Create, FileAccess.Write);
            await using var brotli = new BrotliStream(output, compressionLevel);
            await input.CopyToAsync(brotli);
        }

        /// <summary>
        /// Decompresses the source Brotli compressed file and saves the result to the destination file.
        /// </summary>
        /// <param name="dest">The path where the decompressed file will be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task xDecompressByBrotli(string dest)
        {
            if (!source.xIsAllowFileExtension()) throw new NotSupportedException();
            
            await using var input = new FileStream(source, FileMode.Open, FileAccess.Read);
            await using var output = new FileStream(dest, FileMode.Create, FileAccess.Write);
            await using var brotli = new BrotliStream(input, CompressionMode.Decompress);
            await brotli.CopyToAsync(output);
        }

        /// <summary>
        /// Compresses the specified directory into a ZIP file with the given compression level.
        /// </summary>
        /// <param name="destZipFileName">The destination path for the resulting ZIP file.</param>
        /// <param name="compressionLevel">The level of compression to be applied. Default is <see cref="CompressionLevel.Fastest"/>.</param>
        /// <example>
        /// <code>
        /// string sourceDirectory = @"C:\Path\To\Directory";
        /// string zipFile = @"C:\Path\To\Archive.zip";
        /// sourceDirectory.xCompressByZip(zipFile, CompressionLevel.Optimal);
        /// </code>
        /// </example>
        public void xCompressByZip(string destZipFileName, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            ZipFile.CreateFromDirectory(source, destZipFileName, compressionLevel, false);
        }

        /// <summary>
        /// Decompresses the specified ZIP file to a target directory.
        /// </summary>
        /// <param name="destdir">The target directory where the contents of the ZIP file will be extracted.</param>
        /// <remarks>
        /// This method checks if the file has a valid extension before attempting to extract it.
        /// </remarks>
        /// <example>
        /// <code>
        /// string zipFile = @"C:\Path\To\Archive.zip";
        /// string extractDirectory = @"C:\Path\To\Extracted";
        /// zipFile.xDecompressByZip(extractDirectory);
        /// </code>
        /// </example>
        public void xDecompressByZip(string destdir)
        {
            if (source.xIsAllowFileExtension())
                ZipFile.ExtractToDirectory(source, destdir, null, true);
        }    
    }
}
