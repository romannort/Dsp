using System.Drawing;

namespace Dsp.ImageProcessing.Extensions
{
    public static class ColorConverters
    {
        public static int ColorByName(this Color src, string propName)
        {
            byte result = (byte)(src.GetType()
                .GetProperty(propName)
                .GetValue(src, null));
            
            return result;
        }       
    }
}
