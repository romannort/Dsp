using System.Collections.Generic;
using System.Linq;

namespace Dsp
{
    /// <summary>
    /// 
    /// </summary>
    public class FastWalshTransform
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double[] DoTransform(double[] data)
        {
            double[] result = new double[data.Length];
            data.CopyTo(result, 0);
            TransformInternal(ref result);
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double[] DoTransformInverse(double[] data)
        {
            double[] result = new double[data.Length];
            data.CopyTo(result, 0);
            TransformInternal(ref result);
            
            return result.Select(x => x / data.Length).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private static void TransformInternal(ref double[] data)
        {
            int N = data.Length;
            if (N == 1)
            {
                return;
            }

            double[] left = new double[N / 2];
            double[] right = new double[N / 2];

            for (int j = 0; j < N / 2; ++j)
            {
                left[j] = data[j] + data[j + N / 2];
                right[j] = data[j] - data[j + N / 2];
            }

            TransformInternal(ref left);
            TransformInternal(ref right);

            CopyToInput(ref data, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void CopyToInput(ref double[] data, ICollection<double> left, ICollection<double> right)
        {    
            left.CopyTo(data, 0);
            right.CopyTo(data, data.Length/2);
        }
    }
}