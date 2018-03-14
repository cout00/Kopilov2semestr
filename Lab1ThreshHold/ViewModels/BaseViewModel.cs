using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Catel.MVVM;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Lab1ThreshHold.ViewModels
{
    public abstract class BaseViewModel
    {
        Capture capture = new Capture();
        private DateTime _dateTime = DateTime.Now;
        private event EventHandler<CaptureArgs> OnCapture; 

        public BaseViewModel()
        {

        }

        protected abstract Image<Gray, byte> ProcessQuery(Image<Gray, byte> query);
        
        public async Task Process()
        {
            var frames = 1;
            await Task.Run(() => {
                while (true)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        var query = capture.QueryFrame().ToImage<Gray, byte>();
                        CaptureArgs arg=new CaptureArgs() {Image = query};
                        OnCapture?.Invoke(this,arg);
                        if (arg.OutPutImage!=null)
                        {
                            //var arg.OutPutImage = query.Convert<Bgr, float>();
                            //CvInvoke.PutText(queryBgr, "FPS: " + (frames / seconds).ToString(), new System.Drawing.Point(10, 60), FontFace.HersheySimplex, 1.0, new Bgr(Color.Red).MCvScalar);
                            //ContextChanged = queryFunc.Convert<Bgr, float>().Bitmap.Convert();
                            //Context = queryBgr.Bitmap.Convert();
                        }
                    });
                    frames++;
                    Task.Delay(16).Wait();
                }
            });
        }
    }


    public class CaptureArgs : EventArgs
    {
        public Image<Gray, byte> Image { get; set; }
        public Image<Gray, byte> OutPutImage { get; set; }

    }

}
