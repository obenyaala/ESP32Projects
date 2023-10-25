using Iot.Device.RotaryEncoder.Esp32;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace FanWithMosfet
{
    public class Program
    {
        private static PwmChannel _pwmPin;
        private static ScaledQuadratureEncoder _encoder;
        private const int RANGE_MIN = 0;
        private const int RANGE_MAX = 1;

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");
            
            Configuration.SetPinFunction(17, DeviceFunction.PWM3);
            
            _pwmPin = PwmChannel.CreateFromPin(17, 40000, 0.2);

            if (_pwmPin == null)
            {
                Console.WriteLine("PWM is null!");
                Thread.Sleep(Timeout.Infinite);
            }

            var controler = new GpioController(PinNumberingScheme.Board);
            var encoderSWPin = controler.OpenPin(16, PinMode.Input);

            _encoder = new ScaledQuadratureEncoder(pinA: 19, pinB: 18, pulsesPerRotation: 40,
                pulseIncrement: 0.05, rangeMin: RANGE_MIN, rangeMax: RANGE_MAX, refreshRate: TimeSpan.FromSeconds(1));
            
            _encoder.Value = 1;
            _pwmPin.DutyCycle = _encoder.Value;
            
            _encoder.ValueChanged += _EncoderValueChanged;
            //encoderSWPin.DebounceTimeout = TimeSpan.FromSeconds(1);
            encoderSWPin.ValueChanged += _EncoderSWPinValueChanged;

            //var inc = 0.1;
            //double dutyCycle = 0.2;

            //Console.WriteLine($"Duty cycle: [{dutyCycle}], PWM duty cycle: [{_pwmPin.DutyCycle}]");

            //Thread.Sleep(TimeSpan.FromSeconds(15));

            //for (int i = 0; i < 8; i++)
            //{
            //    dutyCycle += inc;
            //    if (dutyCycle > 1) dutyCycle = 1;
            //    pwmPin.DutyCycle = dutyCycle;
            //    Console.WriteLine($"Index: [{i}], duty cycle: [{dutyCycle}], PWM duty cycle: [{pwmPin.DutyCycle}]");
            //    Thread.Sleep(TimeSpan.FromSeconds(5));
            //}

            //Console.WriteLine("Reversing . . .");

            //for (int i = 8 - 1; i >= 0; i--)
            //{
            //    dutyCycle -= inc;
            //    if (dutyCycle < 0) dutyCycle = 0;
            //    pwmPin.DutyCycle = dutyCycle;
            //    Console.WriteLine($"Index [{i}], duty cycle: [{dutyCycle}], PWM duty cycle: [{pwmPin.DutyCycle}]");
            //    Thread.Sleep(TimeSpan.FromSeconds(5));
            //}

            //Thread.Sleep(TimeSpan.FromSeconds(20));

            //pwmPin.DutyCycle = 0.2;

            //Console.WriteLine($"Duty cycle: [{pwmPin.DutyCycle}]");

            //Thread.Sleep(TimeSpan.FromSeconds(20));

            //pwmPin.DutyCycle = 0.3;

            //Console.WriteLine($"Duty cycle: [{pwmPin.DutyCycle}]");

            //Thread.Sleep(TimeSpan.FromSeconds(20));

            //pwmPin.DutyCycle = 0.4;

            //Console.WriteLine($"Duty cycle: [{pwmPin.DutyCycle}]");

            //Thread.Sleep(TimeSpan.FromSeconds(20));

            //pwmPin.Start();

            //Thread.Sleep(TimeSpan.FromSeconds(2));

            //Console.WriteLine($"Duty cycle: [{pwmPin.DutyCycle}]");

            //for (int i = 0; i < 9; i++)
            //{
            //    pwmPin.Stop();
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //    pwmPin.DutyCycle += 0.1;
            //    pwmPin.Start();
            //    Console.WriteLine($"Duty cycle: [{pwmPin.DutyCycle}]");
            //    Thread.Sleep(TimeSpan.FromSeconds(5));
            //}

            //for (int i = 9 - 1; i >= 0; i--)
            //{
            //    pwmPin.Stop();
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //    pwmPin.DutyCycle -= 0.1;
            //    pwmPin.Start();
            //    Console.WriteLine($"Duty cycle: [{pwmPin.DutyCycle}]");
            //    Thread.Sleep(TimeSpan.FromSeconds(5));
            //}

            //pwmPin.DutyCycle = 0.3;
            Console.WriteLine("Closing application!");
            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }

        private static void _EncoderSWPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.ChangeType == PinEventTypes.Rising)
            {
                Debug.WriteLine($"Encoder button clicked!");
                if (_encoder.Value == 0)
                {
                    _encoder.Value = 1;
                    _pwmPin.DutyCycle = 1;
                }
                else
                {
                    _encoder.Value = 0;
                    _pwmPin.DutyCycle = 0;

                }
            }
        }

        private static void _EncoderValueChanged(object sender, RotaryEncoderEventArgs e)
        {
            double roundedNumber = _RoundToDecimalPlaces(_encoder.Value, 2);
            _pwmPin.DutyCycle = roundedNumber;
            Debug.WriteLine($"Encoder value changed! Value: [{roundedNumber}], Pulses: [{_encoder.PulseCount}], Rotations: [{_encoder.Rotations}]");
        }

        public static double _RoundToDecimalPlaces(double number, int decimalPlaces)
        {
            double multiplier = Math.Pow(10, decimalPlaces);
            double roundedNumber = Math.Floor(number * multiplier + 0.5) / multiplier;
            return roundedNumber;
        }
    }
}
