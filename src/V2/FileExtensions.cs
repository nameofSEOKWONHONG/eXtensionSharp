using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace eXtensionSharp;

public static class FileExtensions
{
    private static Regex _fileExtensionRegex = new Regex(@"^.*\.(zip|ZIP|jpg|JPG|gif|GIF|doc|DOC|pdf|PDF|gr|GR|br|BR|)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    extension(string source)
    {

        /// <summary>
        /// get file name
        /// </summary>
        /// <returns></returns>
        public string xGetFileName() => Path.GetFileName(source);

        /// <summary>
        /// get file name without extension
        /// </summary>
        /// <returns></returns>
		public string xGetFileNameWithoutExtension() => Path.GetFileNameWithoutExtension(source);

        /// <summary>
        /// get extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string xGetExtension() => Path.GetExtension(source);

        public string xRead()
        {
            if (!source.xExists()) return string.Empty;

			return File.ReadAllText(source);
		}

        public async Task<string> xReadAsync()
        {
            if (!source.xExists()) return string.Empty;

			return await File.ReadAllTextAsync(source);
		}

        public string[] xReadLines()
        {
			if (!source.xExists()) return default(string[]);

			return File.ReadAllLines(source);
        }

        public async Task<string[]> xReadLinesAsync()
        {
            if (!source.xExists()) return default(string[]);

			return await File.ReadAllLinesAsync(source);
		}

        public byte[] xReadBytes()
        {
            if (!source.xExists()) return default(byte[]);

			return File.ReadAllBytes(source);
		}

        public async Task<byte[]> xReadBytesAsync()
        {
            if (!source.xExists()) return default(byte[]);

			return await File.ReadAllBytesAsync(source);
		}

		public bool xExists()
		{
			return File.Exists(source);
		}

		public bool xDirExists()
        {
            return Directory.Exists(source);
        }

        public bool xHasExtension()
        {
            var extension = Path.GetExtension(source);
            return extension.xIsNotEmpty();
        }

        public string xUniqueId()
        {
            var ret = string.Empty;
            if(!source.xExists()) return ret;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(source))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
        
        public bool xIsAllowFileExtension()
        {
	        return _fileExtensionRegex.IsMatch(source);
        }

        /// <summary>
        /// create file
        /// if dir path not exists, throw exception
        /// </summary>
        /// <param name="fileName"></param>
        public void xWrite(Func<byte[]> func)
        {
	        using var fs = File.Create(source);
	        var bytes = func();
	        fs.Write(bytes);
        }

        /// <summary>
        ///     create dir and file
        /// </summary>
        /// <param name="fileName"></param>
        public void xWriteAll(Func<byte[]> func)
        {
            var exist = Directory.Exists(source);
            if(!exist)
            {
				source.xCreateDirectory();
			}
			source.xWrite(func);
        }

        /// <summary>
        /// create dir
        /// </summary>
        /// <param name="path"></param>
        public void xDirectoryCreate()
        {
            var isException = false;
            try
            {
                Directory.CreateDirectory(source);
            }
            catch
            {
                isException = true;
            }

            if (isException)
            {
				xCreateDirectory(source);
            }
        }

        public void xCreateDirectory()
        {
            List<string> paths = source.xSplit(Path.DirectorySeparatorChar.ToString()).xToList();
			var driveSplitSymbol = ":";

			var dir = string.Empty;
            paths.xForEach((i, path) =>
            {
                if (!Path.GetExtension(source).xIsEmpty()) return false;
                if (source.xContains(new[] { driveSplitSymbol }))
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
        public void xFileDelete()
        {
            if (File.Exists(source)) File.Delete(source);
        }

        /// <summary>
        /// delete all file, using file root path
        /// </summary>
        public void xDeleteAll()
        {
			var rootPath = Path.GetDirectoryName(Path.GetDirectoryName(source));
			if (Directory.Exists(rootPath))
			{
                Directory.Delete(rootPath, true);
			}
		}

		/// <summary>
		/// file copy from source to target
		/// </summary>
		/// <param name="targetDir"></param>
		/// <param name="isOverWrite"></param>
		/// <param name="isRemoveTargetDir"></param>
		public void xCopy(string targetDir, bool isOverWrite = true, bool isRemoveTargetDir = false)
        {
            if (isRemoveTargetDir)
            {
				if (Directory.Exists(targetDir))
				{
					Directory.Delete(targetDir, true);
				}

				Directory.CreateDirectory(targetDir);
			}

            foreach (var file in Directory.GetFiles(source))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), isOverWrite);

            foreach (var directory in Directory.GetDirectories(source))
                xCopy(directory, Path.Combine(targetDir, Path.GetFileName(directory)), isOverWrite, isRemoveTargetDir);
        }

        public Dictionary<string, string> xGetFileExtensionProperties()
        {
            if (!OperatingSystem.IsWindows()) throw new NotSupportedException("Support Windows only.");

            var dictionary = new Dictionary<string, string>();
            if (!File.Exists(source)) throw new FileNotFoundException();
            
            Process process = new Process();
            process.StartInfo.FileName = "wmic.exe";
            process.StartInfo.Arguments = $"datafile where Name=\"{source}\"";
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

        /// <summary>
        /// get files using path
        /// </summary>
        /// <returns></returns>
		public Dictionary<string, IEnumerable<string>> xGetFiles()
		{
			Dictionary<string, IEnumerable<string>> result = new Dictionary<string, IEnumerable<string>>();

			if (!Directory.Exists(source))
			{
				return result;
			}

			result = xSearchFiles(source);

			return result;
		}

        /// <summary>
        /// search file
        /// </summary>
        /// <returns></returns>
		public Dictionary<string, IEnumerable<string>> xSearchFiles()
		{
			var result = new Dictionary<string, IEnumerable<string>>();

			try
			{
				string[] files = Directory.GetFiles(source);
				result.Add(source, files);

				string[] subdirectories = Directory.GetDirectories(source);
				foreach (string subdir in subdirectories)
				{
					var subdirectoryFiles = xSearchFiles(subdir);
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
			finally
			{
				
			}

			return result;
		}

        /// <summary>
        ///     executable app root path
        ///     c:\development\MyApp
        ///     “TargetFile.cs”.ToApplicationPath()
        ///     c:\development\MyApp\TargetFile.cs
        /// </summary>
        /// <returns></returns>
        public string xToPath(string addPath = null)
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            if (!addPath.xIsEmpty())
                appRoot = appRoot + @"\" + addPath;
            return Path.Combine(appRoot, source);
        }

        public string xCurrentPath() => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, source));
    }
}