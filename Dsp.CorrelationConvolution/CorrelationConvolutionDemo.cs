﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsp.CorrelationConvolution
{
    public class CorrelationConvolutionDemo
    {

        public IDictionary<Double, Double> DiscreteConvolution { get; private set;}

        public IDictionary<Double, Double> FastConvolution { get; private set; }

        public IDictionary<Double, Double> DiscreteCorrelation { get; private set; }

        public IDictionary<Double, Double> FastCorrelation { get; private set; }


        public void Start( /*Func<double, double> x, Func<double, double> y*/)
        {
            const double start = 0;
            const double end = 4*Math.PI;
            //const double step = 1.0; // 0.250;
            const int dotsNumber = 128;

            Func<double, double> x = a => Math.Sin(a) + Math.Cos(4*a);
            Func<double, double> y = b => Math.Cos(4*b);


            Discretizer discretizer = new Discretizer();
            //IDictionary<Double, Double> xData = discretizer.Discretize(x, start, end, step);
            //IDictionary<Double, Double> yData = discretizer.Discretize(y, start, end, step);

            IDictionary<Double, Double> xData = discretizer.Discretize(x, start, end, dotsNumber);
            IDictionary<Double, Double> yData = discretizer.Discretize(y, start, end, dotsNumber);

            DiscreteConvolution dConvolution = new DiscreteConvolution();
            ICollection<Double> discreteConvolutionResult = dConvolution.Do(xData.Values, yData.Values, xData.Count);

            FastConvolution fConvolution = new FastConvolution();
            ICollection<Double> fastConvolutionResult = fConvolution.Do(xData.Values, yData.Values, xData.Count);


            DiscreteConvolution = discreteConvolutionResult
                .Select((v, i) => new KeyValuePair<Double, Double>(xData.Keys.ElementAt(i), v))
                .ToDictionary(kp => kp.Key, kp => kp.Value);

            FastConvolution = fastConvolutionResult
                .Select((v, i) => new KeyValuePair<Double, Double>(xData.Keys.ElementAt(i), v))
                .ToDictionary(kp => kp.Key, kp => kp.Value);
        }

    }
}
