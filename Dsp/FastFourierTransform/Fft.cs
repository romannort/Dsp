using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp.FastFourierTransform
{
	public class Fft
	{
		private Complex Wn;


		public ICollection<Complex> DoTransform(Func<Double, Double> func, Int32 n )
		{
			Init(n);
			IList<Complex> discrete = Discretize(func, n);
			IList<Complex> indices = FftDif(discrete, n);
            return ReArrange(indices, n);
		}

        private ICollection<Complex> ReArrange(IList<Complex> indices, Int32 n)
        {
            Int32 n2 = n / 2;
            for (Int32 i = 1; i < n2; i += 2) {
                Complex tmp = indices[i];
                indices[i] = indices[i + n2 - 1];
                indices[i + n2 - 1] = tmp;
            }
            return indices;
        }

        private void Init(Int32 n)
		{
			Wn = Complex.Pow(Math.E, 2 * Math.PI * Complex.ImaginaryOne / n);
		}

		/// <summary>Recursive FFT with decimation in frequency. </summary>
		/// <param name="vectorA"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private IList<Complex> FftDif(IList<Complex> vectorA, Int32 n)
		{
			if (vectorA.Count == 1) {
				return vectorA;
			}

			Complex w = 1;
            Int32 n2 = n / 2;
            IList<Complex> vectorC = new Complex[n2];
			IList<Complex> vectorB = new Complex[n2];
            
            for (Int32 j = 0; j < n2; ++j) {
				vectorB[j] = vectorA[j] + vectorA[j + n2];
				vectorC[j] = (vectorA[j] - vectorA[j + n2]) * w;

				w *= Wn;
			}

			IList<Complex> processedB = FftDif(vectorB, n2);
			IList<Complex> processedC = FftDif(vectorC, n2);
			IList<Complex> result = processedB.Concat(processedC).ToList();
			return result;
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
