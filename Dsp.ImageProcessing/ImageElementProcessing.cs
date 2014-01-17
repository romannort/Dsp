using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dsp.ImageProcessing.Filters;
using Filters = Dsp.ImageProcessing.Filters;
using Dsp.ImageProcessing.Extensions;

namespace Dsp.ImageProcessing
{
    public class ImageElementProcessing
    {
        public void TresholdE(ref int[] pixelData, int fmin, int fmax)
        {
            const int gmin = 0;
            const int gmax = 255;
            Treshold(ref pixelData, fmin, gmin, fmax, gmax);
        }

        public void TresholdD(ref int[] pixelData, int gmin, int gmax)
        {
            const int fmin = 0;
            const int fmax = 255;
            Treshold(ref pixelData, fmin, gmin, fmax, gmax);
        }

        public void Treshold(ref int[] pixelData, int fmin, int gmin, int fmax, int gmax)
        {
            const Single colorsNumber = 255f;
            Single fmin0 = fmin / colorsNumber;
            Single gmin0 = gmin / colorsNumber;
            Single fmax0 = fmax / colorsNumber;
            Single gmax0 = gmax / colorsNumber;

            Func<Single, Single> treshold = f => (Single)((double)(f - fmin0) / (fmax0 - fmin0) * (gmax0 - gmin0) + gmin0);

            for (int i = 0; i < pixelData.Length; i++)
            {
                Color color = Color.FromArgb(pixelData[i]);
                Single hue = color.GetHue();
                Single saturation = color.GetSaturation();
                Single brightness = color.GetBrightness();

                Single result = treshold(brightness);
                if (result > 1) result = 1f;
                if (result < 0) result = 0f;
                // --------
                color = ColorConverters.ColorFromHsb(hue, saturation, result);

                pixelData[i] = color.ToArgb();
            }
        }


        public int[,] ApplySmooth(int[,] pixels, double weight = 2)
        {
            ConvolutionMatrix matrix = new ConvolutionMatrix(3);
            matrix.SetAll(1);
            matrix.Matrix[1, 1] = weight;
            matrix.Factor = weight + 8;

            FilterBase filter = new ConvolutionFilter() { Matrix = matrix };
            int[,] result = filter.Execute(pixels);

            return result;
        }

        public int[,] MinFilter(int[,] pixels)
        {
            int size = 3;
            FilterBase filter = new MinMaxFilter(Filters.MinMaxFilter.Mode.Min, size);
            int[,] result = filter.Execute(pixels);

            return result;
        }

        public int[,] MaxFilter(int[,] pixels)
        {
            int size = 3;
            FilterBase filter = new MinMaxFilter(Filters.MinMaxFilter.Mode.Max, size);
            int[,] result = filter.Execute(pixels);

            return result;
        }

        public int[,] MinMaxFilter(int[,] pixels)
        {
            int size = 3;
            FilterBase filterMin = new MinMaxFilter(Filters.MinMaxFilter.Mode.Min, size);
            FilterBase filterMax = new MinMaxFilter(Filters.MinMaxFilter.Mode.Max, size);
            // Chains ??
            int[,] result = filterMax.Execute(filterMin.Execute(pixels));

            return result;
        }

        public int[,] Filter(int[,] pixels, ConvolutionMatrix matrix)
        {
            FilterBase filter = new ConvolutionFilter() { Matrix = matrix };
            int[,] result = filter.Execute(pixels);

            return result;
        }
        

        public void ColorInverse(ref int[] pixelData)
        {
            for (int i = 0; i < pixelData.Length; i++)
            {
                pixelData[i] ^= 0x00ffffff;
                // ---------------A|R|G|B|
            }        
        }


        public int[,] Blur(int[,] pixels)
        {
            ConvolutionMatrix matrix = new ConvolutionMatrix(3);
            matrix.SetAll(2);
            matrix.Factor = 16;
            matrix.Offset = 0;
            
            matrix.Matrix[0, 0] = 1;
            matrix.Matrix[0, 2] = 1;
            matrix.Matrix[1, 1] = 4;
            matrix.Matrix[2, 0] = 1;
            matrix.Matrix[2, 2] = 1;

            return Filter(pixels, matrix);
        }

    }
}
