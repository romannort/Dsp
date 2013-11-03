using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dsp.FastFourierTransform;

namespace Dsp.CorrelationConvolution
{
    public class FastConvolution
    {
        private readonly Fft fftTransformer = new Fft();

        public ICollection<Double> Do(ICollection<Double> x, ICollection<Double> y, int n )
        {
            ICollection<Complex> yTransformed = fftTransformer.DoTransform(y);
            
            ICollection<Complex> xTransformed = fftTransformer.DoTransform(x);
            
            // For correlation
            //IEnumerable<Complex> xTransformedConjugated = fftTransformer.DoTransform(x).Select(Complex.Conjugate);

            ICollection<Complex> resultInverse = new List<Complex>(n);
            for (int i = 0; i < n; ++i)
            {
                resultInverse.Add(Complex.Multiply(xTransformed.ElementAt(i),
                                                   yTransformed.ElementAt(i)));
            }

            ICollection<Double> result  = fftTransformer.DoTransformInverse(resultInverse);

            return result;
        }
    } 
}
