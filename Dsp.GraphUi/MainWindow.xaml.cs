using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dsp.GraphUi.Support;
using Dsp.Support;
using OxyPlot;
using Dsp.DiscreteFourierTransform;
using OxyPlot.Axes;
using Dsp.FastFourierTransform;

namespace Dsp.GraphUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly SeriesManager seriesManager = new SeriesManager();

        private readonly Func<Double, Double> f = x => Math.Sin(x) + Math.Cos(4 * x);
        
        private const Int32 N = 16;

        private const double Period = 6.28;


        public MainWindow()
        {
            InitializeComponent();
            SetUpPlot();
        }

        private void SetUpPlot()
        {
            PopulateSeriesData();

            UpdatePlot();
        }

        private void PopulateSeriesData()
        {
            FourierTransform discreteTransform = new Dft();
            FourierTransform fastTransform = new Fft();

            Discretizer discretizer = new Discretizer();
            LineSeriesBuilder seriesBuilder = new LineSeriesBuilder();


            IDictionary<double, double> discreteData = discretizer.Discretize(f, 0, Period, Period/N)
                .Take(N) // Take only the given number of elements. Otherwise problem appears in FFT.
                .ToDictionary(x => x.Key, x => x.Value);


            ICollection<Complex> discreteTransformData = discreteTransform.DoTransform(discreteData.Values);
            ICollection<Complex> fastTransformData = fastTransform.DoTransform(discreteData.Values);

            seriesManager.Add(SeriesNames.DftMagnitudes, seriesBuilder.CreateSeries("DFT Magnitude", discreteData.Keys, discreteTransform.Magnitudes));
            seriesManager.Add(SeriesNames.FftMagnitudes, seriesBuilder.CreateSeries("FFT Magnitude", discreteData.Keys, fastTransform.Magnitudes));
            seriesManager.Add(SeriesNames.DftPhases, seriesBuilder.CreateSeries("DFT Phase", discreteData.Keys, discreteTransform.Phases));
            seriesManager.Add(SeriesNames.FftPhases, seriesBuilder.CreateSeries("FFT Phase", discreteData.Keys, fastTransform.Phases));

            PopulateStats();
            discreteTransform.DoTransformInverse(discreteTransformData);
            fastTransform.DoTransformInverse(fastTransformData);
            
            seriesManager.Add(SeriesNames.OriginalF, seriesBuilder.CreateSeries("Original F(x)", discretizer.Discretize(f, 0, Period, 0.01)));
            seriesManager.Add(SeriesNames.InverseFft, seriesBuilder.CreateSeries("Inverse FFT", discreteData.Keys, fastTransform.Magnitudes));
            seriesManager.Add(SeriesNames.InverseDft, seriesBuilder.CreateSeries("Inverse DFT", discreteData.Keys, discreteTransform.Magnitudes));
        }

        private void Btn_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                String seriesName = element.Tag as String;
                Color newColor = new Color {A = 0xff, R = 0xdd, G = 0xdd, B = 0xdd};
                if (seriesManager.ActiveSeries.Contains(seriesName))
                {
                    seriesManager.ActiveSeries.Remove(seriesName);
                }
                else
                {
                    seriesManager.ActiveSeries.Add(seriesName);
                    newColor = Colors.Chartreuse;
                }
                ((Button)element).Background = new SolidColorBrush(newColor);
                UpdatePlot();    
            }
        }

        private void UpdatePlot()
        {
            PlotModel model = new PlotModel("Fourier Transform");

            AddActiveSeries(model);
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -3));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, Period));

            MyPlotModel.Model = model;
        }

        private void AddActiveSeries(PlotModel model)
        {
            foreach (var series in seriesManager.GetActive())
            {
                model.Series.Add(series);
            }
        }

        private void PopulateStats()
        {
            
            string text = lblDftAdds.Content as String;
            lblDftAdds.Content = text + PerformanceStats.DftAdditions;

            text = lblFftAdds.Content as String;
            lblFftAdds.Content = text + PerformanceStats.FftAdditions;

            text = lblDftMuls.Content as String;
            lblDftMuls.Content = text + PerformanceStats.DftMultiplications;

            text = lblFftMuls.Content as String;
            lblFftMuls.Content = text + PerformanceStats.FftMultiplications;

            text = lblNumber.Content as String;
            lblNumber.Content = text + N;
        }
    }
}
