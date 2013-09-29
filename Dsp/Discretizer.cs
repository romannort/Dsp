using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return result;
        }
    }
}
