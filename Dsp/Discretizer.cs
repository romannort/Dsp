using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsp
{
    public class Discretizer
    {
        public ICollection<Double> Discretize(Func<Double, Double> f, Int32 dotsNumber, Double step)
        {
            ICollection<Double> result = new List<Double>();

            for (Int32 dot = 0; dot < dotsNumber; ++dot)
            {
                Double value = f(dot * step);
                result.Add(value);
            }
            return result.Take(dotsNumber).ToList();
        }

        public IDictionary<Double, Double> Discretize(Func<Double, Double> f, Double startValue, Double endValue, Double step)
        {
            IDictionary<Double, Double> result = new Dictionary<Double, Double>();
            Double currentPoint = startValue;
            while(currentPoint < endValue)
            {
                Double value = f(currentPoint);
                result.Add(currentPoint, value);
                currentPoint += step;
            }
            return result;
        }

        public ICollection<Double> Discretize(Func<Double,Double> f, ICollection<Double> keys)
        {
            ICollection<Double> result = keys.Select(f).ToList();
            return result;
        }
    }
}
