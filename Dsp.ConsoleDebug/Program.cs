using System;
using System.Collections.Generic;
using System.Numerics;
using Dsp.DiscreteFourierTransform;
using Dsp.FastFourierTransform;

namespace Dsp.ConsoleDebug
{
	class Program
	{
		static Func<Double, Double> f = x => Math.Sin(x) + Math.Cos(4 * x);
		static Int32 N = 16;
			

		static void Main(string[] args)
		{
			TestDft();
			TestFft();
		}

		private static void TestDft()
		{
			Dft transformer = new Dft();
			ICollection<Complex> transformed = transformer.DoTransform(f, N);

			Console.WriteLine("Discrete Fourier Transform\n");
			Print(transformed);
		}


		private static void TestFft()
		{
			Fft transformer = new Fft();
			ICollection<Complex> transformed = transformer.DoTransform(f, N);
			Console.WriteLine("Discrete Fourier Transform\n");

			Print(transformed);
		}

		private static void Print(ICollection<Complex> transformed)
		{
			foreach (var complex in transformed) {
				Console.WriteLine("Magnitude: {0} | Phase {1}", complex.Magnitude, complex.Phase);
			}
		}
		
	}
}
