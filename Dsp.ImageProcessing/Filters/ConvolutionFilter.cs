using System.Drawing;
using System.Linq;
using Dsp.ImageProcessing.Extensions;

namespace Dsp.ImageProcessing.Filters
{
    public class ConvolutionFilter: FilterBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelColor"></param>
        /// <param name="matrix"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual int OperateColor(Color[,] pixelColor, ConvolutionMatrix matrix, ColorCode code)
        {
            double rawResult = 0;

            for (int i = 0; i < matrix.Size; ++i)
            {
                for (int j = 0; j < matrix.Size; ++j)
                {
                    rawResult += (pixelColor[i, j].ColorChannelByCode(code) * matrix.Matrix[i, j]);
                }
            }

            int result = (int)(rawResult / matrix.Factor + matrix.Offset);
            if (result < 0)
            {
                result = 0;
            }
            else if (result > 255)
            {
                result = 255;
            }

            return result;
        }
    }

}
