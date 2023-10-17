using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace eXtensionSharp
{
    public static class XFileExtensions
    {
        public static string xGetFileName(this string fileName) => Path.GetFileName(fileName);

        public static string xGetFileNameWithoutExtension(this string fileName) => Path.GetFileNameWithoutExtension(fileName);

        public static string xGetExtension(this string fileName) => Path.GetExtension(fileName);

        public static string xGetFileNameWithBaseDir(this string fileName, string baseDir = "")
        {
            if (baseDir.xIsNotEmpty()) return Path.Combine(baseDir, fileName);
            return Path.Combine(AppContext.BaseDirectory, fileName);
        }

        public static string xFileReadAllText(this string fileName)
        {
            if (fileName.xFileExists())
            {
                return File.ReadAllText(fileName);
            }

            return string.Empty;
        }

        public static async Task<string> xFileReadAllTextAsync(this string fileName)
        {
            if (fileName.xFileExists())
            {
                return await File.ReadAllTextAsync(fileName);
            }

            return string.Empty;
        }

        public static string[] xFileReadAllLines(this string fileName)
        {
            if (fileName.xFileExists())
            {
                return File.ReadAllLines(fileName);
            }

            return new string[0];
        }

        public static async Task<string[]> xFileReadAllLinesAsync(this string fileName)
        {
            if (fileName.xFileExists())
            {
                return await File.ReadAllLinesAsync(fileName);
            }

            return new string[0];
        }

        public static byte[] xFileReadAllBytes(this string fileName)
        {
            if (fileName.xFileExists())
            {
                return File.ReadAllBytes(fileName);
            }

            return null;
        }

        public static async Task<byte[]> xFileReadAllBytesAsync(this string fileName)
        {
            if (fileName.xFileExists())
            {
                return await File.ReadAllBytesAsync(fileName);
            }

            return null;
        }

        public static bool xDirExists(this string dir)
        {
            return Directory.Exists(dir);
        }

        public static bool xFileExists(this string fileName)
        {
            return File.Exists(fileName);
        }

        public static bool xIsFile(this string pathName)
        {
            var extension = Path.GetExtension(pathName);
            return extension.xIsNotEmpty();
        }

        public static string xFileUniqueId(this string fileName)
        {
            var ret = string.Empty;
            if (!File.Exists(fileName)) return string.Empty;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static string xFileUniqueIdByFileInfo(this string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists) throw new FileNotFoundException();
            return
                $"{fileInfo.FullName}|{fileInfo.CreationTime.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD_HH_MM_SS)}|{fileInfo.LastWriteTime.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD_HH_MM_SS)}"
                    .xGetHashCode();
        }

        public static void xFileLock(this string fileName)
        {
#pragma warning disable CA1416
            if (!XEnvExtensions.xIsWindows()) throw new NotSupportedException("windows only");
            if (!File.Exists(fileName)) throw new Exception("file not exists");
            File.Encrypt(fileName);
        }

        public static void xFileUnLock(this string fileName)
        {
            if (!XEnvExtensions.xIsWindows()) throw new NotSupportedException("windows only");
            if (!File.Exists(fileName)) throw new Exception("file not exists");
            File.Decrypt(fileName);
        }

        public static bool xIsFileExtension(this string fileName,
            string pattern = @"^.*\.(zip|ZIP|jpg|JPG|gif|GIF|doc|DOC|pdf|PDF)$")
        {
            return Regex.Match(fileName, pattern).Success;
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
            List<string> paths = fileName.xSplit(Path.DirectorySeparatorChar.ToString()).xToList();

            var dir = string.Empty;
            paths.xForEach((path, i) =>
            {
                if (!Path.GetExtension(path).xIsEmpty()) return false;
                if (path.xContains(new[] { ":" }))
                {
                    dir += path;
                }
                else
                {
                    dir += $"{Path.DirectorySeparatorChar}{path}";
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

            if (isException)
            {
                xDirCreateAll(path);
            }
        }

        public static void xDirCreateAll(this string path)
        {
            List<string> paths = path.xSplit(Path.DirectorySeparatorChar.ToString()).xToList();

            var dir = string.Empty;
            paths.xForEach((path, i) =>
            {
                if (!Path.GetExtension(path).xIsEmpty()) return false;
                if (path.xContains(new[] { ":" }))
                {
                    dir += path;
                }
                else
                {
                    dir += $"{Path.DirectorySeparatorChar}{path}";
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

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

            foreach (var directory in Directory.GetDirectories(sourceDir))
                xCopy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        public static void xWriteFile(this byte[] buffer, string fileName) => File.WriteAllBytes(fileName, buffer);

        public static void xWriteFile(this string fileName, string fileContents)
        {
            File.WriteAllText(fileName, fileContents, Encoding.UTF8);
        }

        public static Dictionary<string, string> xGetFileExtensionProperties(this string fileName)
        {
            if (!XEnvExtensions.xIsWindows()) throw new NotSupportedException("Support Windows only.");

            var dictionary = new Dictionary<string, string>();
            if (!File.Exists(fileName)) throw new FileNotFoundException();
            Process process = new Process();
            process.StartInfo.FileName = "wmic.exe";
            process.StartInfo.Arguments = $"datafile where Name=\"{fileName}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            //0~32, 33 delete, 34~
            return dictionary;
        }
    }
}