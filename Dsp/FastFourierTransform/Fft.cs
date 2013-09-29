using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp.FastFourierTransform
{
    /// <summary>
    /// 
    /// </summary>
	public class Fft: FourierTransform
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		/// <param name="n"></param>
		/// <returns></returns>
        public override ICollection<Complex> DoTransform(Func<Double, Double> func, Int32 n)
		{
            Discretizer discretizer = new Discretizer();
			IList<Double> discrete = (IList<Double>)discretizer.Discretize(func, n, 1.0);
            return DoTransform(discrete);
		}

        public override ICollection<Complex> DoTransform(ICollection<Double> data)
        {
            ICollection<Complex> indices = TransformInner(ToComplex(data));
            return indices;
        }

        public override ICollection<Double> DoTransformReverse(ICollection<Complex> data)
        {
            inverse = true;
            ICollection<Complex> indices = TransformInner(data);
            // Assume all numbers are real
            return Magnitudes;
        }

        private ICollection<Complex> TransformInner(ICollection<Complex> data)
        {
            IList<Complex> indices = FftDifRecursive((IList<Complex>)data, data.Count);
            SetResults(indices);
            ICollection<Complex> result;
            if (inverse)
            {
                result = indices.Select(x => x / data.Count).ToList();
                SetResults(result);
                return result;
            }
            else
            {
                SetResults(indices);
                return indices;
            }
        }

        /// <summary>Recursive FFT (decimation in frequency) </summary>
		/// <param name="vectorA"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private IList<Complex> FftDifRecursive(IList<Complex> vectorA, Int32 n)
		{
			if (vectorA.Count == 1) {
				return vectorA;
			}

			Complex w = 1;
            Int32 n2 = n / 2;
            IList<Complex> vectorOdd = new Complex[n2];
			IList<Complex> vectorEven = new Complex[n2];
            
            for (Int32 j = 0; j < n2; ++j) {
				vectorEven[j] = vectorA[j] + vectorA[j + n2];
				vectorOdd[j] = (vectorA[j] - vectorA[j + n2]) * Multiplier(j, n);
			}

			IList<Complex> processedEven = FftDifRecursive(vectorEven, n2);
			IList<Complex> processedOdd = FftDifRecursive(vectorOdd, n2);

            IList<Complex> result = new Complex[n];
            for (Int32 i = 0; i < n2; ++i) {
                Int32 i2 = i * 2;
                result[i2] = processedEven[i];
                result[i2 + 1] = processedOdd[i];
            }
			return result;
		}

        private Complex Multiplier(Int32 m, Int32 n)
        {
            Int32 reverseCoeff = inverse ? 1 : -1;
            return Complex.Pow(Math.E, -1 * 2 * Math.PI * Complex.ImaginaryOne * m / n);
        }

		/// <summary>Discretizes function with into <paramref name="n"/> points.</summary>
		/// <param name="func"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private IList<Complex> Discretize(Func<Double, Double> func, Int32 n)
		{
			IList<Complex> result = Enumerable.Range(0, n).Select(x => new Complex(func(x), 0)).ToList();
			return result;
		} 
	}
}
