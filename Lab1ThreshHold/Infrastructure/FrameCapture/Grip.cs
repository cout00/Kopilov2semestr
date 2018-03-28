using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Lab1ThreshHold.Infrastructure.FrameCapture
{
    abstract public class Grip
    {
        Capture capture = new Capture();
        public event EventHandler<CaptureArgs<Image<Bgr, float>>> OnOriginalResult;
        private DateTime _dateTime = DateTime.Now;
        private int frames = 1;
        public Grip()
        {

        }

        protected Image<TColor, TDepth> AddFrameRate<TColor, TDepth>(Image<TColor, TDepth> inpImage) where TColor : struct, IColor
            where TDepth : new()
        {
            var seconds = (DateTime.Now - _dateTime).TotalSeconds == 0
                ? 1
                : (DateTime.Now - _dateTime).TotalSeconds;
            CvInvoke.PutText(inpImage, "FPS: " + (Math.Round(frames / seconds)).ToString(), new System.Drawing.Point(10, 60), FontFace.HersheySimplex, 1.0, new Bgr(Color.Red).MCvScalar);
            return inpImage;
        }

        public async Task DoProcessAsync()
        {
            ////var query = capture.QueryFrame().ToImage<Gray, byte>();
            //var data = new byte[10, 6, 1];
            //for (int i = 0; i < data.GetLength(0); i++)
            //{
            //    for (int j = 0; j < data.GetLength(1); j++)
            //    {
            //        data[i, j, 0] = (byte)i;
            //    }
            //}
            ////OnOriginalResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(AddFrameRate(query).Convert<Bgr, float>()));

            ////OnOriginalResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(AddFrameRate(new Image<Gray, byte>(data)).Convert<Bgr, float>()));



            //Process(new Image<Gray, byte>(data));

            ////Process(query.Convert<Gray, byte>());
            //Task.Delay(50).Wait();





            await Task.Run(() => {
                while (true)
                {
                    var query = capture.QueryFrame().ToImage<Gray, byte>();                    
                    //var data = new byte[10, 6, 1];
                    //for (int i = 0; i < data.GetLength(0); i++)
                    //{
                    //    for (int j = 0; j < data.GetLength(1); j++)
                    //    {
                    //        data[i, j, 0] = (byte)i;
                    //    }
                    //}
                    
                    OnOriginalResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(AddFrameRate(query).Convert<Bgr, float>()));

                    //OnOriginalResult?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(AddFrameRate(new Image<Gray, byte>(data)).Convert<Bgr, float>()));



                    //Process(new Image<Gray, byte>(data));

                    Process(query.Convert<Gray, byte>());
                    Task.Delay(50).Wait();
                    frames++;
                }
            });
        }
        

        protected abstract void Process(Image<Gray, byte> queryImage);
    }

    public class CaptureArgs<Type> :EventArgs
    {
        public Type Result { get; private set; }
        public CaptureArgs(Type result)
        {
            Result = result;
        }
    }


}
