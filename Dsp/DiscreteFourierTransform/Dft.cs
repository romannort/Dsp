using System;
using System.Collections.Generic;
using System.Numerics;

namespace Dsp.DiscreteFourierTransform
{
	public class Dft
	{
		
		public ICollection<Complex> DoTransform( Func<Double, Double> func, Int32 n)
		{
			ICollection<Complex> result = new List<Complex>();
			for (Int32 k = 0; k < n; ++k) {
				Complex point = CalculatePoint(func, k, n);
				result.Add(point);
			}

			return result;
		}


		private Complex CalculatePoint(Func<Double, Double> func, Int32 k, Int32 n)
		{
			Complex result = new Complex();
			for (int m = 0; m < n; ++m) {
				Complex subSum = func(m) * Multiplier(m, k, n);
				result += subSum;
			}
			return result;
		}

		private Complex Multiplier(Int32 m, Int32 k, Int32 n, Boolean isInverse = false)
		{
			Int32 inverse = isInverse ? -1 : 1;
			Complex result = Complex.Pow(Math.E, inverse * Complex.ImaginaryOne * 2 * Math.PI * m * k / n);
			return result;
		}

	}
}
