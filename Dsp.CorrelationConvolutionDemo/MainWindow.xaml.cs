using Dsp.Support;
using OxyPlot;
using OxyPlot.Axes;

namespace Dsp.CorrelationConvolutionDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        readonly SeriesManager seriesManager = new SeriesManager();

        public MainWindow()
        {
            InitializeComponent();
            Populate();
        }


        private void Populate()
        {
            CorrelationConvolution.CorrelationConvolutionDemo demo
                = new CorrelationConvolution.CorrelationConvolutionDemo();

            demo.Start();
            PopulateSeries(demo);
            UpdatePlot();
            
        }

        private void UpdatePlot()
        {
            PlotModel model = new PlotModel("Correlation And Convolution");

            AddActiveSeries(model);
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));

            MyPlotModel.Model = model;
        }

        private void AddActiveSeries(PlotModel model)
        {
            foreach (var series in seriesManager.GetActive())
            {
                model.Series.Add(series);
            }
        }

        private void PopulateSeries(CorrelationConvolution.CorrelationConvolutionDemo demo)
        {

            LineSeriesBuilder seriesBuilder = new LineSeriesBuilder();

            seriesManager.Add("D Convln", seriesBuilder.CreateSeries("D Convln", 
                demo.DiscreteConvolution.Keys, demo.DiscreteConvolution.Values));

            seriesManager.Add("Fast Convln", seriesBuilder.CreateSeries("Fast Convln",
                demo.FastConvolution.Keys, demo.FastConvolution.Values));

            seriesManager.Add("D Corr", seriesBuilder.CreateSeries("D Corr",
                demo.DiscreteCorrelation.Keys, demo.DiscreteCorrelation.Values));

            seriesManager.Add("Fast Corr", seriesBuilder.CreateSeries("Fast Corr",
                demo.FastCorrelation.Keys, demo.FastCorrelation.Values));

            
            //Manually activate these series
            
            seriesManager.ActiveSeries.Add("D Convln");
            seriesManager.ActiveSeries.Add("Fast Convln");

            seriesManager.ActiveSeries.Add("D Corr");
            seriesManager.ActiveSeries.Add("Fast Corr");
        }
    }
}
