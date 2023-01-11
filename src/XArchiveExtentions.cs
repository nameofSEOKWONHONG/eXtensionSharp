using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace eXtensionSharp;

public class InMemoryFile
{
    public string FileName { get; set; }
    public byte[] Buffer { get; set; }
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
                            file.Buffer = func(file.Buffer);
                        }
                        await zipStream.WriteAsync(file.Buffer, 0, file.Buffer.Length);
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

        return new InMemoryFile() { Buffer = memFile.ToArray(), FileName = Path.GetFileName(path) };
    }
}