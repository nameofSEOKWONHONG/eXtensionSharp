using System;

namespace eXtensionSharp;

public static class FileInfoExtensions
{
    extension(FileInfo source)
    {
        public string xUniqueId()
        {
            if (!source.xIsEmpty()) return string.Empty;
            if (!source.Exists) return string.Empty;
            return
                $"{source.FullName}|{source.CreationTime.xToDateFormat("yyyy-MM-dd HH:mm:ss")}|{source.LastWriteTime.xToDateFormat("yyyy-MM-dd HH:mm:ss")}"
                    .xGetHashCode();
        }
    }
}