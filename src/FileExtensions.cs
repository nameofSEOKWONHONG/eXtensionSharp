using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eXtensionSharp {
    public static class XFile {
        public static string xFileReadLine(this string fileName) {
            if (fileName.xFileExists()) {
                return File.ReadAllText(fileName);
            }

            return string.Empty;
        }
        public static string[] xFileReadLines(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllLines(fileName);
        }

        public static async Task<string[]> xFileReadLineAsync(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllLinesAsync(fileName);
        }

        public static byte[] xFileReadBytes(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllBytes(fileName);
        }

        public static async Task<byte[]> xFileReadBytesAsync(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllBytesAsync(fileName);
        }

        public static void xFileWriteAllLines(this string fileName, string[] lines, Encoding encoding = null) {
            if (encoding.xIsNotNull())
                File.WriteAllLines(fileName, lines, encoding);

            File.WriteAllLines(fileName, lines);
        }

        public static async Task
            xFileWriteAllLinesAsync(this string fileName, string[] lines, Encoding encoding = null) {
            if (encoding.xIsNotNull())
                await File.WriteAllLinesAsync(fileName, lines, encoding);

            await File.WriteAllLinesAsync(fileName, lines);
        }

        public static void xFileWriteBytes(this string fileName, byte[] bytes) {
            File.WriteAllBytes(fileName, bytes);
        }

        public static async Task xFileWriteBytesAsync(this string fileName, byte[] bytes) {
            await File.WriteAllBytesAsync(fileName, bytes);
        }

        public static bool xFileExists(this string fileName) {
            return File.Exists(fileName);
        }

        public static string xFileUniqueId(this string fileName) {
            var ret = string.Empty;
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(fileName)) {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static bool xIsFileExtension(this string fileName,
            string pattern = @"^.*\.(zip|ZIP|jpg|JPG|gif|GIF|doc|DOC|pdf|PDF)$") {
            var match = Regex.Match(fileName, pattern);
            return match.Success;
        }

        public static void xFileZip(this string srcDir, string destZipFileName,
            CompressionLevel compressionLevel = CompressionLevel.Fastest) {
            ZipFile.CreateFromDirectory(srcDir, destZipFileName, compressionLevel, false);
        }

        public static void xFileUnzip(this string srcFileName, string destdir) {
            if (srcFileName.xIsFileExtension()) ZipFile.ExtractToDirectory(srcFileName, destdir, null, true);
        }
    }
}