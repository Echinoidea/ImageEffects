using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace ImageEffects
{
/*    internal enum SortMethod
    {
        NAIVE,
        HSV,
        LUMINOSITY,
        STEP,
        HILBERT,
        TRAVELLINGSALESMAN
    }*/

    internal class PixelSorter
    {
        public Bitmap inputImg;

        public PixelSorter() { }

        public PixelSorter(string imageFilePath)
        {
            this.inputImg = new Bitmap(imageFilePath);
        }

        public PixelSorter(Bitmap imageBitmap)
        {
            this.inputImg = imageBitmap;
        }

        /*private int ColorToHSV(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            double hue = color.GetHue();
            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = (max / 255d);

            return (int)value; 
        }*/

        private List<Color> SortRGB(List<Color> unsorted)
        {
            List<Color> sorted = new List<Color>();

            sorted = unsorted.OrderBy(i => i.R).
                ThenBy(i => i.G).
                ThenBy(i => i.B)
                .ToList();

            return sorted;
        }

        // Implement different sorting methods https://www.alanzucconi.com/2015/09/30/colour-sorting/
        /*private List<Color> SortByColor(List<Color> unsorted, SortMethod method = SortMethod.HSV)
        {
            List<Color> sorted = new List<Color>();

            return sorted;
        }*/

        private Bitmap DrawSortedColumn(Bitmap image, int col, List<Color> sorted)
        {
            Bitmap sortedImage = image;

            for (int i = 0; i < sorted.Count; i++)
            {
                sortedImage.SetPixel(col, i, sorted[i]);
            }

            return sortedImage;
        }

        public Bitmap SortVertical(int division)
        {
            Bitmap output = this.inputImg;

            List<Color> unsorted = new List<Color>();
            List<Color> sorted = new List<Color>();

            int chunkSize = (int)Math.Floor(Convert.ToDouble(this.inputImg.Height / division));

            for (int i = 0; i < this.inputImg.Width; i++)
            {
                for (int j = 0;  j < this.inputImg.Height; j++)
                {
                    if (unsorted.Count >= chunkSize)
                    {
                        // Sort the unsorted list of n pixels
                        sorted.AddRange(SortRGB(unsorted)); // Add the sorted unsorted list to sorted
                        unsorted.Clear(); // Clear the unsorted list and continue to add to it and repeat this
                        continue;
                    }

                    unsorted.Add(this.inputImg.GetPixel(i, j));
                }

                sorted.AddRange(SortRGB(unsorted));
                output = DrawSortedColumn(output, i, sorted);
                unsorted.Clear();
                sorted.Clear();
            }

            /*output.Save("Sorted.bmp");*/
            return output;
        }

        private Bitmap DrawSortedRow(Bitmap image, int row, List<Color> sorted)
        {
            Bitmap sortedImage = image;

            for (int i = 0; i < sorted.Count; i++)
            {
                sortedImage.SetPixel(i, row, sorted[i]);
            }

            return sortedImage;
        }

        public Bitmap SortHorizontal(int division)
        {
            Bitmap output = this.inputImg;

            List<Color> unsorted = new List<Color>();
            List<Color> sorted = new List<Color>();

            int chunkSize = (int)Math.Floor(Convert.ToDouble(this.inputImg.Height / division));

            for (int i = 0; i < this.inputImg.Height; i++)
            {
                for (int j = 0; j < this.inputImg.Width; j++)
                {
                    if (unsorted.Count >= chunkSize)
                    {
                        // Sort the unsorted list of n pixels
                        sorted.AddRange(SortRGB(unsorted)); // Add the sorted unsorted list to sorted
                        unsorted.Clear(); // Clear the unsorted list and continue to add to it and repeat this
                        continue;
                    }

                    unsorted.Add(this.inputImg.GetPixel(j, i));
                }

                sorted.AddRange(SortRGB(unsorted));
                output = DrawSortedRow(output, i, sorted);
                unsorted.Clear();
                sorted.Clear();
            }

            /*output.Save("Sorted.bmp");*/
            return output;
        }
    }
}
