using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Dsp.FastFourierTransform
{
    /// <summary> Makes Fourier transform using Radix-2 DIF method. </summary>
	public class Fft: FourierTransform
	{
		/// <summary> Does fast fouries transform. </summary>
		/// <param name="func"></param>
		/// <param name="n"></param>
		/// <returns></returns>
        public override ICollection<Complex> DoTransform(Func<Double, Double> func, Int32 n)
		{
            Discretizer discretizer = new Discretizer();
			IList<Double> discrete = (IList<Double>)discretizer.Discretize(func, n, 1.0);
            return DoTransform(discrete);
		}

        public override ICollection<Complex> DoTransform(ICollection<Double> data)
        {
            ICollection<Complex> indices = TransformInner(ToComplex(data));
            return indices;
        }

        public override ICollection<Double> DoTransformReverse(ICollection<Complex> data)
        {
            inverse = true;
            ICollection<Complex> indices = TransformInner(data);
            SetResultsInverse(indices);
            // Assume all numbers are real
            return Magnitudes;
        }

        private ICollection<Complex> TransformInner(ICollection<Complex> data)
        {
            IList<Complex> indices = FftDifRecursive((IList<Complex>)data, data.Count);
            SetResults(indices);
            if (!inverse)
            {
                ICollection<Complex> result = indices.Select(x => x / data.Count).ToList();
                SetResults(result);
                return result;
            }
            SetResults(indices);
            return indices;
        }

        /// <summary>Recursive FFT (decimation in frequency) </summary>
		/// <param name="vectorA"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private IList<Complex> FftDifRecursive(IList<Complex> vectorA, Int32 n)
		{
			if (vectorA.Count == 1) {
				return vectorA;
			}

            Int32 n2 = n / 2;
            IList<Complex> vectorOdd = new Complex[n2];
			IList<Complex> vectorEven = new Complex[n2];
            
            for (Int32 j = 0; j < n2; ++j) {
				vectorEven[j] = vectorA[j] + vectorA[j + n2];
                PerformanceStats.FftAdditions++;

				vectorOdd[j] = (vectorA[j] - vectorA[j + n2]) * Multiplier(j, n);
                PerformanceStats.FftAdditions++;
                PerformanceStats.FftMultiplications++;
            }

			IList<Complex> processedEven = FftDifRecursive(vectorEven, n2);
			IList<Complex> processedOdd = FftDifRecursive(vectorOdd, n2);

            IList<Complex> result = new Complex[n];
            for (Int32 i = 0; i < n2; ++i) {
                Int32 i2 = i * 2;
                result[i2] = processedEven[i];
                result[i2 + 1] = processedOdd[i];
            }
			return result;
		}

        private Complex Multiplier(Int32 m, Int32 n)
        {
            Int32 reverseCoeff = inverse ? 1 : -1;
            // PerformanceStats ?
            return Complex.Pow(Math.E, reverseCoeff * 2 * Math.PI * Complex.ImaginaryOne * m / n);
        }

        /// <summary>
        /// Swap data indices whenever index i has binary                                                        
        /// digits reversed from index j, where data is                                                          
        /// two doubles per index.                                                                               
        /// </summary>                                        
        /// <remarks> Useful for FFT DIT algorithm.</remarks>                                                   
        /// <param name="data"></param>                                                                          
        /// <param name="n"></param>                                                                             
        private static void Reverse(IList<double> data, int n)
        {
            // bit reverse the indices. This is exercise 5 in section                                            
            // 7.2.1.1 of Knuth's TAOCP the idea is a binary counter                                             
            // in k and one with bits reversed in j                                                              
            int j = 0, k = 0; // Knuth R1: initialize                                                            
            var top = n / 2;  // this is Knuth's 2^(n-1)                                                         
            while (true)
            {
                // Knuth R2: swap - swap j+1 and k+2^(n-1), 2 entries each                                       
                var t = data[j + 2];
                data[j + 2] = data[k + n];
                data[k + n] = t;
                t = data[j + 3];
                data[j + 3] = data[k + n + 1];
                data[k + n + 1] = t;
                if (j > k)
                { // swap two more                                                                               
                    // j and k                                                                                   
                    t = data[j];
                    data[j] = data[k];
                    data[k] = t;
                    t = data[j + 1];
                    data[j + 1] = data[k + 1];
                    data[k + 1] = t;
                    // j + top + 1 and k+top + 1                                                                 
                    t = data[j + n + 2];
                    data[j + n + 2] = data[k + n + 2];
                    data[k + n + 2] = t;
                    t = data[j + n + 3];
                    data[j + n + 3] = data[k + n + 3];
                    data[k + n + 3] = t;
                }
                // Knuth R3: advance k                                                                           
                k += 4;
                if (k >= n)
                    break;
                // Knuth R4: advance j                                                                           
                var h = top;
                while (j >= h)
                {
                    j -= h;
                    h /= 2;
                }
                j += h;
            } // bit reverse loop                                                                                
        }
	}
}
