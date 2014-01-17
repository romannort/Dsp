using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsp.ImageProcessing.Filters.Math
{
    internal class MatrixMultiplication
    {

        public static double[,] NaiveMultiplication(double[,] m1, double[,] m2)
        {
            double[,] resultMatrix = new double[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    resultMatrix[i, j] = 0;
                    for (int k = 0; k < m1.GetLength(0); k++)
                    {
                        resultMatrix[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return resultMatrix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public unsafe static int[,] UnsafeMultiplication(int[,] m1, int[,] m2)
        {
            int height = m1.GetLength(0);
            int width = m2.GetLength(1);
            int length = m1.GetLength(1);
            int[,] resultMatrix = new int[height, width];
            
            unsafe
            {
                fixed (int* pm = resultMatrix, pm1 = m1, pm2 = m2)
                {
                    int i1, i2;
                    for (int i = 0; i < height; i++)
                    {
                        i1 = i * length;
                        for (int j = 0; j < width; j++)
                        {
                            i2 = j;
                            int result = 0;
                            for (int k = 0; k < length; k++, i2 += width)
                            {
                                result += pm1[i1 + k] * pm2[i2];
                            }
                            pm[i * width + j] = result;
                        }
                    }
                }
            }
            return resultMatrix;
        }
    }
}
