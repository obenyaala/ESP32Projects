using Iot.Device.Ws28xx.Esp32;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;

namespace LEDBlinkDemo
{
    public class Program
    {
        private static readonly Stopwatch _stopwatch = new();
        private static readonly object _lock = new();
        public static void Main()
        {
            var controller = new GpioController(PinNumberingScheme.Board);
            var movementSensor = controller.OpenPin(17, PinMode.Input);
            movementSensor.ValueChanged += _MovementSensorValueChanged;

            //Configuration.SetPinFunction(23, DeviceFunction.SPI2_MOSI);
            //Configuration.SetPinFunction(19, DeviceFunction.SPI2_MISO);
            //Configuration.SetPinFunction(18, DeviceFunction.SPI2_CLOCK);
            //Configuration.SetPinFunction(22, DeviceFunction.ADC1_CH10);
            //// Using VSPI on bus 2 for ESP32 and pin 22 for chipselect

            //SpiConnectionSettings settings = new(2, 22)
            //{
            //    ClockFrequency = 2_400_000,
            //    Mode = SpiMode.Mode0,
            //    DataBitLength = 8
            //};
            
            //using SpiDevice spi = SpiDevice.Create(settings);
            var neo = new Ws2808(23, 1);
            var image = neo.Image;
            //image.SetPixel(0, 0, 254, 0, 0);
            //neo.Update();
            //Thread.Sleep(2000);            
            //image.SetPixel(0, 0, 0, 254, 0);
            //neo.Update();
            //Thread.Sleep(2000);
            //image.SetPixel(0, 0, 0, 0, 254);
            //neo.Update();
            //Thread.Sleep(2000);
            byte r = 10; 
            byte g = 10;
            byte b = 10;

            Console.WriteLine("Setting Green!");
            image.SetPixel(0, 0, g, 0, 0);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting Red!");
            image.SetPixel(0, 0, 0, r, 0);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting Blue!");
            image.SetPixel(0, 0, 0, 0, b);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting White!");
            image.SetPixel(0, 0, r, g, b);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));

            r = 150;
            g = 150;
            b = 150;

            Console.WriteLine("Setting Green!");
            image.SetPixel(0, 0, g, 0, 0);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting Red!");
            image.SetPixel(0, 0, 0, r, 0);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting Blue!");
            image.SetPixel(0, 0, 0, 0, b);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting White!");
            image.SetPixel(0, 0, r, g, b);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));

            r = 250;
            g = 250;
            b = 250;

            Console.WriteLine("Setting Green!");
            image.SetPixel(0, 0, g, 0, 0);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting Red!");
            image.SetPixel(0, 0, 0, r, 0);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting Blue!");
            image.SetPixel(0, 0, 0, 0, b);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));
            Console.WriteLine("Setting White!");
            image.SetPixel(0, 0, r, g, b);
            neo.Update();
            Thread.Sleep(TimeSpan.FromSeconds(7));

            Console.WriteLine("Clearing . . .");
            image.Clear();
            neo.Update();

            Thread.Sleep(Timeout.Infinite);
        }


        private static void _MovementSensorValueChanged(object sender, PinValueChangedEventArgs e)
        {
            lock (_lock)
            {
                switch (e.ChangeType)
                {
                    case PinEventTypes.None:
                        Console.WriteLine("Received Type: None");
                        break;
                    case PinEventTypes.Rising:
                        Console.WriteLine("Received Type: Rising");
                        _stopwatch.Start();
                        break;
                    case PinEventTypes.Falling:
                        Console.WriteLine("Received Type: Falling");
                        _stopwatch.Stop();
                        Console.WriteLine($"Elapsed Time: [{@_stopwatch.Elapsed}]");
                        _stopwatch.Reset();
                        break;
                    default:
                        break;
                }
            }


        }
    }
}
