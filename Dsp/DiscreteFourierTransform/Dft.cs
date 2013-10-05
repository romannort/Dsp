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
            ICollection<Complex> result = TransfromInner(ToComplex(data));
            SetResults(result);
            return result;
        }

        public override ICollection<double> DoTransformReverse(ICollection<Complex> data)
        {
            inverse = true;
            ICollection<Complex> result = TransfromInner(data);
            SetResultsInverse(result);
            // Assume all results are Real so Magnitue equal to real value and Phase == 0
            return Magnitudes;
        }

        private ICollection<Complex> TransfromInner(ICollection<Complex> data)
        {
            ICollection<Complex> result = new List<Complex>();
            Int32 n = data.Count;
            for (Int32 k = 0; k < n; ++k)
            {
                Complex point = CalculatePoint(data, k, n);
                result.Add(point);
            }
            return result;
        }

		private Complex CalculatePoint(ICollection<Complex> sourceData, Int32 k, Int32 n)
		{
			Complex result = new Complex();
			for (int m = 0; m < n; ++m) {
				Complex subSum = sourceData.ElementAt(m) * Multiplier(m, k, n);
			    PerformanceStats.DftMultiplicaiton++;

                result += subSum;
			    PerformanceStats.DftAddition++;
			}
            Double inverseCoef = n;
            return inverse ? result : result / inverseCoef; 
		}

		private Complex Multiplier(Int32 m, Int32 k, Int32 n)
		{
            Int32 reverseCoeff = inverse ? 1 : -1;
			Complex result = Complex.Pow(Math.E, reverseCoeff * Complex.ImaginaryOne * 2 * Math.PI * m * k / n);
            //PerformanceStats.DftMultiplicaiton++ ?
			return result;
		}

	}
}
