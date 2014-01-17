using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsp.ImageProcessing
{
    public class HistogramDataBuilder
    {
        /// <summary> Smooth hostogram if <c>true</c>.</summary>
        public bool Smooth { get; set; }

        /// <summary> </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int[] GetBrightnessHistogram(int[] data)
        {
            int[] result = new int[256];
            IList<int> list = data.Select(Color.FromArgb)
                .Select(pixelColor => (int)Math.Floor(pixelColor.GetBrightness() * 255)).ToList();
            
            foreach (int brightness in list)
            {
                result[brightness]++;
            }

            return Smooth ? SmoothHistogram(result) : result;            
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
