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
                    rawRed += (int)(color.GetChannel(ChannelsFromARGBExtension.RED) * coeff);
                    rawGreen += (int)(color.GetChannel(ChannelsFromARGBExtension.GREEN) * coeff);
                    rawBlue += (int)(color.GetChannel(ChannelsFromARGBExtension.BLUE) * coeff);
                }
            }

            rawRed = CorrectResult(rawRed, Matrix.Factor, Matrix.Offset);
            rawGreen = CorrectResult(rawGreen, Matrix.Factor, Matrix.Offset);
            rawBlue = CorrectResult(rawBlue, Matrix.Factor, Matrix.Offset);

            int result = BuildColorFromChannels(rawRed, rawGreen, rawBlue);
       
            return result;
        }

        private byte CorrectResult(int rawResult, double factor, double offset)
        {
            byte result = (byte)(rawResult / factor + offset);
            return result;
        }

        private static int BuildColorFromChannels(int red, int green, int blue)
        {
            const short alpha = 0xFF;
            const short alphaOffset = 24;
            const short redOffset = 16;
            const short greenOffset = 8;
            const short blueOffset = 0;

            int result =    (alpha << alphaOffset)  | 
                            (red << redOffset)      | 
                            (green << greenOffset)  | 
                            (blue << blueOffset);
            return result;
        }

    }   
}
