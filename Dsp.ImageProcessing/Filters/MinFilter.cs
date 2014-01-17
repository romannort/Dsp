using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsp.ImageProcessing.Extensions;

namespace Dsp.ImageProcessing.Filters
{
    internal class MinMaxFilter: FilterBase
    {
        #region Nested Types

        internal enum Mode
        {
            Min,
            Max
        }

        #endregion

        private Mode mode;

        /// <summary> 
        /// </summary>
        /// <param name="mode">Min or Max mode.</param>
        /// <param name="size">Size of a square to take Min/Max.</param>
        internal MinMaxFilter(Mode mode, int size = 3):
            base(new ConvolutionMatrix(size))
        {
            this.mode = mode;
        }

        protected override int OperateColors(int[,] pixelColors, ConvolutionMatrix matrix)
        {
            return MinMaxColor(pixelColors);
        }

        private int MinMaxColor(int[,] pixelColor)
        {
            int minRed = pixelColor[0, 0] & 0x00ff0000;
            int minGreen = pixelColor[0, 0] & 0x0000ff00;
            int minBlue = pixelColor[0, 0] & 0x000000ff;

            int maxRed = minRed;
            int maxGreen = minGreen;
            int maxBlue = minBlue;

            for (int i = 0; i < pixelColor.GetLength(0); ++i)
            {
                for (int j = 0; j < pixelColor.GetLength(1); ++j)
                {
                    int nextColor = pixelColor[i, j];
                    int nextRed = nextColor & 0x00FF0000;
                    int nextGreen = nextColor & 0x0000FF00;
                    int nextBlue = nextColor & 0x000000FF;
                    
                    if (this.mode == Mode.Min)
                    {
                        if (nextGreen < minGreen) minGreen = nextGreen;
                        if (nextRed < minRed) minRed = nextRed;
                        if (nextBlue < minBlue) minBlue = nextBlue;
                    }
                    else
                    {
                        if (nextGreen > maxGreen) maxGreen = nextGreen;
                        if (nextRed > maxRed) maxRed = nextRed;
                        if (nextBlue > maxBlue) maxBlue = nextBlue;
                    }
                }
            }

            if (this.mode == Mode.Min)
            {
                uint minValue = 0xFF000000 | (uint)minRed | (uint)minGreen | (uint)minBlue;
                return (int)minValue;
            }
            uint maxValue = 0xFF000000 | (uint)(maxRed) | (uint)(maxGreen) | (uint)maxBlue;
            return (int)maxValue;
        }

    }
}
