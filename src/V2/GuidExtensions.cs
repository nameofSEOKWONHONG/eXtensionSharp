using System;

namespace eXtensionSharp.V2;

public static class GuidExtensions
{
    extension(Guid? source)
    {
        public string xToString(string format = "")
        {
            return source?.ToString(format);
        }
    }
}
