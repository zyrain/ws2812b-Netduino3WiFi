using System;
using System.Threading;
using Windows.Devices.Spi;

namespace Ws2812b_Netduino3WiFi
{
    /// <summary>
    /// Class representing an SPI-driven WS2812b LEDLightStrip
    /// 
    /// MOSI - D11 -> Master Out Slave In = ? on schematic
    /// MISO - (unused) D12 -> MISO = ? on schematic 
    /// SCLK - (unused) D13 -> SPCK = ? on schematic
    /// CS/SS - D4 -> Chip Select = NSS = ? on schematic
    /// 
    /// Mode = The four possible modes are determined by the active state of the clock signal (high or low) and when the data is sampled (rising or falling edge of the clock).
    /// irrelevant for unclocked?
    /// 
    /// SpiSharingMode.Exclusive
    /// SpiMode.Mode1
    /// DataBitOrder.MSB
    /// LED Order GRB
    /// </summary>
    public class Ws2812b
    {
        private byte[] bitZero = new byte[] { (byte)0b11000000 };  // 11100000    400ns high 850ns  low = 1250ns
        private byte[] bitOne = new byte[] { (byte)0b11111100 };   // 11111000    800ns high 450ns  low
        private byte[] bitSilence = new byte[] { 0 };

        SpiDevice spi;

        /// <summary>
        /// Creates a new NeoPixelSPI with a given chip-select pin, using a given SPI module
        /// </summary>
        /// <param name="chipSelectPin">chip-select pin</param>
        /// <param name="spiModule">SPI module</param>
        public Ws2812b(string busId, int chipSelectPin)
        {
            var spiConnectionSettings = new SpiConnectionSettings(chipSelectPin);
            spiConnectionSettings.BitOrder = DataBitOrder.MSB;
            spiConnectionSettings.ClockFrequency = 6400000; // 6.666 MHz  150ns
            //spiConnectionSettings.DataBitLength = 24;
            spiConnectionSettings.Mode = SpiMode.Mode1;
            spiConnectionSettings.SharingMode = SpiSharingMode.Exclusive;
            this.spi = SpiDevice.FromId(busId, spiConnectionSettings);
            Reset();
        }

        /// <summary>
        /// Shows one pixel, assuming there is only one
        /// </summary>
        /// <param name="pixel">pixel to show</param>
        public void ShowOnePixel(Pixel pixel)
        {
            byte[] data = pixel.ToTransferBytes(bitZero, bitOne);
            if ((data == null) || (data.Length == 0))
            {
                return;
            }
            this.spi.Write(data);
            this.Reset();
        }

        /// <summary>
        /// Shows the given pixels, assuming they are in the correct order
        /// </summary>
        /// <param name="pixels"></param>
        public void ShowPixels(Pixel[] pixels)
        {
            this.ShowPixels(pixels, 0, pixels.Length);
        }

        /// <summary>
        /// Shows the given pixels, assuming they are in the correct order
        /// </summary>
        /// <param name="pixels">array of pixels</param>
        /// <param name="start">index to start</param>
        /// <param name="count">number of pixels to send</param>
        public void ShowPixels(Pixel[] pixels, int start, int count)
        {
            if ((pixels == null) || (pixels.Length == 0))
            {
                return;
            }
            if (start < 0)
            {
                start = 0;
            }
            if (start + count > pixels.Length)
            {
                count = pixels.Length - start;
            }
            int bitLenPart = 24 * bitZero.Length;
            byte[] data = new byte[pixels.Length * bitLenPart];
            int pos = 0;
            byte[] partData = null;
            Pixel onePixel = null;
            for (int i = start; i < start + count; i++)
            {
                onePixel = pixels[i];
                partData = onePixel.ToTransferBytes(bitZero, bitOne);
                if (partData == null)
                {
                    break;
                }
                Array.Copy(partData, 0, data, pos, partData.Length);
                pos = pos + bitLenPart;
                partData = null;
            }
            this.spi.Write(data);
            this.Reset();
        }

        /// <summary>
        /// Sends a pseudo-finish sequence to the wire indicating the end of transmisson
        /// </summary>
        private void Reset()
        {
            // send "low" and wait
            this.spi.Write(bitSilence);
            Thread.Sleep(1);
        }
    }
}