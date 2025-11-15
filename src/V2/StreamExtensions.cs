using System;

namespace eXtensionSharp;

public static class StreamExtensions
{
    extension(Stream source)
    {
       public byte[] xStremToByteArray()
        {
            using var ms = new MemoryStream();
            source.CopyTo(ms);
            return ms.ToArray();
        }        
    }
}
