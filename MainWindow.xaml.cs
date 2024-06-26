﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using RadioButton = System.Windows.Controls.RadioButton;
using Image = System.Drawing.Image;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;


namespace ImageEffects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Effector instances
        private Downscaler downscaler;
        private PixelSorter pixelSorter;
        private BitWiser bitwiser;

        // Files
        private string selectedFile;
        private string saveToPath;
        private Bitmap outputImage;

        public MainWindow()
        {
            InitializeComponent();
            MapColorsToComboBoxes();
        }


        #region UTILITY METHODS


        /// <summary>
        /// Convert a System.Drawing.Bitmap to a BitmapSource to be displayed as image source on the app.
        /// </summary>
        /// <param name="bitmap">The Bitmap image to convert to BitmapSource object</param>
        /// <returns>BitmapSource</returns>
        private BitmapSource ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        /// <summary>
        /// Janky temporary fix for bad downscaler bitmap traversal exceeding array bounds.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private static int GetClosestFactor(int target, int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (number % (target + i) == 0)
                {
                    return target + i;
                }
                else if (number % (target - i) == 0)
                {
                    return target - i;
                }
            }
            return number;
        }


        IEnumerable<System.Drawing.Color> GetSystemColors()
        {
            Type type = typeof(System.Drawing.Color);
            return type.GetProperties().Where(info => info.PropertyType == type).Select(info => (System.Drawing.Color)info.GetValue(null, null));
        }

        private void MapColorsToComboBoxes()
        {
            ComboColors.ItemsSource = GetSystemColors();
            ComboColorsVariant.ItemsSource = GetSystemColors();
        }

        /// <summary>
        /// Set each effector instance's image property to be the last outputted image.
        /// </summary>
        /// <param name="output">The last outputted image</param>
        private void UpdateEffectorsToOutputImg(Bitmap output)
        {
            this.downscaler = new Downscaler(output);
            this.pixelSorter = new PixelSorter(output);
            this.bitwiser = new BitWiser(output);
        }

        /// <summary>
        /// Reset each effector instance to load the original image from the file path.
        /// </summary>
        /// <param name="filepath">Input image file path</param>
        private void ResetEffectorsToOriginal(string filepath)
        {
            this.downscaler = new Downscaler(filepath);
            this.pixelSorter = new PixelSorter(filepath);
            this.bitwiser = new BitWiser(filepath);
        }

        private Bitmap LoadImageFromPath(string filepath)
        {
            return new Bitmap(filepath);
        }

        /// <summary>
        /// Update the image component on the app.
        /// </summary>
        /// <param name="image">The image to convert to a BitmapSource and then display</param>
        private void UpdateImage(Bitmap image)
        {
            ImgOutput.Source = ConvertBitmap(image);
        }

        private List<Bitmap> GenerateVariants(string operation)
        {
            List<Bitmap> variants = new List<Bitmap>();

            for (int variant = 1; variant < 65; variant++)
            {
                ResetEffectorsToOriginal(selectedFile);
                if (operation == "OR")
                {
                    variants.Add(this.bitwiser.BitwiseOR_Variant((System.Drawing.Color)ComboColorsVariant.SelectedItem, variant));
                }
                else if (operation == "AND")
                {
                    variants.Add(this.bitwiser.BitwiseAND_Variant((System.Drawing.Color)ComboColorsVariant.SelectedItem, variant));
                }
                else if (operation == "XOR")
                {
                    variants.Add(this.bitwiser.BitwiseXOR_Variant((System.Drawing.Color)ComboColorsVariant.SelectedItem, variant));
                }
                else
                {
                    Console.WriteLine("No operation selected");
                }
            }

            return variants;
        }


        #endregion

        #region COMPONENT EVENT HANDLERS

        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dlg.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                LblSelectedFile.Content = filename;
                Console.WriteLine(filename);
                selectedFile = filename;

                BtnSave.IsEnabled = true;

                // Open image from path name
                ResetEffectorsToOriginal(selectedFile);
                UpdateImage(LoadImageFromPath(selectedFile));
            }
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            var selectedEffect = MainGrid.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.GroupName == "Effect" && r.IsChecked.Value).Content;

            var selectedSortType = MainGrid.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.GroupName == "PixelSortType" && r.IsChecked.Value).Content;

            var selectedShiftDirection = MainGrid.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.GroupName == "ShiftDirection" && r.IsChecked.Value).Content;
            byte bits = (byte)Int16.Parse(TxtBoxBits.Text);

            var selectedBitwiseOperation = MainGrid.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.GroupName == "BitwiseOperation" && r.IsChecked.Value).Content;
            var selectedBitwiseOperationVariant = MainGrid.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.GroupName == "BitwiseOperationVariant" && r.IsChecked.Value).Content;
            int variant = (int)Int16.Parse(TxtBoxVariant.Text);

            switch (selectedEffect)
            {
                case "Downscale":
                    outputImage = downscaler.GridAverage(GetClosestFactor(Convert.ToInt32(TxtBoxDownscaleNum.Text), this.downscaler.inputImg.Width));
                    break;

                case "Pixel Sort":
                    if (selectedSortType.ToString() == "Vertical")
                    {
                        outputImage = pixelSorter.SortVertical(GetClosestFactor(Convert.ToInt32(TxtBoxPixelSortCols.Text), this.pixelSorter.inputImg.Width));
                    }
                    else if (selectedSortType.ToString() == "Horizontal")
                    {
                        outputImage = pixelSorter.SortHorizontal(GetClosestFactor(Convert.ToInt32(TxtBoxPixelSortCols.Text), this.pixelSorter.inputImg.Width));
                    }
                    break;

                case "Bit Shift":
                    if (selectedShiftDirection.ToString() == "Left Shift")
                    {
                        outputImage = bitwiser.Shift(bits, "left");
                    }
                    else if (selectedShiftDirection.ToString() == "Right Shift")
                    {
                        outputImage = bitwiser.Shift(bits, "right");
                    }
                    break;

                case "Bitwise":
                    if (selectedBitwiseOperation.ToString() == "OR")
                    {
                        outputImage = bitwiser.BitwiseOR((System.Drawing.Color)ComboColors.SelectedItem);
                    }
                    else if (selectedBitwiseOperation.ToString() == "AND")
                    {
                        outputImage = bitwiser.BitwiseAND((System.Drawing.Color)ComboColors.SelectedItem);
                    }
                    else if (selectedBitwiseOperation.ToString() == "XOR")
                    {
                        outputImage = bitwiser.BitwiseXOR((System.Drawing.Color)ComboColors.SelectedItem);
                    }
                    break;

                case "Bitwise Variants":
                    if (selectedBitwiseOperationVariant.ToString() == "OR")
                    {
                        outputImage = bitwiser.BitwiseOR_Variant((System.Drawing.Color)ComboColorsVariant.SelectedItem, variant);
                    }
                    else if (selectedBitwiseOperationVariant.ToString() == "AND")
                    {
                        outputImage = bitwiser.BitwiseAND_Variant((System.Drawing.Color)ComboColorsVariant.SelectedItem, variant);
                    }
                    else if (selectedBitwiseOperationVariant.ToString() == "XOR")
                    {
                        outputImage = bitwiser.BitwiseXOR_Variant((System.Drawing.Color)ComboColorsVariant.SelectedItem, variant);
                    }
                    break;
            }

            // TODO: Turn this block into a method called update effectors or something

            UpdateEffectorsToOutputImg(outputImage);
            UpdateImage(outputImage);
            BtnSave.IsEnabled = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Opening Dialog");
            Stream myStream;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Title = "Save picture as ";
            saveFileDialog1.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        // Code to write the stream goes here.
                        JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                        jpg.Frames.Add(BitmapFrame.Create(ConvertBitmap(outputImage)));
                        jpg.Save(myStream);
                        myStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                
            }

            Console.WriteLine("Image saved");
        }

        private void BtnSaveVariants_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Opening Folder Browser Dialog");

            var selectedBitwiseOperationVariant = MainGrid.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.GroupName == "BitwiseOperationVariant" && r.IsChecked.Value).Content;

            using (var folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();
                Console.WriteLine("Result");

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string folderPath = folderDialog.SelectedPath;

                    List<Bitmap> bmp = GenerateVariants((string)selectedBitwiseOperationVariant);

                    // Iterate through the list of bitmaps
                    foreach (Bitmap bitmap in bmp)
                    {
                        // Generate a unique filename for each bitmap (you can adjust this as needed)
                        string fileName = $"bitmap_{DateTime.Now.Ticks}.bmp";
                        string filePath = System.IO.Path.Combine(folderPath, fileName);

                        // Save the bitmap to the selected folder
                        bitmap.Save(filePath);
                    }

                    System.Windows.MessageBox.Show("Bitmaps saved successfully.");
                }
            }
        }


        private void RadBtnHorizontal_Checked(object sender, RoutedEventArgs e) { }

        private void RadBtnVertical_Checked(object sender, RoutedEventArgs e) { }

        

        private void ChkBoxDownscale_Checked(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = true;
            BtnReset.IsEnabled = true;
            BtnSaveVariants.IsEnabled = false;
        }

        private void ChkBoxPixelSort_Checked(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = true;
            BtnReset.IsEnabled = true;
            BtnSaveVariants.IsEnabled = false;
        }

        private void RadBtnBitShift_Checked(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = true;
            BtnReset.IsEnabled = true;
            BtnSaveVariants.IsEnabled = false;
        }

        private void RadBtnBitWise_Checked(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = true;
            BtnReset.IsEnabled = true;
            BtnSaveVariants.IsEnabled = false;
        }

        private void RadBtnBitWiseVariants_Checked(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = true;
            BtnReset.IsEnabled = true;
            BtnSaveVariants.IsEnabled = true;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetEffectorsToOriginal(selectedFile);
            UpdateImage(LoadImageFromPath(selectedFile));
        }

        #endregion
    }
}
