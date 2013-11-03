using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot.Series;

namespace Dsp.Support
{
    public class SeriesManager
    {
        private readonly IDictionary<String, Series> series = new Dictionary<string, Series>();
    
        private readonly ICollection<String> activeSeries = new List<string>();

        public ICollection<String> ActiveSeries { get { return activeSeries; } } 


        public void Add(String name, Series value)
        {
            series.Add(name, value);
        }

        public Series Get(String name)
        {
            return series[name];
        }

        public IEnumerable<Series> GetActive()
        {
            return series.Where(s => activeSeries.Contains(s.Key)).Select(pair => pair.Value);
        } 
    }
}
