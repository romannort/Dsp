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

        public FilterBase(ConvolutionMatrix matrix)
        {
            Matrix = matrix;
        }

        public virtual int[,] Execute(int[,] pixels)
        {
            int[,] clonedPixels = (int[,])pixels.Clone();

            Color[,] pixelColor = new Color[Matrix.Size, Matrix.Size];
            int A, R, G, B;

            int width = pixels.GetLength(1);
            int height = pixels.GetLength(0);

            int borderOffset = (int)System.Math.Ceiling(Matrix.Size / 2f);
            int filterOuputOffset = borderOffset - 1;

            for (int x = 0; x < height - borderOffset; x++)
            {
                for (int y = 0; y < width - borderOffset; y++) // reversed sides
                {
                    FillPixelColorsMatrix(pixels, x, y, ref pixelColor);
            
                    A = pixelColor[1, 1].A;
                    R = OperateColor(pixelColor, Matrix, ColorCode.R);
                    G = OperateColor(pixelColor, Matrix, ColorCode.G);
                    B = OperateColor(pixelColor, Matrix, ColorCode.B);

                    clonedPixels[x + filterOuputOffset, y + filterOuputOffset] = 
                        Color.FromArgb(A, R, G, B).ToArgb();
                }
            }

            return clonedPixels;
        }


        private void FillPixelColorsMatrix(int[,] pixels, int x, int y, ref Color[,] pixelColors)
        {
            for (int i = 0; i < Matrix.Size; ++i)
            {
                for (int j = 0; j < Matrix.Size; ++j)
                {
                    pixelColors[i, j] = Color.FromArgb(pixels[x + i, y + j]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelColors"></param>
        /// <param name="matrix"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual int OperateColor(Color[,] pixelColors, ConvolutionMatrix matrix, ColorCode code);
    }
}
