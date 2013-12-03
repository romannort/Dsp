namespace Dsp.ImageProcessing
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvolutionMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        public int MatrixSize = 3;

        public double[,] Matrix;
        
        public double Factor = 1;
        
        public double Offset = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public ConvolutionMatrix(int size)
        {
            MatrixSize = 3;
            Matrix = new double[size, size];
        }

        public void SetAll(double value)
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                for (int j = 0; j < MatrixSize; j++)
                {
                    Matrix[i, j] = value;
                }
            }
        }  
    }
}
