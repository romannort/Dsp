using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsp.ImageProcessing.Extensions
{
    /// <summary></summary>
    internal static class ChannelsFromARGBExtension
    {
        public static short RED = 16;

        public static short GREEN = 8;

        public static short BLUE = 0;

        public static int GetChannel(this int color, short offset)
        {
            return (color >> offset) & 0xFF;
        }
    }
}
