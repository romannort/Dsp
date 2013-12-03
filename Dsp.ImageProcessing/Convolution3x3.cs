using System.Drawing;
using System.Linq;
using Dsp.ImageProcessing.Extensions;

namespace Dsp.ImageProcessing
{
    public static class Convolution3x3
    {
        public static int[,] Convolution(int[,] pixels, ConvolutionMatrix m)
        {
            int[,] clonedPixels = (int[,])pixels.Clone();

            Color[,] pixelColor = new Color[3, 3];
            int A, R, G, B;

            int width = pixels.GetLength(1);
            int height = pixels.GetLength(0);

            for (int x = 0; x < height - 2; x++)
            {
                for (int y = 0; y < width - 2; y++) // reversed sides
                {
                    pixelColor[0, 0] = Color.FromArgb(pixels[x, y]);
                    pixelColor[0, 1] = Color.FromArgb(pixels[x, y + 1]);
                    pixelColor[0, 2] = Color.FromArgb(pixels[x, y + 2]);
                    pixelColor[1, 0] = Color.FromArgb(pixels[x + 1, y]);
                    pixelColor[1, 1] = Color.FromArgb(pixels[x + 1, y + 1]);
                    pixelColor[1, 2] = Color.FromArgb(pixels[x + 1, y + 2]);
                    pixelColor[2, 0] = Color.FromArgb(pixels[x + 2, y]);
                    pixelColor[2, 1] = Color.FromArgb(pixels[x + 2, y + 1]);
                    pixelColor[2, 2] = Color.FromArgb(pixels[x + 2, y + 2]);

                    A = pixelColor[1, 1].A;

                    R = OperateColor(pixelColor, m, "R");

                    G = OperateColor(pixelColor, m, "G");

                    B = OperateColor(pixelColor, m, "B");

                    
                    clonedPixels[x + 1, y + 1] = Color.FromArgb(A, R, G, B).ToArgb();
                }
            }
            return clonedPixels;
        }


        public static int[,] MinMaxFilter(int[,] pixels, string minmax)
        {
            int[,] clonedPixels = (int[,])pixels.Clone();

            Color[,] pixelColor = new Color[3, 3];

            int width = pixels.GetLength(1);
            int height = pixels.GetLength(0);

            for (int x = 0; x < height - 2; x++)
            {
                for (int y = 0; y < width - 2; y++) // reversed sides
                {
                    pixelColor[0, 0] = Color.FromArgb(pixels[x, y]);
                    pixelColor[0, 1] = Color.FromArgb(pixels[x, y + 1]);
                    pixelColor[0, 2] = Color.FromArgb(pixels[x, y + 2]);
                    pixelColor[1, 0] = Color.FromArgb(pixels[x + 1, y]);
                    pixelColor[1, 1] = Color.FromArgb(pixels[x + 1, y + 1]);
                    pixelColor[1, 2] = Color.FromArgb(pixels[x + 1, y + 2]);
                    pixelColor[2, 0] = Color.FromArgb(pixels[x + 2, y]);
                    pixelColor[2, 1] = Color.FromArgb(pixels[x + 2, y + 1]);
                    pixelColor[2, 2] = Color.FromArgb(pixels[x + 2, y + 2]);

                    int A = pixelColor[1, 1].A;

                    int R = MinMaxColor(pixelColor, "R", minmax);

                    int G = MinMaxColor(pixelColor, "G", minmax);

                    int B = MinMaxColor(pixelColor, "B", minmax);

                    clonedPixels[x + 1, y + 1] = Color.FromArgb(A, R, G, B).ToArgb();
                }
            }
            return clonedPixels;
        }

        private static int MinMaxColor(Color[,] pixelColor, string colorName, string minmax)
        {
            int[] linearArray = new int[pixelColor.GetLength(0) * pixelColor.GetLength(1)];

            int index = 0;
            for (int i = 0; i < pixelColor.GetLength(0); ++i)
            {
                for (int j = 0; j < pixelColor.GetLength(1); ++j)
                {
                    linearArray[index++] = pixelColor[i, j].ColorByName(colorName);
                }
            }
            
            if (minmax == "MIN")
            {
                return linearArray.Min();
            }
            if (minmax == "MAX")
            {
                return linearArray.Max();
            }
            return 0;
        }

        private static int OperateColor(Color[,] pixelColor, ConvolutionMatrix m, string colorName)
        {
             var result = (int)((((pixelColor[0, 0].ColorByName(colorName) * m.Matrix[0, 0]) +
                                 (pixelColor[1, 0].ColorByName(colorName) * m.Matrix[1, 0]) +
                                 (pixelColor[2, 0].ColorByName(colorName) * m.Matrix[2, 0]) +
                                 (pixelColor[0, 1].ColorByName(colorName) * m.Matrix[0, 1]) +
                                 (pixelColor[1, 1].ColorByName(colorName) * m.Matrix[1, 1]) +
                                 (pixelColor[2, 1].ColorByName(colorName) * m.Matrix[2, 1]) +
                                 (pixelColor[0, 2].ColorByName(colorName) * m.Matrix[0, 2]) +
                                 (pixelColor[1, 2].ColorByName(colorName) * m.Matrix[1, 2]) +
                                 (pixelColor[2, 2].ColorByName(colorName) * m.Matrix[2, 2]))
                                        / m.Factor) + m.Offset);

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
