using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace eXtensionSharp
{
    public static class XFileExtensions
    {
        /// <summary>
        /// 파일명 조회
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string xGetFileName(this string fileName) => Path.GetFileName(fileName);

        /// <summary>
        /// 파일명 조회 (확장자 없이)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
		public static string xGetFileNameWithoutExtension(this string fileName) => Path.GetFileNameWithoutExtension(fileName);

        /// <summary>
        /// 파일 확장자 조회
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string xGetExtension(this string fileName) => Path.GetExtension(fileName);

        public static string xRead(this string fileName)
        {
            if (fileName.xExists().xIsFalse()) return string.Empty;

			return File.ReadAllText(fileName);
		}

        public static async Task<string> xReadAsync(this string fileName)
        {
            if (fileName.xExists().xIsFalse()) return string.Empty;

			return await File.ReadAllTextAsync(fileName);
		}

        public static string[] xReadLines(this string fileName)
        {
			if (fileName.xExists().xIsFalse()) return default(string[]);

			return File.ReadAllLines(fileName);
        }

        public static async Task<string[]> xReadLinesAsync(this string fileName)
        {
            if (fileName.xExists().xIsFalse()) return default(string[]);

			return await File.ReadAllLinesAsync(fileName);
		}

        public static byte[] xReadBytes(this string fileName)
        {
            if (fileName.xExists().xIsFalse()) return default(byte[]);

			return File.ReadAllBytes(fileName);
		}

        public static async Task<byte[]> xReadBytesAsync(this string fileName)
        {
            if (fileName.xExists().xIsFalse()) return default(byte[]);

			return await File.ReadAllBytesAsync(fileName);
		}

		public static bool xExists(this string fileName)
		{
			return File.Exists(fileName);
		}

		public static bool xDirExists(this string dir)
        {
            return Directory.Exists(dir);
        }

        public static bool xHasExtension(this string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.xIsNotEmpty();
        }

        public static string xUniqueId(this string fileName)
        {
            var ret = string.Empty;
            if(fileName.xExists().xIsFalse()) return ret;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static string xUniqueId(this FileInfo fileInfo)
        {
            if (!fileInfo.Exists.xIsFalse()) return string.Empty;
            return
                $"{fileInfo.FullName}|{fileInfo.CreationTime.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD_HH_MM_SS)}|{fileInfo.LastWriteTime.xToDate(ENUM_DATE_FORMAT.YYYY_MM_DD_HH_MM_SS)}"
                    .xGetHashCode();
        }

		private static Regex _fileExtension =
			new Regex(@"^.*\.(zip|ZIP|jpg|JPG|gif|GIF|doc|DOC|pdf|PDF)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		public static bool xIsExtension(this string fileName)
        {
            return _fileExtension.IsMatch(fileName);
        }

        public static void xZip(this string srcDir, string destZipFileName,
            CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            ZipFile.CreateFromDirectory(srcDir, destZipFileName, compressionLevel, false);
        }

        public static void xUnzip(this string srcFileName, string destdir)
        {
            if (srcFileName.xIsExtension())
                ZipFile.ExtractToDirectory(srcFileName, destdir, null, true);
        }

        /// <summary>
        ///     create file
        ///     if dir path not exists, throw exception
        /// </summary>
        /// <param name="fileName"></param>
        public static void xWrite(this string fileName, Func<byte[]> func)
        {
            using (var fs = File.Create(fileName))
            {
                var bytes = func();
                fs.Write(bytes);
            }   
        }

        /// <summary>
        ///     create dir and file
        /// </summary>
        /// <param name="fileName"></param>
        public static void xWriteAll(this string fileName, Func<byte[]> func)
        {
            var exist = Directory.Exists(fileName);
            if(exist.xIsFalse())
            {
				fileName.xDirectoryCreateAll();
			}
			fileName.xWrite(func);
        }

        /// <summary>
        /// create dir
        /// </summary>
        /// <param name="path"></param>
        public static void xDirectoryCreate(this string path)
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
				xDirectoryCreateAll(path);
            }
        }

        public static void xDirectoryCreateAll(this string path)
        {
            List<string> paths = path.xSplit(Path.DirectorySeparatorChar.ToString()).xToList();
			var driveSplitSymbol = ":";

			var dir = string.Empty;
            paths.xForEach((i, path) =>
            {
                if (!Path.GetExtension(path).xIsEmpty()) return false;
                if (path.xContains(new[] { driveSplitSymbol }))
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

        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="fullFileName"></param>
        public static void xFileDelete(this string fullFileName)
        {
            if (File.Exists(fullFileName)) File.Delete(fullFileName);
        }

        /// <summary>
        /// delete all files in path
        /// </summary>
        /// <param name="fullPathName"></param>
        public static void xDeleteAll(this string fullPathName)
        {
			var rootPath = Path.GetDirectoryName(Path.GetDirectoryName(fullPathName));
			if (Directory.Exists(rootPath))
			{
                Directory.Delete(rootPath, true);
			}
		}

		/// <summary>
		/// file copy from source to target
		/// </summary>
		/// <param name="sourceDir"></param>
		/// <param name="targetDir"></param>
		/// <param name="isOverWrite"></param>
		/// <param name="isRemoveTargetDir"></param>
		public static void xCopy(this string sourceDir, string targetDir, bool isOverWrite = true, bool isRemoveTargetDir = false)
        {
            if (isRemoveTargetDir)
            {
				if (Directory.Exists(targetDir))
				{
					Directory.Delete(targetDir, true);
				}

				Directory.CreateDirectory(targetDir);
			}

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), isOverWrite);

            foreach (var directory in Directory.GetDirectories(sourceDir))
                xCopy(directory, Path.Combine(targetDir, Path.GetFileName(directory)), isOverWrite, isRemoveTargetDir);
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

        public static string[] xReadFileToLine(this string filePath, int linesToRead)
        {
            var lines = new List<string>();
            
            var line = string.Empty;
            int lineCount = 0;

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            while ((line = reader.ReadLine()) != null && lineCount < linesToRead)
            {
                lines.Add(line);
                lineCount++;
            }

            return lines.ToArray();
        }

		public static Dictionary<string, IEnumerable<string>> xGetFiles(this string path)
		{
			Dictionary<string, IEnumerable<string>> result = new Dictionary<string, IEnumerable<string>>();

			if (!Directory.Exists(path))
			{
				return result;
			}

			result = SearchFiles(path);

			return result;
		}

		private static Dictionary<string, IEnumerable<string>> SearchFiles(string directory)
		{
			var result = new Dictionary<string, IEnumerable<string>>();

			try
			{
				string[] files = Directory.GetFiles(directory);
				result.Add(directory, files);

				string[] subdirectories = Directory.GetDirectories(directory);
				foreach (string subdir in subdirectories)
				{
					// 재귀적으로 하위 디렉토리에서 파일 검색
					var subdirectoryFiles = SearchFiles(subdir);
					foreach (var kvp in subdirectoryFiles)
					{
						if (!result.ContainsKey(kvp.Key))
							result.Add(kvp.Key, kvp.Value);
						else
						{
							var fileList = new List<string>(result[kvp.Key]);
							fileList.AddRange(kvp.Value);
							result[kvp.Key] = fileList;
						}
					}
				}
			}
			catch
			{
				
			}

			return result;
		}
	}
}