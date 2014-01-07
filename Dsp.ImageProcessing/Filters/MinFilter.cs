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
        /// 
        /// </summary>
        /// <param name="mode">Min or Max mode.</param>
        /// <param name="size">Size of a square to take Min/Max.</param>
        internal MinMaxFilter(Mode mode, int size = 3):
            base(new ConvolutionMatrix(size))
        {
            this.mode = mode;
        }

        protected override int OperateColor(Color[,] pixelColors, ConvolutionMatrix matrix, ColorCode code)
        {
            return MinMaxColor(pixelColors, code);
        }

        private int MinMaxColor(Color[,] pixelColor, ColorCode color)
        {
            int minValue = pixelColor[0, 0].ColorChannelByCode(color);
            int maxValue = minValue;
            for (int i = 0; i < pixelColor.GetLength(0); ++i)
            {
                for (int j = 0; j < pixelColor.GetLength(1); ++j)
                {
                    int nextColor = pixelColor[i, j].ColorChannelByCode(color);
                    if (nextColor < minValue) minValue = nextColor;
                    if (nextColor > maxValue) maxValue = nextColor;
                }
            }

            if (this.mode == Mode.Min) return minValue;
            if (this.mode == Mode.Max) return maxValue;

            return 0;
        }

    }
}
