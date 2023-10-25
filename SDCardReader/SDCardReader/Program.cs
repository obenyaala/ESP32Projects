using nanoFramework.Hardware.Esp32;
using nanoFramework.System.IO.FileSystem;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using static nanoFramework.System.IO.FileSystem.SDCard;

namespace SDCardReader
{
    public class Program
    {
        public static void Main()
        {


            try
            {
                Debug.WriteLine("Hello from nanoFramework!");

                Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
                Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
                Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);

                Thread.Sleep(1000);

                var sDCardSpiParameters = new SDCardSpiParameters
                {
                    chipSelectPin = 4,
                    spiBus = 1,
                    enableCardDetectPin = false,
                };

                var sDCardMmcParameters = new SDCardMmcParameters
                {
                    enableCardDetectPin = false,
                    dataWidth = SDDataWidth._1_bit
                };

                using var sdCard = new SDCard(sDCardSpiParameters);
                //using var sdCard = new SDCard(sDCardMmcParameters);
                
                sdCard.Mount();

                Debug.WriteLine($"Is mounted: [{sdCard.IsMounted}]");
               
                var logicalDrives = Directory.GetLogicalDrives();

                var externalCard = logicalDrives[0];

                if (Directory.Exists(externalCard))
                {
                    var filePath = Path.Combine(externalCard, "TestFile.txt");
                    var log = $"Writing test log [{DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss.fff")}]";
                    using FileStream file = File.Exists(filePath) ? new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite) : File.Create(filePath);
                    file.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = Encoding.UTF8.GetBytes(log);
                    file.Write(bytes, 0, bytes.Length);
                }

                sdCard.Unmount();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excetion mounting sd card: [{ex.Message}]");
            }


            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }

        private static void _StorageEventManagerRemovableDeviceRemoved(object sender, RemovableDeviceEventArgs e)
        {
            Debug.WriteLine($"Device card removed: [{e.Path}]");
        }

        private static void _StorageEventManagerRemovableDeviceInserted(object sender, RemovableDeviceEventArgs e)
        {
            Debug.WriteLine($"Card inserted: [{e.Path}]");
        }
    }
}
