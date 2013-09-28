using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Dsp.DiscreteFourierTransform
{
	public class Dft: FourierTransform
	{
        public override ICollection<Complex> DoTransform(Func<Double, Double> func, Int32 n)
		{
            Discretizer discretizer = new Discretizer();
            ICollection<Double> data = discretizer.Discretize(func, n, 1.0);
            ICollection<Complex> result = DoTransform(data);
            return result;
		}

        public override ICollection<Complex> DoTransform(ICollection<double> data)
        {
            ICollection<Complex> result = TransfromInner((ICollection<Complex>)data);
            return result;
        }

        public override ICollection<double> DoTransformReverse(ICollection<Complex> data)
        {
            reverse = true;
            ICollection<Complex> result = TransfromInner(data);
            // Assume all results are Real so Magnitue equal to real value and Phase == 0
            return Magnitudes;
        }

        private ICollection<Complex> TransfromInner(ICollection<Complex> data)
        {
            ICollection<Complex> result = new List<Complex>();
            Int32 n = data.Count;
            for (Int32 k = 0; k < n; ++k)
            {
                Complex point = CalculatePoint(data.ElementAt(k), k, n);
                result.Add(point);
            }
            SetResults(result);
            return result;
        }

		private Complex CalculatePoint(Complex fValue, Int32 k, Int32 n)
		{
			Complex result = new Complex();
			for (int m = 0; m < n; ++m) {
				Complex subSum = fValue * Multiplier(m, k, n);
				result += subSum;
			}
			return result;
		}

		private Complex Multiplier(Int32 m, Int32 k, Int32 n)
		{
            Int32 reverseCoeff = reverse ? 1 : -1;
			Complex result = Complex.Pow(Math.E, reverseCoeff * Complex.ImaginaryOne * 2 * Math.PI * m * k / n);
			return result;
		}

	}
}
