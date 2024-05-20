using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.Diagnostics;

namespace ImageEffects
{
    internal class BitWiser
    {
        public Bitmap inputImg;

        public BitWiser() { }

        public BitWiser(string imageFilePath)
        {
            this.inputImg = new Bitmap(imageFilePath);
        }

        public BitWiser(Bitmap imageBitmap)
        {
            this.inputImg = imageBitmap;
        }

        private Color ComplimentColor(Color color, byte bits)
        {
            Color output = color;
            byte r = color.R; byte g = color.G; byte b = color.B;

            return Color.FromArgb(~r, ~g, ~b);
        }

        private Color OR(Color color1, Color color2)
        {
            Color output = color1;
            byte r1 = color1.R; byte g1 = color1.G; byte b1 = color1.B;
            byte r2 = color2.R; byte g2 = color2.G; byte b2 = color2.B;



            return Color.FromArgb(r1 | r2, g1 | g2, b1 | b2);
        }

        private Color AND(Color color1, Color color2)
        {
            Color output = color1;
            byte r1 = color1.R; byte g1 = color1.G; byte b1 = color1.B;
            byte r2 = color2.R; byte g2 = color2.G; byte b2 = color2.B;



            return Color.FromArgb(r1 & r2, g1 & g2, b1 & b2);
        }

        private Color XOR(Color color1, Color color2)
        {
            Color output = color1;
            byte r1 = color1.R; byte g1 = color1.G; byte b1 = color1.B;
            byte r2 = color2.R; byte g2 = color2.G; byte b2 = color2.B;



            return Color.FromArgb(r1 ^ r2, g1 ^ g2, b1 ^ b2);
        }

        private Color ShiftColor(Color color, byte bits, string direction = "left")
        {
            Color output = color;
            byte r = color.R; byte g = color.G; byte b = color.B;

            if (direction == "left")
            {
                return Color.FromArgb(r << bits < 255 ? r << bits : 255, g << bits < 255 ? g << bits : 255, b << bits < 255 ? b << bits : 255);
            }
            else if (direction == "right")
            {
                return Color.FromArgb(r >> bits < 255 ? r >> bits : 255, g >> bits < 255 ? g >> bits : 255, b >> bits < 255 ? b >> bits : 255);
            }
            else
            {
                return output;
            }
        }


        /// <summary>
        /// FAST Bitwise OR operations on all pixels. Runs about twice as fast as naive.
        /// Credit: hashi, jcvandan on Stackoverflow
        /// </summary>
        /// <param name="comparisonColor"></param>
        /// <returns></returns>
        public Bitmap BitwiseOR_FAST(Color comparisonColor)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] r = new byte[bytes / 3];
            byte[] g = new byte[bytes / 3];
            byte[] b = new byte[bytes / 3];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte _r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte _g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte _b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, OR(Color.FromArgb(_r, _g, _b), comparisonColor));
                }
            }


            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("FAST OR {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            return output;
        }

        public Bitmap BitwiseAND_FAST(Color comparisonColor)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] r = new byte[bytes / 3];
            byte[] g = new byte[bytes / 3];
            byte[] b = new byte[bytes / 3];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte _r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte _g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte _b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, AND(Color.FromArgb(_r, _g, _b), comparisonColor));
                }
            }


            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("FAST AND {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            return output;
        }

        public Bitmap BitwiseXOR_FAST(Color comparisonColor)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] r = new byte[bytes / 3];
            byte[] g = new byte[bytes / 3];
            byte[] b = new byte[bytes / 3];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte _r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte _g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte _b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, XOR(Color.FromArgb(_r, _g, _b), comparisonColor));
                }
            }


            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("FAST XOR {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            return output;
        }

        public Bitmap BitwiseOR(Color comparisonColor)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap output = inputImg;

            for (int i = 0; i < this.inputImg.Width; i++)
            {
                for (int j = 0; j < this.inputImg.Height; j++)
                {
                    Color pixel = this.inputImg.GetPixel(i, j);

                    output.SetPixel(i, j, OR(pixel, comparisonColor));
                }
            }

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("REGULAR OR {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            return output;
        }

        public Bitmap BitwiseAND(Color comparisonColor)
        {
            Bitmap output = inputImg;

            for (int i = 0; i < this.inputImg.Width; i++)
            {
                for (int j = 0; j < this.inputImg.Height; j++)
                {
                    Color pixel = this.inputImg.GetPixel(i, j);

                    output.SetPixel(i, j, AND(pixel, comparisonColor));
                }
            }

            return output;
        }

        public Bitmap BitwiseXOR(Color comparisonColor)
        {
            Bitmap output = inputImg;

            for (int i = 0; i < this.inputImg.Width; i++)
            {
                for (int j = 0; j < this.inputImg.Height; j++)
                {
                    Color pixel = this.inputImg.GetPixel(i, j);

                    output.SetPixel(i, j, XOR(pixel, comparisonColor));
                }
            }

            return output;
        }

        public Bitmap Shift(byte bits = 1, string direction = "left")
        {

            Bitmap output = inputImg;

            for (int i = 0; i < this.inputImg.Width; i++)
            {
                for (int j = 0; j < this.inputImg.Height; j++)
                {
                    Color pixel = this.inputImg.GetPixel(i, j);

                    output.SetPixel(i, j, ShiftColor(pixel, bits, direction));
                }
            }

            return output;
        }

        
    }
}
