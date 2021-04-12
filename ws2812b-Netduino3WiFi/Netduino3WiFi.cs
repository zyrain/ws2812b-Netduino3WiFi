using System.Device.Gpio;

namespace Ws2812b_Netduino3WiFi
{

    /// MOSI - D11 -> Master Out Slave In = ? on schematic
    /// MISO - (unused) D12 -> MISO = ? on schematic 
    /// SCLK - (unused) D13 -> SPCK = ? on schematic
    /// CS/SS - D4 -> Chip Select = NSS = ? on schematic
    /// 

    class Netduino3WiFi
    {
        public const int CS_PIN = ('B' - 'A') * 16 + 12;  // D4
        public const int SCLK_PIN = ('B' - 'A') * 16 + 13; // D13
        public const int MISO_PIN = ('B' - 'A') * 16 + 14; // D12
        public const int MOSI_PIN = ('B' - 'A') * 16 + 15; // D11

        public const string SPI_BUS_ID = "SPI2";

        public const int ONBOARD_LED_PIN = 10;

        private GpioController gpio = new GpioController();

        public Netduino3WiFi()
        {
            gpio.OpenPin(ONBOARD_LED_PIN, PinMode.Output);
        }

        public bool OnBoardLed
        {
            set
            {
                gpio.Write(ONBOARD_LED_PIN, value ? PinValue.High : PinValue.Low);
            }
        }
    }
}
 
