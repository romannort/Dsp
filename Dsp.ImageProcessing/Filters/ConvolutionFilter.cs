using System.Drawing;
using System.Linq;
using Dsp.ImageProcessing.Extensions;

namespace Dsp.ImageProcessing.Filters
{
    internal class ConvolutionFilter : FilterBase
    {
        protected override int OperateColors(int[,] pixelColors, ConvolutionMatrix matrix)
        {
            int rawRed = 0;
            int rawGreen = 0;
            int rawBlue = 0;

            for (int i = 0; i < matrix.Size; ++i)
            {
                for (int j = 0; j < matrix.Size; ++j)
                {
                    int color = pixelColors[i, j];
                    double coeff = matrix.Matrix[i, j];
                    rawRed += (int)(color.GetChannel(ChannelsExtension.RED) * coeff);
                    rawGreen += (int)(color.GetChannel(ChannelsExtension.GREEN) * coeff);
                    rawBlue += (int)(color.GetChannel(ChannelsExtension.BLUE) * coeff);
                }
            }

            rawRed = CorrectResult(rawRed, Matrix.Factor, Matrix.Offset);
            rawGreen = CorrectResult(rawGreen, Matrix.Factor, Matrix.Offset);
            rawBlue = CorrectResult(rawBlue, Matrix.Factor, Matrix.Offset);

            int result = (0xff << 24) | (rawRed << 16) | (rawGreen << 8) | rawBlue;
       
            return result;
        }

        private byte CorrectResult(int rawResult, double factor, double offset)
        {
            byte result = (byte)(rawResult / factor + offset);
            return result;
        }

    }

    /// <summary></summary>
    public static class ChannelsExtension
    {
        public static short RED = 16;

        public static short GREEN = 8;

        public static short BLUE = 0;

        public static int GetChannel(this int color, short offset)
        {
            return (color >> offset) & 0xFF;
        }
    }
}
