using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Catel.Data;
using Catel.Windows.Threading;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Lab1ThreshHold.ViewModels
{
    using Catel.MVVM;
    using Emgu.CV.CvEnum;
    using System.Threading.Tasks;

    public class MainWindowViewModel :ViewModelBase
    {
        volatile Capture capture = new Capture();
        private DateTime _dateTime = DateTime.Now;
        public MainWindowViewModel()
        {
            
        }

        public BitmapSource Context
        {
            get { return GetValue<BitmapSource>(ContextProperty); }
            set { SetValue(ContextProperty, value); }
        }




        public async Task SetCapture()
        {
            var frames = 1;         
            await Task.Run(() => {
                while (true)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var query = capture.QueryFrame().ToImage<Gray, byte>().ApplyFunc(pixel =>
                        {
                            return pixel < 128 ? byte.MinValue : byte.MaxValue;
                        });
                        var seconds = (DateTime.Now - _dateTime).Seconds==0
                            ? 1
                            : (DateTime.Now - _dateTime).Seconds;
                        var queryBgr = query.Convert<Bgr, float>();
                        CvInvoke.PutText(queryBgr, "FPS: "+(frames/seconds).ToString(), new System.Drawing.Point(10, 60), FontFace.HersheySimplex, 1.0, new Bgr(Color.Red).MCvScalar);
                        Context = queryBgr.Bitmap.Convert();                        
                    });
                    frames++;
                    Task.Delay(16).Wait();
                }
            });
        }

        public static readonly PropertyData ContextProperty = RegisterProperty("Context", typeof(BitmapSource));

        protected override async Task InitializeAsync()
        {
            await SetCapture();
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
