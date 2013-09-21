using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp.FastFourierTransform
{
	public class Fft
	{
		private Complex Wn;

		private Int32 n2;

		public ICollection<Complex> DoTransform(Func<Double, Double> func, Int32 n )
		{
			Init(n);
			IList<Complex> discrete = Discretize(func, n);
			return FftDif(discrete, n);
		}

		private void Init(Int32 n)
		{
			Wn = Complex.Pow(Math.E, 2 * Math.PI * Complex.ImaginaryOne / n);
			n2 = n / 2;
		}

		/// <summary>Recursive FFt with decimation in frequency. </summary>
		/// <param name="vectorA"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private IList<Complex> FftDif(IList<Complex> vectorA, Int32 n)
		{
			if (vectorA.Count == 1) {
				return vectorA;
			}

			Complex w = 1;
			IList<Complex> vectorC = new Complex[n];
			IList<Complex> vectorB = new Complex[n];
			
			for (Int32 j = 0; j < n2; ++j) {
				vectorB[j] = vectorA[j] + vectorA[j + n2];
				vectorC[j + n2] = (vectorA[j] - vectorA[j + n2]) * w;

				w *= Wn;
			}

			IList<Complex> processedB = FftDif(vectorB, n);
			IList<Complex> processedC = FftDif(vectorC, n);
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
