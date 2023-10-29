using System.Device.Gpio;

namespace WebServerTemplate
{
    public class ControllerService
    {
        private readonly GpioController _gpioController = new (PinNumberingScheme.Board);
    }
}
