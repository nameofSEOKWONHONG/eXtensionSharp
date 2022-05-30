using System;
using System.Runtime.InteropServices;

namespace eXtensionSharp
{
    public class XEnvExtensions
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

        public static int xCpuCoreCount()
        {
            return Environment.ProcessorCount;
        }

        public static bool xIsX64()
        {
            return Environment.Is64BitOperatingSystem;
        }
    }
}