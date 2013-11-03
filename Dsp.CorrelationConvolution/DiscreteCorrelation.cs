using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp.CorrelationConvolution
{
    public class DiscreteCorrelation
    {

        public ICollection<Complex> Do(ICollection<double> x, ICollection<double> y, Int32 n)
        {
            ICollection<Complex> result = new List<Complex>(n);

            for (int m = 0; m < n; ++m)
            {
                double value = .0;
                for (int h = 0; h < n; ++h)
                {
                    value = x.ElementAt(h) * y.ElementAt(m + h);
                }
                value /= n;
                result.Add(value);
            }
            return result;
        }
    }
}
