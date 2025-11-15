using System;

namespace eXtensionSharp.V2;

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
