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
    public class Lab4Grip :Grip
    {
        public event EventHandler<CaptureArgs<Image<Bgr, float>>> OnNoisedImage; 
        public event EventHandler<CaptureArgs<Image<Bgr, float>>> OnResultImage;


        byte[,,] DoRandomNoise(byte[,,] data, double noisePow)
        {
            Random rand=new Random();
            for (int i = 0; i < data.GetLength(0) * data.GetLength(1) * noisePow; i++)
            {
                data[rand.Next(0, data.GetLength(0) - 1),
                    rand.Next(0, data.GetLength(1) - 1), 0] = rand.Next(0, 1) == 0 ? (byte)0 : (byte)255;
            }
            return data;
        }

        private int d = 35;

        byte[,,] Medianvolution(byte[,,] data, int kernelSize)
        {
            var kernelStep = (int)(kernelSize / 2);
            var resultArray = new byte[data.GetLength(0), data.GetLength(1), 1];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    var kernelPosX = 0;
                    List<byte> intences=new List<byte>();
                    for (int k = i - kernelStep; kernelPosX < kernelSize; k++)
                    {
                        var kernelPosY = 0;
                        for (int l = j - kernelStep; kernelPosY < kernelSize; l++)
                        {
                            int newI = k >= data.GetLength(0) - 1 ? data.GetLength(0) - 2 - kernelPosY : Math.Abs(k);
                            int newJ = l >= data.GetLength(1) - 1 ? data.GetLength(1) - 2 - kernelPosX : Math.Abs(l);
                            intences.Add(data[newI, newJ, 0]);                                            
                            kernelPosY++;
                        }
                        kernelPosX++;
                    }
                    intences.Sort();//var test= ;
                    resultArray[i, j, 0] = (byte)intences.GetRange(d / 2, intences.Count - d).Average(a => a);
                }
            }
            return resultArray;
        }

        protected override void Process(Image<Gray, byte> queryImage)
        {
            var data = DoRandomNoise(queryImage.Data, 0.3);
            OnNoisedImage?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(new Image<Gray, byte>(data).Convert<Bgr,float>()));
            var result= Medianvolution(data, 6);
            OnResultImage?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(new Image<Gray, byte>(result).Convert<Bgr, float>()));
        }
    }
}
