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
        public event EventHandler<CaptureArgs<Image<Bgr, float>>> OnSobelResult;        
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

        float[,] GetNearKernel(float[,] nearKernel)
        {
            var max = nearKernel.GetLength(0) > nearKernel.GetLength(1)
                ? nearKernel.GetLength(0)
                : nearKernel.GetLength(1);
            max = max % 2 == 0 ? max + 1 : max;
            return ResizeArray<float>(nearKernel, max, max);
        }

        float[,] kernelY = new float[,]
        {
            {-1,-1 ,-1,-1,-1 },
            {-1,-1 ,-1,-1,-1 },
            {-1,-1 ,24,-1,-1 },
            {-1,-1 ,-1,-1,-1 },
            {-1,-1 ,-1,-1,-1 }
        }
            ;

        float[,] kernelX = new float[,]
        {
            {-1,0 ,1 },
            {-2,0,2 },
            {-1,0,1 }
        };

        public Lab3Grip()
        {
        }


        byte[,,] Convolution(byte[,,] data, float[,] kernel)
        {
            var kernelStep = (int)(kernel.GetLength(0) / 2);
            var kernelSize = kernel.GetLength(0);
            var resultArray = new byte[data.GetLength(0), data.GetLength(1), 1];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    var kernelPosX = 0;
                    var kernelRezult = 0;
                    for (int k = i - kernelStep; kernelPosX < kernelSize; k++)
                    {
                        var kernelPosY = 0;
                        for (int l = j - kernelStep; kernelPosY < kernelSize; l++)
                        {
                            int newI = k >= data.GetLength(0) - 1 ? data.GetLength(0) - 2 - kernelPosY : Math.Abs(k);
                            int newJ = l >= data.GetLength(1) - 1 ? data.GetLength(1) - 2 - kernelPosX : Math.Abs(l);
                            kernelRezult += (int)kernel[kernelPosX, kernelPosY] *
                                            data[newI, newJ, 0];
                            kernelPosY++;
                        }
                        kernelPosX++;
                    }
                    if (kernelRezult>255)
                    {
                        kernelRezult = 255;
                    }
                    if (kernelRezult<0)
                    {
                        kernelRezult = 0;
                    }
                    //kernelRezult = kernelRezult >= 128 ? 255 : 0;
                    resultArray[i, j, 0] = (byte)kernelRezult;
                }
            }
            return resultArray;
        }



        protected override void Process(Image<Gray, byte> queryImage)
        {
            //var arrayX = Convolution(queryImage.Data, kernelX);
            var arrayY = Convolution(queryImage.Data, kernelY);
            //var resultArray = new byte[queryImage.Data.GetLength(0), queryImage.Data.GetLength(1), 1];
            //for (int i = 0; i < arrayX.GetLength(0); i++)
            //{
            //    for (int j = 0; j < arrayX.GetLength(1); j++)
            //    {
            //        //var kernelRezult = Math.Sqrt(Math.Pow(arrayX[i, j, 0], 2) + Math.Pow(arrayY[i, j, 0], 2));

            //        var kernelRezult = arrayX[i, j, 0] + arrayY[i, j, 0];
            //        if (kernelRezult > 255)
            //        {
            //            kernelRezult = 255;
            //        }
            //        if (kernelRezult < 0)
            //        {
            //            kernelRezult = 0;
            //        }
            //        resultArray[i, j, 0] = (byte)kernelRezult;
            //    }
            //}
            //var result = queryImage.Convolution(new ConvolutionKernelF(kernelY));
            //var sobelx = queryImage.Sobel(1, 0, 3);
            //var sobely = queryImage.Sobel(1, 1, 3).Convert<Gray, byte>(); ;
            //var dif = sobely.AbsDiff(sobely).Convert<Gray, byte>();
            OnSobelResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(queryImage.AbsDiff((new Image<Gray, byte>(arrayY))).Convert<Bgr, float>()));

            //OnSobelResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(new Image<Gray, byte>(sobely.Data).Convert<Bgr, float>()));
            //OnSobelResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(new Image<Gray, byte>(data).Convolution(new ConvolutionKernelF(_kernel)).Convert<Bgr, float>()));

            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }

    }
}
