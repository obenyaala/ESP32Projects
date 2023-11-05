using nanoFramework.Networking;
using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Threading;

namespace WIFITest
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(3));

            try
            {
                var availableNetwork = new WifiAvailableNetwork();
                var adapter = WifiNetworkHelper.WifiAdapter;
                var connected = WifiNetworkHelper.ScanAndConnectDhcp(WIFIConfig.SSID, WIFIConfig.WIFI_PASSWORD, token: cancellationTokenSource.Token);

                Debug.WriteLine($"Status: [{WifiNetworkHelper.Status}]. Is Connected: [{connected}]");

                Debug.WriteLine($"UTC time: [{DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")}]");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception while connection to wifi! {ex}");
            }

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
