using Iot.Device.RotaryEncoder.Esp32;
using Iot.Device.Ws28xx.Esp32;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

namespace EncoderRGBStrip
{
    public class Program
    {
        private static ScaledQuadratureEncoder _encoder;
        private static object _encoderLock = new();
        private static object _neoLock = new();
        private static Ws2808 _neo;
        private const int RANGE_MIN = 0;
        private const int RANGE_MAX = 20;

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            var controler = new GpioController(PinNumberingScheme.Board);
            var encoderSWPin = controler.OpenPin(17, PinMode.Input);
            _encoder = new ScaledQuadratureEncoder(pinA: 18, pinB: 19, pulsesPerRotation: 80,
                pulseIncrement: 0.25, rangeMin: RANGE_MIN, rangeMax: RANGE_MAX, refreshRate: TimeSpan.FromMilliseconds(20));

            _encoder.ValueChanged += _EncoderValueChanged;
            encoderSWPin.DebounceTimeout = TimeSpan.FromSeconds(2);
            encoderSWPin.ValueChanged += _EncoderSWPinValueChanged;

            _neo = new Ws2808(23, 1);
            _neo.Image.Clear();
            _neo.Update();
            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }

        private static void _EncoderSWPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            Debug.WriteLine($"Encoder button value changed! Change type: [{e.ChangeType}]");
        }

        private static void _EncoderValueChanged(object sender, RotaryEncoderEventArgs e)
        {
            lock (_encoderLock)
            {
                var index = Math.Round(_encoder.Value);
                Debug.WriteLine($"Encoder value changed! Value: [{index}], Pulses: [{_encoder.PulseCount}], Rotations: [{_encoder.Rotations}]");
                _SetColor((int)index);
            }
        }

        private static void _SetColor(int index)
        {
            lock (_neoLock)
            {
                if (index == RANGE_MIN)
                {
                    _ClearPixels();
                    return;
                }

                if (index >= RANGE_MAX)
                {
                    _SetColor(255, 255, 255);
                    return;
                }

                var value = (255.0 / 20.0) * index;
                _SetColor((byte)value, (byte)value, (byte)value);
            }
        }

        private static void _SetColor(byte r, byte g, byte b)
        {
            var image = _neo.Image;
            
            Debug.WriteLine($"Color values R: [{r}], G: [{g}], B: [{b}]");

            for (int i = 0; i < image.Width; i++)
            {
                image.SetPixel(i, 0, g, r, b);
            }

            _neo.Update();
        }

        private static void _ClearPixels()
        {
            _neo.Image.Clear();
            _neo.Update();
        }
    }
}
