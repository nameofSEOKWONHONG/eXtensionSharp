using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XFileWriterExtensions
    {
        public static void xFileWriteAllLines(this string fileName, string[] lines, Encoding encoding = null)
        {
            File.WriteAllLines(fileName, lines, encoding);
        }

        public static async Task
            xFileWriteAllLinesAsync(this string fileName, string[] lines, Encoding encoding = null)
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