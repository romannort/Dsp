using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using Dsp;
using OxyPlot.Series;

namespace Dsp.GraphUi
{
    internal class LineSeriesBuilder
    {

        internal LineSeries CreateSeries(String name, ICollection<Double> data)
        {
            LineSeries ls = new LineSeries(name);
            IList<IDataPoint> points = data.Select((x, i) => ((IDataPoint)new DataPoint(i, x))).ToList();
            ls.Points = points;
            return ls;
        }

        internal LineSeries CreateSeries(String name, IDictionary<Double, Double> data)
        {
            LineSeries ls = new LineSeries(name);
            IList<IDataPoint> points = data.Select(x => ((IDataPoint)new DataPoint(x.Key, x.Value))).ToList();
            ls.Points = points;
            return ls;
        }

        internal LineSeries CreateSeries(String name, ICollection<Double> xParts, ICollection<Double> yParts)
        {
            if (xParts.Count != yParts.Count) {
                throw new ArgumentException("Number of elements in parts doesn't match.");
            }
            LineSeries ls = new LineSeries(name);
            ls.Points = xParts.Select((x, i) => ((IDataPoint)new DataPoint(x, yParts.ElementAt(i)))).ToList();
            return ls;
        }
    }
}
