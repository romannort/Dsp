namespace Dsp.ImageProcessing.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvolutionMatrix
    {

        private double[,] Matrix;

        /// <summary>
        /// 
        /// </summary>
        private int matrixSize = 3;
        
        /// <summary>
        /// The result of calculation will be divided by this divisor. 
        /// </summary>
        /// <remarks>
        /// You will hardly use 1, which lets result unchanged, and 9 or 25 according to matrix size, 
        /// which gives the average of pixel values.
        /// </remarks>
        public double Factor = 1;
        
        /// <summary>
        /// This value is added to the division result. 
        /// </summary>
        /// <remarks>
        /// This is useful if result may be negative. This offset may be negative.
        /// </remarks>
        public double Offset = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public ConvolutionMatrix(int size)
        {
            matrixSize = size;
            Matrix = new double[size, size];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetAll(double value)
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    Matrix[i, j] = value;
                }
            }
        }  
    }
}
