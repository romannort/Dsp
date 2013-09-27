using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Dsp.DiscreteFourierTransform
{
	public class Dft: FourierTransform
	{
        public override ICollection<Complex> DoTransform( Func<Double, Double> func, Int32 n)
		{
			ICollection<Complex> result = new List<Complex>();
			for (Int32 k = 0; k < n; ++k) {
				Complex point = CalculatePoint(func, k, n);
				result.Add(point);
			}
			SetResults(result);
            return result;
		}

        private void SetResults(ICollection<Complex> result)
        {
            Magnitudes = result.Select(x => x.Magnitude).ToList();
            Phases = result.Select(x => x.Phase).ToList();
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
			Int32 inverse = isInverse ? 1 : -1;
			Complex result = Complex.Pow(Math.E, inverse * Complex.ImaginaryOne * 2 * Math.PI * m * k / n);
			return result;
		}

	}
}
