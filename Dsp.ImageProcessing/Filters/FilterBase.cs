using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsp.ImageProcessing.Extensions;

namespace Dsp.ImageProcessing.Filters
{
    internal abstract class FilterBase
    {
        internal ConvolutionMatrix Matrix { get; set; }

        public FilterBase()
        {

        }

        public FilterBase(ConvolutionMatrix matrix)
        {
            Matrix = matrix;
        }

        public virtual int[,] Execute(int[,] pixels)
        {
            int[,] clonedPixels = (int[,])pixels.Clone();
           
            int width = pixels.GetLength(1);
            int height = pixels.GetLength(0);

            int borderOffset = (int)System.Math.Ceiling(Matrix.Size / 2f);
            int filterOuputOffset = borderOffset - 1;

            for (int x = 0; x < height - borderOffset; x++)
            {
                for (int y = 0; y < width - borderOffset; y++) // reversed sides
                {
                    int result = OperateColors(GetSourceMatrix(pixels, x, y), Matrix);
                    clonedPixels[x + filterOuputOffset, y + filterOuputOffset] = result;
                }
            }

            return clonedPixels;
        }

        private int[,] GetSourceMatrix(int[,] pixels, int x, int y)
        {
            int[,] result = new int[Matrix.Size, Matrix.Size];

            for (int i = 0; i < Matrix.Size; ++i)
            {
                for (int j = 0; j < Matrix.Size; ++j)
                {
                    result[i, j] = pixels[x + i, y + j];
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelColor"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        protected abstract int OperateColors(int[,] pixelColor, ConvolutionMatrix matrix);
    }
}
