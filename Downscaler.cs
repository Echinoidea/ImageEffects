using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.Design;
/*using System.Windows.Media;*/

namespace ImageEffects
{
    internal class Downscaler
    {
        public Bitmap inputImg;

        public Downscaler() { }

        public Downscaler(string imageFilePath)
        {
            this.inputImg = new Bitmap(imageFilePath);
        }

        public Downscaler(Bitmap imageBitmap)
        {
            this.inputImg = imageBitmap;
        }

        private Color AveragePixelBlockBruteForce(int width, int height, int x, int y)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            for (int i = 0; i < height; i++)
            {
                if (i >= this.inputImg.Height)
                {
                    i = this.inputImg.Height;
                    y = 0;
                }

                for (int j = 0; j < width; j++)
                {
                    if (j >= this.inputImg.Width)
                    {
                        j = this.inputImg.Width;
                        x = 0;
                    }
                     
                    Color pixel = this.inputImg.GetPixel(i + y, j + x);

                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;
                }
            }

            int totalPixels = width * height;
            r = r / totalPixels;
            g = g / totalPixels;
            b = b / totalPixels;

            return Color.FromArgb(r, g, b);
        }

        private Bitmap DrawAveragedPixelBlock(Bitmap image, Color color, int width, int height, int x, int y)
        {
            Bitmap output = image;
            
            for (int i = 0; i < height; i++)
            {
                if (i >= this.inputImg.Height)
                {
                    i = this.inputImg.Height - 1;
                    y = 0;
                }

                for (int j = 0; j < width; j++)
                {
                    if (j >= this.inputImg.Width)
                    {
                        j = this.inputImg.Width - 1;
                        x = 0;
                    }

                    output.SetPixel(i + y, j + x, color);
                }
            }

            return output;
        }

        public Bitmap GridAverage(int division)
        {
            // Default to just the input image
            Bitmap output = this.inputImg;

            int blockWidth = (int)Math.Floor(Convert.ToDouble(this.inputImg.Width / division));
            int blockHeight = (int)Math.Floor(Convert.ToDouble(this.inputImg.Height / division));

            for (int i = 0; i < this.inputImg.Height ; i+=blockHeight)
            {
                for (int j = 0; j < this.inputImg.Width ; j+=blockWidth)
                {
                    // Console.WriteLine("I: " + i + " J: " + j);
                    Color avgColor = AveragePixelBlockBruteForce(blockWidth, blockHeight, i, j);
                    output = DrawAveragedPixelBlock(output, avgColor, blockWidth, blockHeight, i, j);
                }
            }

            output.Save("Output.bmp");
            return output;
        }
    }
}
