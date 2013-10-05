using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dsp.GraphUi.Support;
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

        private const Double Step = 0.5;

        private const Double StepOrigin = 0.01;


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


            // Step < 1 
            ICollection<Double> keys = Enumerable.Range(0, N).Select(i => i * Step).ToList();
            ICollection<Double> originalValues = discretizer.Discretize(f, keys);

            ICollection<Complex> discreteTransformData = discreteTransform.DoTransform(originalValues);
            ICollection<Complex> fastTransformData = fastTransform.DoTransform(originalValues);

            seriesManager.Add(SeriesNames.DftMagnitudes, seriesBuilder.CreateSeries("DFT Magnitude", discreteTransform.Magnitudes));
            seriesManager.Add(SeriesNames.FftMagnitudes, seriesBuilder.CreateSeries("FFT Magnitude", fastTransform.Magnitudes));
            seriesManager.Add(SeriesNames.DftPhases, seriesBuilder.CreateSeries("DFT Phase", discreteTransform.Phases));
            seriesManager.Add(SeriesNames.FftPhases, seriesBuilder.CreateSeries("FFT Phase", fastTransform.Phases));

            PopulateStats();
            discreteTransform.DoTransformReverse(discreteTransformData);
            fastTransform.DoTransformReverse(fastTransformData);

            seriesManager.Add(SeriesNames.OriginalF, seriesBuilder.CreateSeries("Original F(x)", discretizer.Discretize(f, 0, N * Step, StepOrigin)));
            seriesManager.Add(SeriesNames.InverseFft, seriesBuilder.CreateSeries("Inverse FFT", keys, fastTransform.Magnitudes));
            seriesManager.Add(SeriesNames.InverseDft, seriesBuilder.CreateSeries("Inverse DFT", keys, discreteTransform.Magnitudes));
        }

        private void Btn_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                String seriesName = element.Tag as String;
                Color newColor = new Color() {A = 0xff, R = 0xdd, G = 0xdd, B = 0xdd};
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
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, N * Step));

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
