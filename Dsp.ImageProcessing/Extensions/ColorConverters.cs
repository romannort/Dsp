using System.Drawing;

namespace Dsp.ImageProcessing.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ColorConverters
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static int ColorChannelByCode(this Color src, ColorCode code)
        {
            switch (code)
            {
                case ColorCode.R: return src.R;
                case ColorCode.G: return src.G;
                case ColorCode.B: return src.B;
                default: throw new System.ArgumentException("Invalid color code.");
            }
        }       
    }
}
