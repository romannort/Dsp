using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Func<int, int> treshold = f => (int)((double)(f - fmin)/(fmax - fmin)*(gmax - gmin) + gmin);

            for (int i = 0; i < pixelData.Length; i++)
            {
                
                Channels c = new Channels(pixelData[i]);

                byte quantizatedBrightness = (byte)(c.Brightness*255);
                byte quantizatedResult = ChannelTreshold(treshold, quantizatedBrightness);
                c.Brightness = quantizatedResult/255f;

                pixelData[i] = c.ToColour();
            }
        }


        private static byte ChannelTreshold(Func<int, int> treshold,  byte c)
        {
            int g = treshold(c);
            if (g > 255)
            {
                g = 255;
            }
            if (g < 0)
            {
                g = 0;
            }
            return (byte)g;
        }

        public void ColorInverse(ref int[] pixelData)
        {
            for (int i = 0; i < pixelData.Length; i++)
            {
                pixelData[i] ^= 0x00ffffff;
                // ---------------A|R|G|B|  ?? BGRA??
            }        
        }

        
    }


    internal class Channels
    {
        internal Byte A;
        internal Byte R;
        internal Byte G;
        internal Byte B;

        internal Single Hue;
        internal Single Saturation;
        internal Single Brightness;

        internal Channels(){}

        internal Channels(Int32 fullColour)
        {
            B = (Byte) ((fullColour & 0x11000000) >> 24);
            G = (byte) ((fullColour & 0x00110000) >> 16);
            R = (byte) ((fullColour & 0x00001100) >> 8);
            A = (Byte) (fullColour & 0x00000011);

            Color color = Color.FromArgb(A, R, G, B);
            Hue = color.GetHue();
            Saturation = color.GetSaturation();
            Brightness = color.GetBrightness();
        }

        internal Int32 ToColour()
        {
            HsbToRgb();

            Int32 colour = ((Byte)(B) << 24)
                | ((Byte)(G) << 16)
                | ((Byte)(R) << 8)
                | A;
            
            return colour;
        }

        private void HsbToRgb()
        {

            float hue = Hue;
            float saturation = Saturation;
            float brightness = Brightness;

            double r = 0, g = 0, b = 0;
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
                        Console.WriteLine("ERROR! Wrong Hue sector: " + sectorNumber);
                        break;
                }
                r += M;
                g += M;
                b += M;
            }
            
            // ------
            R = (byte)(r * 255);
            G = (byte)(g * 255);
            B = (byte)(b * 255);
        }
    }



}
