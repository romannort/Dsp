using System;

namespace Dsp.ImageProcessing.Extensions
{
    public static class ArrayConverter
    {
        public static int[] ToLinear(int [,] array2D)
        {
            int[] result = new int[array2D.Length];

            int height = array2D.GetLength(0);
            int width = array2D.GetLength(1);
            Int64 index = 0;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    result[index++] = array2D[y, x];
                }
            }

            return result;
        }

        public static int[,] To2D(int[] array, int width, int height)
        {
            int[,] result = new int[height, width];

            Int64 index = 0;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    result[y, x] = array[index++];
                }
            }

            return result;
        }

    }
}
