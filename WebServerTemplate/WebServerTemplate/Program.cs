using nanoFramework.Networking;
using nanoFramework.WebServer;
using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Threading;

namespace WebServerTemplate
{
    public class Program
    {
        private const string SSID = "";
        private const string WIFI_PASSWORD = "";
        private static readonly TimeSpan _timeout = TimeSpan.FromMinutes(3);   

        public static void Main()
        {

            var cancellationTokenSource = new CancellationTokenSource(_timeout);

            var connected = false;

            try
            {
                var availableNetwork = new WifiAvailableNetwork();
                var adapter = WifiNetworkHelper.WifiAdapter;
                // Static IP over Router 192.168.178.61
                connected = WifiNetworkHelper.ScanAndConnectDhcp(SSID, WIFI_PASSWORD, reconnectionKind: WifiReconnectionKind.Automatic, requiresDateTime: false, token: cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception while connection to wifi! {@ex}");
            }

            if (!connected)
            {
                //TODO Turn LED on to indicate failed connection to WLAN
                Debug.WriteLine($"Could not connect to WLAN: [{SSID}]. Exception helper: [{@WifiNetworkHelper.HelperException}]");
                
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource = new CancellationTokenSource(_timeout);
                }

                try
                {
                    connected = NetworkHelper.SetupAndConnectNetwork(cancellationTokenSource.Token, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception ahile setup and connect network: [{@ex}]");
                }

                if (!connected)
                {
                    //TODO Turn LED on to indicate failed connection to WLAN
                    return;
                }
            }

            try
            {
                using WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerGPIO) });

                server.Start();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception while starting web server [{@ex}]");
            }

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
