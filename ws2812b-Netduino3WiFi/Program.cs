using System;
using System.Diagnostics;
using System.Threading;

namespace Ws2812b_Netduino3WiFi
{
    public class Program
    {
        public static void Main()
        {
            Netduino3WiFi netduino = new Netduino3WiFi();

            Ws2812b lightStrip = new Ws2812b(Netduino3WiFi.SPI_BUS_ID, Netduino3WiFi.CS_PIN);
			
            Pixel[] allWhite = new Pixel[60];
            for (var i = 0; i < 60; i++)
            {
                allWhite[i] = Pixel.White ;
            }

            Pixel[] ribbon = new Pixel[60];
            var halfCyan = new Pixel(0, 128, 128);
            var leaf = new Pixel(r:0, g:5, b:1);
            for (var i = 0; i < 30; i++)
            {
                ribbon[2*i] = halfCyan;
                ribbon[2*i+1] = leaf;
            }

            while (true)
            {
                netduino.OnBoardLed = true;
                lightStrip.ShowPixels(allWhite);
                Thread.Sleep(1000);

                netduino.OnBoardLed = false;
                lightStrip.ShowPixels(ribbon);
                Thread.Sleep(1000);
            }
        }
    }
}


