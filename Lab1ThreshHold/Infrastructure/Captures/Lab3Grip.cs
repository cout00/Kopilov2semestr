using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Lab1ThreshHold.Infrastructure.FrameCapture;

namespace Lab1ThreshHold.Infrastructure.Captures
{
    public class Lab3Grip :Grip
    {
        private readonly short[,] _kernel;

        T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
            for (int j = 0; j < minCols; j++)
                newArray[i, j] = original[i, j];
            return newArray;
        }

        short[,] GetNearKernel(short[,] nearKernel)
        {
            var max = nearKernel.GetLength(0) > nearKernel.GetLength(1)
                ? nearKernel.GetLength(0)
                : nearKernel.GetLength(1);
            return ResizeArray<short>(nearKernel, max, max);
        }


        short[,] Kernel=new short[4,4]
        {
            {1,1,1,1},
            { 0,0,0,0},
            {-1,-1,-1,-1},
            { 0,0,0,0},
        };

        public Lab3Grip(short[,] kernel)
        {
            _kernel = GetNearKernel(kernel);
        }

        protected override void Process(Image<Gray, byte> queryImage)
        {
            var data = queryImage.Data;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    
                }
            }
        }
    }
}
