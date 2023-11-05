using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;

namespace RGBStrip
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            Configuration.SetPinFunction(23, DeviceFunction.PWM1);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var pwmPin = PwmChannel.CreateFromPin(23, 80000, 0);

            Debug.WriteLine("Duty cycle : 0");
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Debug.WriteLine("Duty cycle : 0.2");
            pwmPin.DutyCycle = 0.2;
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Debug.WriteLine("Duty cycle : 0.4");
            pwmPin.DutyCycle = 0.4;
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Debug.WriteLine("Duty cycle : 0.6");
            pwmPin.DutyCycle = 0.6;
            Thread.Sleep(TimeSpan.FromSeconds(5));
            
            Debug.WriteLine("Duty cycle : 0.8");
            pwmPin.DutyCycle = 0.8;
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Debug.WriteLine("Duty cycle : 1");
            pwmPin.DutyCycle = 1;
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Debug.WriteLine("Duty cycle : 0");
            pwmPin.DutyCycle = 0;
            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
