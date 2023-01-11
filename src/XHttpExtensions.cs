using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace eXtensionSharp
{
    public static class XHttpExtensions
    {
        public static async Task xHttpDownloadFileAsync(this string uri
            , string outputPath)
        {
            Uri uriResult;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                throw new InvalidOperationException("URI is invalid.");

            if (File.Exists(outputPath))
                throw new Exception("File already exists.");

            using (var httpClient = new HttpClient())
            {
                byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
                File.WriteAllBytes(outputPath, fileBytes);
            }
        }
    }
}