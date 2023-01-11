using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XFileWriterExtensions
    {
        public static void xFileWrite(this string fileName, string content, Encoding encoding)
        {
            File.WriteAllText(fileName, content, encoding);
        }

        public static async Task xFileWriteAsync(this string fileName, string content, Encoding encoding)
        {
            await File.WriteAllTextAsync(fileName, content, encoding);
        }

        public static void xFileWriteAllLines(this string fileName, string[] lines, Encoding encoding)
        {
            File.WriteAllLines(fileName, lines, encoding);
        }

        public static async Task
            xFileWriteAllLinesAsync(this string fileName, string[] lines, Encoding encoding)
        {
            await File.WriteAllLinesAsync(fileName, lines, encoding);
        }

        public static void xFileWriteBytes(this string fileName, byte[] bytes)
        {
            File.WriteAllBytes(fileName, bytes);
        }

        public static async Task xFileWriteBytesAsync(this string fileName, byte[] bytes)
        {
            await File.WriteAllBytesAsync(fileName, bytes);
        }
    }
}