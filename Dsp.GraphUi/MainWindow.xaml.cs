using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Numerics;
using OxyPlot;
using Dsp.DiscreteFourierTransform;
using OxyPlot.Axes;
using Dsp.FastFourierTransform;

namespace Dsp.GraphUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            SetUpPlot();
        }

        private void SetUpPlot()
        {
            PlotModel model = new PlotModel("Fourier Transform");
            FourierTransform discreteTransform = new Dft();
            FourierTransform fastTransform = new Fft();
            
            Func<Double, Double> f = x => Math.Sin(x) + Math.Cos(4 * x);
            Int32 N = 128;
            Discretizer discretizer = new Discretizer();
            LineSeriesBuilder seriesBuilder = new LineSeriesBuilder();
            
            ICollection<Double> keys = Enumerable.Range(0, N).Select( i => i * 0.1).ToList();
            ICollection<Double> values = discretizer.Discretize(f, keys);

            ICollection<Complex> discreteData = discreteTransform.DoTransform(values); //DoTransform(f, N);
            ICollection<Complex> fastData = fastTransform.DoTransform(values); //DoTransform(f, N);

            //model.Series.Add(seriesBuilder.CreateSeries("DFT Magnitude", discreteTransform.Magnitudes));
            //model.Series.Add(seriesBuilder.CreateSeries("FFT Magnitude", fastTransform.Magnitudes));
            //model.Series.Add(seriesBuilder.CreateSeries("DFT Phase", discreteTransform.Phases));
            //model.Series.Add(seriesBuilder.CreateSeries("FFT Phase", fastTransform.Phases));

            discreteTransform.DoTransformReverse(discreteData);
            fastTransform.DoTransformReverse(fastData);

            model.Series.Add(seriesBuilder.CreateSeries("Original F(x)", discretizer.Discretize(f, 0, N, 0.01)));
            model.Series.Add(seriesBuilder.CreateSeries("Inverse FFT", keys, fastTransform.Magnitudes));
            model.Series.Add(seriesBuilder.CreateSeries("Inverse DFT", keys, discreteTransform.Magnitudes));

            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, N));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            
            MyPlotModel.Model = model;
        }
    }
}
