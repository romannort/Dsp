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
                // ---------------A|R|G|B|  ?? BGRA??
            }        
        }

        
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int[] GetBrightnessHistogram(int[] data)
        {
            int [] result = new int[256];
            foreach (int brightness in data.Select(Color.FromArgb)
                .Select(pixelColor => (int) Math.Floor(pixelColor.GetBrightness()*255)))
            {
                result[brightness]++;
            }
            return SmoothHistogram(result);
        }


        private static int[] SmoothHistogram(IList<int> originalValues)
        {
            int[] smoothedValues = new int[originalValues.Count];

            double[] mask = new[] { 0.25, 0.5, 0.25 };

            for (int bin = 1; bin < originalValues.Count - 1; bin++)
            {
                double smoothedValue = 0;
                for (int i = 0; i < mask.Length; i++)
                {
                    smoothedValue += originalValues[bin - 1 + i] * mask[i];
                }
                smoothedValues[bin] = (int)smoothedValue;
            }

            return smoothedValues;
        }
    }
}
