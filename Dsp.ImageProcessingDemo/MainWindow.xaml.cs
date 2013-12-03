using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dsp.ImageProcessing;
using Dsp.ImageProcessing.Extensions;

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
            if (method == "D")
            {
                int fmin = 0;
                int fmax = 255;
                Int32.TryParse(FMin.Text, out fmin);
                Int32.TryParse(FMax.Text, out fmax);
                imageProcessing.TresholdD(ref pixelData, fmin, fmax);
            }
            else if ("E" == method)
            {
                int gmin = 0;
                Int32.TryParse(GMin.Text, out gmin);
                int gmax = 255;
                Int32.TryParse(GMax.Text, out gmax);
                imageProcessing.TresholdE(ref pixelData, gmin, gmax);    
            }
            else if (method == "Filter")
            {
                int[,] pixels = ArrayConverter.To2D(pixelData, modifiedImage.PixelWidth, modifiedImage.PixelHeight);
                //pixels = imageProcessing.ApplySmooth(pixels);
                pixels = imageProcessing.MinFilter(pixels);
                pixelData = ArrayConverter.ToLinear(pixels);
            }
            // --------------------------------

            modifiedImage.WritePixels(new Int32Rect(0, 0, width, height), pixelData, widthInByte, 0);

            ModifiedImage.Source = modifiedImage;
        }

        private void TresholdD_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("D");
        }

        private void TresholdE_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("E");
        }

        private void FilterTest_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyImage("Filter");
        }
    }
}


 //«Ваша команда будет как-то участвовать в Олимпиаде?» Я и ответил: «Наша команда в полном составе придет и поддержит биатлонистов».