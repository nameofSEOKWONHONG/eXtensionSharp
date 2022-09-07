using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace eXtensionSharp
{
    public static class XPathExtensions
    {
        /// <summary>
        ///     executable app root path
        ///     c:\development\MyApp
        ///     “TargetFile.cs”.ToApplicationPath()
        ///     c:\development\MyApp\TargetFile.cs
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string xToPath(this string fileName, string addPath = null)
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            if (!addPath.xIsEmpty())
                appRoot = appRoot + @"\" + addPath;
            return Path.Combine(appRoot, fileName);
        }

        public static string xCurrentPath(this string path) => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, path));
    }
}