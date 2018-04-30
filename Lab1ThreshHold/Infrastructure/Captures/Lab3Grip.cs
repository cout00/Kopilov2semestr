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
        //T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        //{
        //    var newArray = new T[rows, cols];
        //    int minRows = Math.Min(rows, original.GetLength(0));
        //    int minCols = Math.Min(cols, original.GetLength(1));
        //    for (int i = 0; i < minRows; i++)
        //        for (int j = 0; j < minCols; j++)
        //            newArray[i, j] = original[i, j];
        //    return newArray;
        //}

        //float[,] GetNearKernel(float[,] nearKernel)
        //{
        //    var max = nearKernel.GetLength(0) > nearKernel.GetLength(1)
        //        ? nearKernel.GetLength(0)
        //        : nearKernel.GetLength(1);
        //    max = max % 2 == 0 ? max + 1 : max;
        //    return ResizeArray<float>(nearKernel, max, max);
        //}

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


        

        double[,,] Copy(double[,,] inpArray)
        {
            var temp = new double[inpArray.GetLength(0), inpArray.GetLength(1), 1];
            for (int i = 0; i < inpArray.GetLength(0); i++)
            {
                for (int j = 0; j < inpArray.GetLength(1); j++)
                {
                    temp[i, j, 0] = inpArray[i, j, 0];
                }
            }
            return temp;
        }

        double[,,] ToDouble(byte[,,] inpArray)
        {
            var temp = new double[inpArray.GetLength(0), inpArray.GetLength(1), 1];
            for (int i = 0; i < inpArray.GetLength(0); i++)
            {
                for (int j = 0; j < inpArray.GetLength(1); j++)
                {
                    temp[i, j, 0] = inpArray[i, j, 0];
                }
            }
            return temp;
        }

        protected override void Process(Image<Gray, byte> queryImage)
        {
            var alpha = 0.95;            
            double[,,] test = new double[300, 300, 1];
            for (int i = 0; i < test.GetLength(0); i++)
            {
                for (int j = 0; j < test.GetLength(1); j++)
                {
                    if (i > 140 && i < 160 && j > 140 && j < 160)
                    {
                        test[i, j, 0] = 255;
                    }
                }
            }

            var data = Copy(test);

            //var data = ToDouble(queryImage.Data);
            var temp = Copy(data);
            var tempCopy = Copy(temp);
            byte newVal = 0;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 1; j < data.GetLength(1); j++)
                {
                    temp[i, j, 0] = (alpha * temp[i, j - 1, 0] + (1 - alpha) * data[i, j, 0]);
                }
                for (int j = data.GetLength(1) - 1; j >= 0; j--)
                {
                    if (j == data.GetLength(1) - 1)
                    {
                        temp[i, j, 0] = ((temp[i, j, 0] + tempCopy[i, j, 0]) / 2);
                    }
                    else
                    {
                        tempCopy[i, j, 0] = (alpha * tempCopy[i, j + 1, 0] + (1 - alpha) * data[i, j, 0]);
                        temp[i, j, 0] = ((temp[i, j, 0] + tempCopy[i, j, 0]) / 2);
                    }
                }
            }
            var temp2 = Copy(temp);
            tempCopy = Copy(temp);

            for (int i = 0; i < temp2.GetLength(1); i++)
            {
                for (int j = 1; j < temp2.GetLength(0); j++)
                {
                    temp2[j, i, 0] = (alpha * temp2[j-1, i, 0] + (1 - alpha) * temp[j, i, 0]);
                }
                for (int j = data.GetLength(0) - 1; j >= 0; j--)
                {
                    if (j == data.GetLength(0) - 1)
                    {
                        temp2[j, i, 0] = ((temp2[j, i, 0] + tempCopy[j, i, 0]) / 2);
                    }
                    else
                    {
                        tempCopy[j, i, 0] = (alpha * tempCopy[j+1, i, 0] + (1 - alpha) * temp[j, i, 0]);
                        temp2[j, i, 0] = ((temp2[j, i, 0] + tempCopy[j, i, 0]) / 2);
                    }
                }
            }

            var resultArray = new byte[temp2.GetLength(0), temp2.GetLength(1), temp2.GetLength(2)];
            for (int i = 0; i < resultArray.GetLength(0); i++)
            {
                for (int j = 0; j < resultArray.GetLength(1); j++)
                {
                   // resultArray[i, j, 0] = (byte)(temp2[i, j, 0]);

                    resultArray[i, j, 0] = (temp2[i, j, 0] * 25)>255?(byte)255:(byte)(temp2[i, j, 0] * 25);
                }
            }



            OnSobelResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(new Image<Gray, byte>(resultArray).Convert<Bgr, float>()));
        }

    }
}
