using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dsp.ImageProcessing;
using Dsp.ImageProcessing.Extensions;
using ArrayConverter = Dsp.ImageProcessing.Extensions.ArrayConverter;

namespace Dsp.ImageProcessingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        readonly ImageElementProcessing imageProcessing = new ImageElementProcessing();

        private BitmapImage originalImage;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void FilePath_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            LoadImage();
        }


        private void LoadImage()
        {
            this.originalImage = new BitmapImage();
            originalImage.BeginInit();
            originalImage.UriSource = new Uri(FilePath.Text, UriKind.Absolute);

            originalImage.EndInit();

            OriginalImage.Source = originalImage;
            ModifiedImage.Source = originalImage;


            int[] pixelData = new int[originalImage.PixelHeight*originalImage.PixelWidth];
            int widthInByte = 4*originalImage.PixelWidth;
            originalImage.CopyPixels(pixelData, widthInByte, 0);
            DrawHistogram(pixelData, "ORIGINAL");
            DrawHistogram(pixelData, "MODIFIED");
        }

        // Sample --
        private void ModifyImage(string method)
        {
            WriteableBitmap modifiedImage;
            try
            {
                BitmapSource bitmapSource = new FormatConvertedBitmap(originalImage, 
                    PixelFormats.Bgra32, 
                    null, 0);
                modifiedImage = new WriteableBitmap(bitmapSource);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            int height = modifiedImage.PixelHeight;
            int width = modifiedImage.PixelWidth;
            int[] pixelData = new int[width*height];
            int widthInByte = 4*width;

            modifiedImage.CopyPixels(pixelData, widthInByte, 0);

            // ----------- Extern call here --
            int[,] pixels;
            switch (method)
            {
                case "D":
                    double gmin = 0;
                    Double.TryParse(GMin.Text, out gmin);
                    double gmax = 255;
                    Double.TryParse(GMax.Text, out gmax);
                    
                    imageProcessing.TresholdD(ref pixelData, (int)gmin, (int)gmax);
                    break;

                case "E":
                    
                    double fmin = 0;
                    double fmax = 255;
                    Double.TryParse(FMin.Text, out fmin);
                    Double.TryParse(FMax.Text, out fmax);
                    
                    imageProcessing.TresholdE(ref pixelData, (int)fmin, (int)fmax);
                    break;

                case "MIN":
                    pixels = ArrayConverter.To2D(pixelData, modifiedImage.PixelWidth, modifiedImage.PixelHeight);
                    pixels = imageProcessing.MinFilter(pixels);
                    pixelData = ArrayConverter.ToLinear(pixels);
                    break;

                case "MAX":
                    pixels = ArrayConverter.To2D(pixelData, modifiedImage.PixelWidth, modifiedImage.PixelHeight);
                    pixels = imageProcessing.MaxFilter(pixels);
                    pixelData = ArrayConverter.ToLinear(pixels);

                    break;

                case "MINMAX":
                    pixels = ArrayConverter.To2D(pixelData, modifiedImage.PixelWidth, modifiedImage.PixelHeight);
                    pixels = imageProcessing.MinMaxFilter(pixels);
                    pixelData = ArrayConverter.ToLinear(pixels);

                    break;

                case "BLUR":
                    pixels = ArrayConverter.To2D(pixelData, modifiedImage.PixelWidth, modifiedImage.PixelHeight);
                    pixels = imageProcessing.Blur(pixels);
                    pixelData = ArrayConverter.ToLinear(pixels);
                    break;

                default:
                    break;
            }
            // --------------------------------

            modifiedImage.WritePixels(new Int32Rect(0, 0, width, height), pixelData, widthInByte, 0);
            ModifiedImage.Source = modifiedImage;

            DrawHistogram(pixelData, "MODIFIED");
        }

        private void TresholdD_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("D");
        }

        private void TresholdE_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("E");
        }

        private void MinFilter_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("MIN");
        }

        private void MaxFilter_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("MAX");
        }

        private void MinMaxFilter_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("MINMAX");
        }


        private void DrawHistogram(int[] values, string image)
        {
            int[] imageHistogram = imageProcessing.GetBrightnessHistogram(values);
            PointCollection points = imageHistogram.ToPointCollection();
            // T_T
            if (image == "ORIGINAL")
            {
                OriginalImageHistogramPolygon.Points = points;
            }
            else
            {
                ModifiedImageHistogramPolygon.Points = points;
            }
        }

        private void BlurFilter_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("BLUR");
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Slider slider = sender as Slider;
            //if (slider != null)
            //{
            //    string param = slider.Tag as String;
            //    ModifyImage(param);
            //}
        }
    }
}