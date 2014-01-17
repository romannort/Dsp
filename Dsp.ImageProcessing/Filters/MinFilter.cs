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
            int initialColor = pixelColor[0, 0];
            int minRed = initialColor & ChannelMasks.Red;
            int minGreen = initialColor & ChannelMasks.Green;
            int minBlue = initialColor & ChannelMasks.Blue;

            int maxRed = minRed;
            int maxGreen = minGreen;
            int maxBlue = minBlue;

            for (int i = 0; i < pixelColor.GetLength(0); ++i)
            {
                for (int j = 0; j < pixelColor.GetLength(1); ++j)
                {
                    int nextColor = pixelColor[i, j];
                    int nextRed = nextColor & ChannelMasks.Red;
                    int nextGreen = nextColor & ChannelMasks.Green;
                    int nextBlue = nextColor & ChannelMasks.Blue;
                    
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

            uint result;
            if (this.mode == Mode.Min)
            {
                result = BuildColorFromMaskedChannels(minRed, minGreen, minBlue);
            }
            else
            {
                result = BuildColorFromMaskedChannels(maxRed, maxGreen, maxBlue);
            }

            return (int)result;
        }

        private static uint BuildColorFromMaskedChannels(int red, int blue, int green)
        {
            const uint alpha = 0xFF000000;
            uint result = alpha | (uint)red | (uint)green | (uint)blue;
            return result;
        }
    }
}
