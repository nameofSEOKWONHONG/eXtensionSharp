using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XFile
    {
        public static string xFileReadAllText(this string fileName)
        {
            var fullFileName = Path.Combine(AppContext.BaseDirectory, fileName);
            if (fullFileName.xFileExists())
            {
                var line = File.ReadAllText(fileName);
                return line;
            }

            return string.Empty;
        }

        public static string[] xFileReadAllLines(this string fileName)
        {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            var lines = File.ReadAllLines(fileName);
            return lines;
        }

        public static async Task<string[]> xFileReadAllLinesAsync(this string fileName)
        {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllLinesAsync(fileName);
        }

        public static byte[] xFileReadAllBytes(this string fileName)
        {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllBytes(fileName);
        }

        public static async Task<byte[]> xFileReadAllBytesAsync(this string fileName)
        {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllBytesAsync(fileName);
        }

        public static void xFileWriteAllLines(this string fileName, string[] lines, Encoding encoding = null)
        {
            File.WriteAllLines(fileName, lines, encoding.xIfNotNull(x => x, Encoding.Default));
        }

        public static async Task
            xFileWriteAllLinesAsync(this string fileName, string[] lines, Encoding encoding = null)
        {
            if (encoding.xIsNotNull())
                await File.WriteAllLinesAsync(fileName, lines, encoding);

            await File.WriteAllLinesAsync(fileName, lines);
        }

        public static void xFileWriteBytes(this string fileName, byte[] bytes)
        {
            File.WriteAllBytes(fileName, bytes);
        }

        public static async Task xFileWriteBytesAsync(this string fileName, byte[] bytes)
        {
            await File.WriteAllBytesAsync(fileName, bytes);
        }

        public static bool xDirExists(this string dir)
        {
            return Directory.Exists(dir);
        }
        public static bool xFileExists(this string fileName)
        {
            return File.Exists(fileName);
        }

        public static string xFileUniqueId(this string fileName)
        {
            var ret = string.Empty;
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static bool xIsFileExtension(this string fileName,
            string pattern = @"^.*\.(zip|ZIP|jpg|JPG|gif|GIF|doc|DOC|pdf|PDF)$")
        {
            var match = Regex.Match(fileName, pattern);
            return match.Success;
        }

        public static void xFileZip(this string srcDir, string destZipFileName,
            CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            ZipFile.CreateFromDirectory(srcDir, destZipFileName, compressionLevel, false);
        }

        public static void xFileUnzip(this string srcFileName, string destdir)
        {
            if (srcFileName.xIsFileExtension())
                ZipFile.ExtractToDirectory(srcFileName, destdir, null, true);
        }

        /// <summary>
        ///     create file
        ///     if dir path not exists, throw exception
        /// </summary>
        /// <param name="fileName"></param>
        public static void xFileCreate(this string fileName)
        {
            var fs = File.Create(fileName);
            fs.Close();
        }

        /// <summary>
        ///     create dir and file
        /// </summary>
        /// <param name="fileName"></param>
        public static void xFileCreateAll(this string fileName)
        {
            List<string> paths = null;

            if (XOS.xIsWindows())
                paths = fileName.xSplit('\\').xToList();
            else if (XOS.xIsLinux() || XOS.xIsMac()) paths = fileName.xSplit('/').xToList();

            var dir = string.Empty;
            paths.xForEach((path, i) =>
            {
                if (Path.GetExtension(path).xIsNotEmpty()) return false;
                if (path.xContains(new[] {":"}))
                {
                    dir += path;
                }
                else
                {
                    if (XOS.xIsWindows())
                        dir += $"{"\\"}{path}";
                    else if (XOS.xIsLinux() || XOS.xIsLinux()) dir += $"{"/"}{path}";
                }

                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                return true;
            });
            fileName.xFileCreate();
        }

        public static void xDirCreate(this string path)
        {
            var isException = false;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch
            {
                isException = true;
            }

            if(isException)
            {
                xDirCreateAll(path);
            }
        }

        public static void xDirCreateAll(this string path)
        {
            List<string> paths = null;

            if (XOS.xIsWindows())
                paths = path.xSplit('\\').xToList();
            else if (XOS.xIsLinux() || XOS.xIsMac()) paths = path.xSplit('/').xToList();

            var dir = string.Empty;
            paths.xForEach((path, i) =>
            {
                if (Path.GetExtension(path).xIsNotEmpty()) return false;
                if (path.xContains(new[] {":"}))
                {
                    dir += path;
                }
                else
                {
                    if (XOS.xIsWindows())
                        dir += $"{"\\"}{path}";
                    else if (XOS.xIsLinux() || XOS.xIsLinux()) dir += $"{"/"}{path}";
                }

                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                return true;
            });
        }

        public static void xFileDelete(this string fullFileName)
        {
            if (File.Exists(fullFileName)) File.Delete(fullFileName);
        }

        public static void xFileDeleteAll(this string fullPathName)
        {
            if (Directory.Exists(fullPathName))
            {
                Directory.Delete(fullPathName, true);
            }
        }
        
        public static void xCopy(this string sourceDir, string targetDir)
        {
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
            
            Directory.CreateDirectory(targetDir);

            foreach(var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

            foreach(var directory in Directory.GetDirectories(sourceDir))
                xCopy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
    }
}