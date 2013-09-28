using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using Dsp;
using Dsp.DiscreteFourierTransform;
using OxyPlot.Series;
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
            Int32 N = 16;
            
            discreteTransform.DoTransform(f, N);
            fastTransform.DoTransform(f, N);

            model.Series.Add(CreateSeries("DFT Magnitude", discreteTransform.Magnitudes));
            model.Series.Add(CreateSeries("FFT Magnitude", fastTransform.Magnitudes));
            model.Series.Add(CreateSeries("DFT Phase", discreteTransform.Phases));
            model.Series.Add(CreateSeries("FFT Phase", fastTransform.Phases));

            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, N));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            MyPlotModel.Model = model;
        }

        private LineSeries CreateSeries(String name, ICollection<Double> data)
        {
            LineSeries ls = new LineSeries(name);
            IList<IDataPoint> points = data.Select((x, i) => ((IDataPoint)new DataPoint(i, x))).ToList();
            ls.Points = points;
            return ls;
        }

    }
}
