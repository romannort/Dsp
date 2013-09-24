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
            Dft discreteTransform = new Dft();
            
            Func<Double, Double> f = x => Math.Sin(x) + Math.Cos(4 * x);
		    Int32 N = 16;
		
            discreteTransform.DoTransform(f, N);

            LineSeries ls = new LineSeries("sin(x)+sin(3x)/3+sin(5x)/5+...");

            IList<IDataPoint> points = discreteTransform.Magnitudes.Select((x, i) => ((IDataPoint)new DataPoint(i, x))).ToList();
            //ls.Points.Add(new DataPoint(x, y))
            ls.Points = points;
            
            model.Series.Add(ls);
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -4, 4));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            MyPlotModel.Model = model;         // this is raising the INotifyPropertyChanged event
        }

    }
}
