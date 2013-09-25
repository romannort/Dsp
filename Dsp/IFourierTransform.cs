using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dsp
{
    public interface IFourierTransform
    {
        /// <summary>
        /// Gets magnitudes collection
        /// </summary>
        ICollection<Double> Magnitudes { get; set; }

        ICollection<Double> Phases { get; set; }

        ICollection<Complex> DoTransform(Func<Double, Double> f, Int32 n);
    }
}
