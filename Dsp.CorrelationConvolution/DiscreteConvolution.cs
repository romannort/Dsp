﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsp.CorrelationConvolution
{
    public class DiscreteConvolution
    {
        public ICollection<Double> Do(ICollection<double> x, ICollection<double> y, Int32 n)
        {
            ICollection<Double> result = new List<Double>(n);

            for (int m = 0; m < n; ++m)
            {
                double value = .0;
                for (int h = 0; h < n; ++h)
                {
                    if (m - h > 0 && m - h < n)
                    {
                        value += x.ElementAt(h) * y.ElementAt(m - h);   
                    }
                }
                value /= n;
                result.Add(value);
            }
            return result;
        }
    }
}
