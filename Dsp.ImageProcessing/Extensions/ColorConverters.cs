﻿using System.Drawing;
using System;

namespace Dsp.ImageProcessing.Extensions
{

    internal static class ColorConverters
    {
        
        internal static Color ColorFromHsb(float hue, float saturation, float brightness)
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
                        throw new Exception("Wrong Hue sector: " + sectorNumber);
                }
                r += M;
                g += M;
                b += M;
            }
            return  Color.FromArgb(
                (int)(r * 255), 
                (int)(g * 255), 
                (int)(b * 255));
            
        }
    }
}
