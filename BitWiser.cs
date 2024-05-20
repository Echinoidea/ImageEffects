using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
        /// Compare each pixel to comparisonColor with bitwise OR.
        /// Fast bitmap traversing -- credit: hashi, jcvandan on Stackoverflow
        /// </summary>
        /// <param name="comparisonColor">The color to compare each pixel to</param>
        /// <returns>Altered color bitmap</returns>
        public Bitmap BitwiseOR_FAST(Color comparisonColor)
        {
            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, OR(Color.FromArgb(r, g, b), comparisonColor));
                }
            }

            return output;
        }

        /// <summary>
        /// Compare each pixel to comparisonColor with bitwise AND.
        /// Fast bitmap traversing -- credit: hashi, jcvandan on Stackoverflow
        /// </summary>
        /// <param name="comparisonColor">The color to compare each pixel to</param>
        /// <returns>Altered color bitmap</returns>
        public Bitmap BitwiseAND_FAST(Color comparisonColor)
        {
            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, AND(Color.FromArgb(r, g, b), comparisonColor));
                }
            }


            return output;
        }

        /// <summary>
        /// Compare each pixel to comparisonColor with bitwise XOR.
        /// Fast bitmap traversing -- credit: hashi, jcvandan on Stackoverflow
        /// </summary>
        /// <param name="comparisonColor">The color to compare each pixel to</param>
        /// <returns>Altered color bitmap</returns>
        public Bitmap BitwiseXOR_FAST(Color comparisonColor)
        {
            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, XOR(Color.FromArgb(r, g, b), comparisonColor));
                }
            }

            return output;
        }

        public Bitmap BitwiseOR(Color comparisonColor)
        {
            Bitmap output = inputImg;

            for (int i = 0; i < this.inputImg.Width; i++)
            {
                for (int j = 0; j < this.inputImg.Height; j++)
                {
                    Color pixel = this.inputImg.GetPixel(i, j);

                    output.SetPixel(i, j, OR(pixel, comparisonColor));
                }
            }

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

        /// <summary>
        /// Perform bitwise shift left or right to each pixel.
        /// </summary>
        /// <param name="bits">How far to shift</param>
        /// <param name="direction">Direction to shift</param>
        /// <returns></returns>
        public Bitmap Shift(byte bits = 1, string direction = "left")
        {

            Bitmap output = inputImg;

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            // Declare arrays to hold the bytes of the bitmap
            int bytes = bmpData.Stride * output.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;

            output.UnlockBits(bmpData);

            for (int col = 0; col < bmpData.Height; col++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    byte r = (byte)(rgbValues[(col * stride) + (row * 3) + 2]);
                    byte g = (byte)(rgbValues[(col * stride) + (row * 3) + 1]);
                    byte b = (byte)(rgbValues[(col * stride) + (row * 3)]);

                    output.SetPixel(row, col, ShiftColor(Color.FromArgb(r, g, b), bits, direction));
                }
            }

            return output;
        }

        
    }
}
