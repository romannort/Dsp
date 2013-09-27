using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dsp
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FourierTransform
    {
        /// <summary>
        /// Gets magnitudes collection
        /// </summary>
        public ICollection<Double> Magnitudes { get; protected set; }

        public ICollection<Double> Phases { get; protected set; }

        public abstract ICollection<Complex> DoTransform(Func<Double, Double> f, Int32 n);
    }
}
