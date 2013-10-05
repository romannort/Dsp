namespace Dsp
{
    public class PerformanceStats
    {
        public static int FftMultiplications { get; set; }

        public static int FftAdditions { get; set; }

        public static int DftMultiplications { get; set; }

        public static int DftAdditions { get; set; }

        public static void Clear()
        {
            FftAdditions = 0;
            FftMultiplications = 0;
            DftAdditions = 0;
            DftMultiplications = 0;
        }
    }
}
