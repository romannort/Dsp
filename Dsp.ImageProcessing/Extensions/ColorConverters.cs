using System.Drawing;

namespace Dsp.ImageProcessing.Extensions
{
    public static class ColorConverters
    {

        static string Red = "R";
        static string Green = "G";
        static string Blue = "B";

        public static int ColorByName(this Color src, string propName)
        {
            //byte result = (byte)(src.GetType()
            //    .GetProperty(propName)
            //    .GetValue(src, null));

            if (propName == Red) return src.R;
            if (propName == Green) return src.G;
            return src.B;
            
            //return result;
        }       
    }
}
