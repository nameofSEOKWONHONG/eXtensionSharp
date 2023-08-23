namespace eXtensionSharp
{
    public static class XHttpExtensions
    {
        public static async Task xHttpDownloadFileAsync(this string baseUrl
            , string url)
        {
            if(url.xIsEmpty()) return;
            
            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri uriResult))
                throw new InvalidOperationException("URI is invalid.");

            if (File.Exists(url))
                throw new Exception("File already exists.");

            using var httpClient = new HttpClient();
            byte[] fileBytes = await httpClient.GetByteArrayAsync(baseUrl);
            await File.WriteAllBytesAsync(url, fileBytes);
        }
    }
}