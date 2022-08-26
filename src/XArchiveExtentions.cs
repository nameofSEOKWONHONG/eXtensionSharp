using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace eXtensionSharp;

public class InMemoryFile
{
    public string FileName { get; set; }
    public byte[] Content { get; set; }
}

public static class XArchiveExtentions
{
    public static async Task<byte[]> xToArchiveAsync(this IEnumerable<InMemoryFile> files, Func<byte[], byte[]> func = null)
    {
        byte[] archiveFile;
        using (var archiveStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open())
                    {
                        if (func.xIsNotEmpty())
                        {
                            file.Content = func(file.Content);
                        }
                        await zipStream.WriteAsync(file.Content, 0, file.Content.Length);
                    }
                }
            }

            archiveFile = archiveStream.ToArray();
        }

        return archiveFile;
    }
    
    public static InMemoryFile xLoadFromFile(this string path)
    {
        using var fs = File.OpenRead(path);
        using var memFile = new MemoryStream();
        fs.CopyTo(memFile);

        memFile.Seek(0, SeekOrigin.Begin);

        return new InMemoryFile() { Content = memFile.ToArray(), FileName = Path.GetFileName(path) };
    } 
}