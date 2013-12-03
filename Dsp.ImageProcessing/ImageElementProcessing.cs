using System;
using System.Drawing;

namespace Dsp.ImageProcessing
{
    public class ImageElementProcessing
    {
        public void TresholdD(ref int[] pixelData, int fmin, int fmax)
        {
            const int gmin = 0;
            const int gmax = 255;
            Treshold(ref pixelData, fmin, gmin, fmax, gmax);
        }

        public void TresholdE(ref int[] pixelData, int gmin, int gmax)
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
                 

                float result = treshold(brightness);
                if (result > 1) result = 1f;
                if (result < 0) result = 0f;
                // --------

                color = ColorFromHsb(hue, saturation, result);

                pixelData[i] = color.ToArgb();
            }
        }


        
        public void ColorInverse(ref int[] pixelData)
        {
            for (int i = 0; i < pixelData.Length; i++)
            {
                pixelData[i] ^= 0x00ffffff;
                // ---------------A|R|G|B|  ?? BGRA??
            }        
        }

        private static Color ColorFromHsb(float hue, float saturation, float brightness)
        {
            double r, g, b;
            if (saturation == 0)
            {
                r = g = b = brightness;
            }
            else
            {
                // the color wheel consists of 6 sectors. Figure out which sector you're in.
                double H = hue / 60.0;
                int sectorNumber = (int)(Math.Floor(H));
                // get the fractional part of the sector
                double C = (1 - Math.Abs(2 * brightness - 1)) * saturation;
                double X = C * (1 - Math.Abs(H % 2 - 1));
                double M = brightness - 0.5 * C;

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = C;
                        g = X;
                        b = 0;
                        break;
                    case 1:
                        r = X;
                        g = C;
                        b = 0;
                        break;
                    case 2:
                        r = 0;
                        g = C;
                        b = X;
                        break;
                    case 3:
                        r = 0;
                        g = X;
                        b = C;
                        break;
                    case 4:
                        r = X;
                        g = 0;
                        b = C;
                        break;
                    case 5:
                        r = C;
                        g = 0;
                        b = X;
                        break;
                    default:
                        throw  new Exception("ERROR! Wrong Hue sector: " + sectorNumber);
                }
                r += M;
                g += M;
                b += M;
            }
            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
   
    }
}
