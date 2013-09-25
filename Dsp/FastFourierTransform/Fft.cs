using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp.FastFourierTransform
{
	public class Fft: IFourierTransform
	{
		private Complex Wn;

        public ICollection<Double> Magnitudes { get; set; }

        public ICollection<Double> Phases { get; set; }

        public ICollection<Complex> DoTransform(Func<Double, Double> func, Int32 n )
		{
			Init(n);
			IList<Complex> discrete = Discretize(func, n);
			IList<Complex> indices = FftDif(discrete, n);
            SetResults(indices);
            return indices;
		}

        private void SetResults(ICollection<Complex> result)
        {
            Magnitudes = result.Select(x => x.Magnitude).ToList();
            Phases = result.Select(x => x.Phase).ToList();
        }

        
        private void Init(Int32 n)
		{
			Wn = Complex.Pow(Math.E, 2 * Math.PI * Complex.ImaginaryOne / n);
		}

        private void ReArrange(IList<Complex> indices)
        {
            int[] codes = new[] {0,8,4,12,2,10,6,14,1,9,5,13,3,11,7,15};
            //Enumerable.Range(0, n)
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
            IList<Complex> vectorOdd = new Complex[n2];
			IList<Complex> vectorEven = new Complex[n2];
            
            for (Int32 j = 0; j < n2; ++j) {
				vectorEven[j] = vectorA[j] + vectorA[j + n2];
				vectorOdd[j] = (vectorA[j] - vectorA[j + n2]) * Multiplier(j, n);
			}

			IList<Complex> processedEven = FftDif(vectorEven, n2);
			IList<Complex> processedOdd = FftDif(vectorOdd, n2);

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
