using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace HomeAutomation.Web.Helpers
{
    public static class Enums
    {
        public static GpioPin GetPinFromIndex(int i)
        {
            return Pi.Gpio[i];
        }
        public static string JobId { get; set; }
    }
}
