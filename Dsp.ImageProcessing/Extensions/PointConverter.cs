using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Dsp.ImageProcessing.Extensions
{
    public static class PointConverter
    {
        public static PointCollection ToPointCollection(this int[] values)
        {
            int max = values.Max();

            PointCollection points = new PointCollection();
            // first point (lower-left corner)
            points.Add(new Point(0, max));
            // middle points
            for (int i = 0; i < values.Length; i++)
            {
                points.Add(new Point(i, max - values[i]));
            }
            // last point (lower-right corner)
            points.Add(new Point(values.Length - 1, max));

            return points;
        }
    }
}
