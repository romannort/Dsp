using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FourierTransform
    {
        /// <summary>
        /// 
        /// </summary>
        protected Boolean inverse = false;

        /// <summary>
        /// Gets magnitudes collection
        /// </summary>
        public ICollection<Double> Magnitudes { get; protected set; }

        public ICollection<Double> Phases { get; protected set; }

        public abstract ICollection<Complex> DoTransform(Func<Double, Double> f, Int32 n);

        public abstract ICollection<Complex> DoTransform(ICollection<Double> data);

        public abstract ICollection<Double> DoTransformReverse(ICollection<Complex> data);

        protected void SetResults(ICollection<Complex> result)
        {
            Magnitudes = result.Select(x => x.Magnitude).ToList();
            Phases = result.Select(x => x.Phase).ToList();
        }

        protected ICollection<Complex> ToComplex(ICollection<Double> data)
        {
            return data.Select(x => new Complex(x, 0)).ToList();
        }

        protected ICollection<Double> ToDouble(ICollection<Complex> data)
        {
            // Assume the data has only Real part
            return data.Select(x => x.Magnitude).ToList();
        }

        protected void SetResultsInverse(ICollection<Complex> data)
        {
            Magnitudes.Clear();
            Phases.Clear();
            foreach (var complex in data)
            {
                Int32 inverseCoeff = 1;
                if (Math.Abs(complex.Phase - 0) > 0.01) {
                    inverseCoeff *= -1;
                }
                Magnitudes.Add(complex.Magnitude * inverseCoeff);
                Phases.Add(0);
            }
        }
    }
}
