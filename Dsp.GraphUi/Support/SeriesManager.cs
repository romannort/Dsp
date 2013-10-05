using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot.Series;

namespace Dsp.GraphUi.Support
{
    internal class SeriesManager
    {
        private readonly IDictionary<String, Series> series = new Dictionary<string, Series>();
    
        private readonly ICollection<String> activeSeries = new List<string>();

        internal ICollection<String> ActiveSeries { get { return activeSeries; } } 


        internal void Add(String name, Series value)
        {
            series.Add(name, value);
        }

        internal Series Get(String name)
        {
            return series[name];
        }

        internal IEnumerable<Series> GetActive()
        {
            return series.Where(s => activeSeries.Contains(s.Key)).Select(pair => pair.Value);
        } 
    }
}
