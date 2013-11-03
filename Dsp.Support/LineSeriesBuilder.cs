using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;

namespace Dsp.Support
{
    public class LineSeriesBuilder
    {
        /// <summary>Bad workaround for fixed colors... </summary>
        private readonly ICollection<OxyColor> colors = new List<OxyColor>()
            {
                OxyColors.Red,
                OxyColors.Blue,
                OxyColors.Green,
                OxyColors.Yellow,
                OxyColors.Purple,
                OxyColors.Black,
                OxyColors.Orange
            };

        public LineSeries CreateSeries(String name, ICollection<Double> data)
        {
            LineSeries ls = new LineSeries(name);
            IList<IDataPoint> points = data.Select((x, i) => ((IDataPoint)new DataPoint(i, x))).ToList();
            ls.Points = points;
            ls.Color = colors.First();
            colors.Remove(ls.Color);
            return ls;
        }

        public LineSeries CreateSeries(String name, IDictionary<Double, Double> data)
        {
            LineSeries ls = new LineSeries(name);
            IList<IDataPoint> points = data.Select(x => ((IDataPoint)new DataPoint(x.Key, x.Value))).ToList();
            ls.Points = points;
            ls.Color = colors.First();
            colors.Remove(ls.Color);
            return ls;
        }

        public LineSeries CreateSeries(String name, ICollection<Double> xParts, ICollection<Double> yParts)
        {
            if (xParts.Count != yParts.Count) {
                throw new ArgumentException("Number of elements in the parts doesn't match.");
            }
            LineSeries ls = new LineSeries(name);
            ls.Points = xParts.Select((x, i) => ((IDataPoint)new DataPoint(x, yParts.ElementAt(i)))).ToList();
            ls.Color = colors.First();
            colors.Remove(ls.Color);
            return ls;
        }
    }
}
