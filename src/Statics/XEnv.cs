using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace eXtensionSharp
{
    public class XEnv
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

        public static bool xIsFreeBSD()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
        }

        public static int xCpuCoreCount()
        {
            return Environment.ProcessorCount;
        }

        public static bool xIsX64()
        {
            return Environment.Is64BitOperatingSystem;
        }
        
        public static string GetLocalIPAddress()
        {
            string ipAddress = string.Empty;

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                     networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                    foreach (UnicastIPAddressInformation ipAddressInfo in ipProperties.UnicastAddresses)
                    {
                        if (ipAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddress = ipAddressInfo.Address.ToString();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        break;
                    }
                }
            }

            return ipAddress;
        }
        
        public static string GetExternalIPAddress()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://ipinfo.io");
            var response = client.GetAsync("ip").GetAwaiter().GetResult();
            var html = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            string externalip = html.Trim();
            if (externalip.xIsNotEmpty())
            {
                externalip = GetLocalIPAddress();//null경우 Get Internal IP를 가져오게 한다.
            }
            return externalip;
        }        
    }
}