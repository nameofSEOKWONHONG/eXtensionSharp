using System.Runtime.InteropServices;

namespace eXtensionSharp
{
    public class XOS
    {
        public static bool xIsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool xIsMac()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool xIsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }
    }
}