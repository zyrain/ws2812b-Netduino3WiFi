using System;

namespace Ws2812b_Netduino3WiFi
{
    /// <summary>
    /// Class representing one pixel<br />
    /// Highly inspired by frank26080115's NeoPixel class on NeoPixel-on-NetduinoPlus2 @ github
    /// </summary>
    public class Pixel
    {
        public static Pixel Black = new Pixel(0,0,0);
        public static Pixel White = new Pixel(255, 255, 255);

        public static Pixel Red = new Pixel(r:255, g:0, b:0);
        public static Pixel Green = new Pixel(r:0, g:255, b:0);
        public static Pixel Blue = new Pixel(r:0, g:0, b:255);

        public static Pixel Cyan = new Pixel(r: 0, g: 255, b: 255);
        public static Pixel Yellow = new Pixel(r: 255, g: 255, b: 0);
        public static Pixel Magenta = new Pixel(r: 255, g: 0, b: 255);


        /// <summary>
        /// Green, 0 to 255
        /// </summary>
        public byte G
        {
            get;
            set;
        }

        /// <summary>
        /// Red, 0 to 255
        /// </summary>
        public byte R
        {
            get;
            set;
        }

        /// <summary>
        /// Blue, 0 to 255
        /// </summary>
        public byte B
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new pixel, black
        /// </summary>
        public Pixel()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Creates a new pixel with given color
        /// </summary>
        /// <param name="r">Initial red, 0 to 255</param>
        /// <param name="g">Initial green, 0 to 255</param>
        /// <param name="b">Initial blue, 0 to 255</param>
        public Pixel(byte r, byte g, byte b)
        {
            this.G = g;
            this.R = r;
            this.B = b;
        }

        /// <summary>
        /// Creates a new pixel with given color
        /// </summary>
        /// <param name="r">Initial red, 0 to 255</param>
        /// <param name="g">Initial green, 0 to 255</param>
        /// <param name="b">Initial blue, 0 to 255</param>
        public Pixel(int r, int g, int b) : this((byte)r, (byte)g, (byte)b) { }

        /// <summary>
        /// Creates a new pixel with given ARGB color, where A is ignored
        /// </summary>
        /// <param name="argb">ARGB color value</param>
        public Pixel(int argb) : this((byte)(argb & 0x00FF0000), (byte)(argb & 0x0000FF00), (byte) (argb & 0x000000FF)) { }

        /// <summary>
        /// Creates the bytes needed for transfer via SPI in GRB format<br />
        /// Make sure that zero and one bytes have the same length and are properly initialized
        /// </summary>
        /// <param name="zeroBytes">bytes for zero bit</param>
        /// <param name="oneBytes">bytes for one bit</param>
        /// <returns>transfer bytes</returns>
        public byte[] ToTransferBytes(byte[] zeroBytes, byte[] oneBytes)
        {
            if ((zeroBytes == null) || (zeroBytes.Length == 0) || (oneBytes == null) || (oneBytes.Length == 0))
            {
                return new byte[0];
            }
            int len = zeroBytes.Length;
            if (oneBytes.Length != len)
            {
                return new byte[0];
            }

            byte[] result = new byte[24 * len];

            int pos = 0;
            byte msk;

            msk = 128;
            for (int i = 7; i >= 0; i--)
            {
                byte v = (byte)(this.G & msk);
                if (v > 0)
                {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else
                {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            msk = 128;
            for (int i = 7; i >= 0; i--)
            {
                byte v = (byte)(this.R & msk);
                if (v > 0)
                {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else
                {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            msk = 128;
            for (int i = 7; i >= 0; i--)
            {
                byte v = (byte)(this.B & msk);
                if (v > 0)
                {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else
                {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            return result;
        }
    }
}