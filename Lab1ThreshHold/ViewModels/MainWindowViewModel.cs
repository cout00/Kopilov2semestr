using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Catel.Data;
using Catel.Windows.Threading;
using DevExpress.Xpf.Charts;
using Emgu.CV;
using Emgu.CV.Structure;
using Lab1ThreshHold.Infrastructure;
using Lab1ThreshHold.Infrastructure.Captures;
using Lab1ThreshHold.Views;
using Ninject;
using Image = System.Windows.Controls.Image;

namespace Lab1ThreshHold.ViewModels
{
    using Catel.MVVM;
    using Emgu.CV.CvEnum;
    using Lab1ThreshHold;
    using System.Threading.Tasks;

    public class MainWindowViewModel :ViewModelBase
    {
        private Image imageSrcLink;
        private Image imageResLink;
        private ChartControl chartSrcLink;
        private ChartControl chartResLink;
        public MainWindowViewModel()
        {
        }

        public BitmapSource ContextChanged
        {
            get { return GetValue<BitmapSource>(ContextChangedProperty); }
            set { SetValue(ContextChangedProperty, value); }
        }

        public static readonly PropertyData ContextChangedProperty = RegisterProperty("ContextChanged", typeof(BitmapSource));

        public List<System.Windows.Point> Points
        {
            get { return GetValue<List<System.Windows.Point>>(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly PropertyData PointsProperty = RegisterProperty("Points", typeof(List<System.Windows.Point>));

        public BitmapSource Context
        {
            get { return GetValue<BitmapSource>(ContextProperty); }
            set { SetValue(ContextProperty, value); }
        }

        #region PointsRes property

        public List<Point> PointsRes
        {
            get { return GetValue<List<Point>>(PointsResProperty); }
            set { SetValue(PointsResProperty, value); }
        }

        public static readonly PropertyData PointsResProperty = RegisterProperty("PointsRes", typeof(List<Point>));

        #endregion


        public static readonly PropertyData ContextProperty = RegisterProperty("Context", typeof(BitmapSource));

        protected override async Task InitializeAsync()
        {
            imageSrcLink = Container<Window>.FindElement<Image>(typeof(MainWindow), window => window.Name == "imageSrc");
            imageResLink = Container<Window>.FindElement<Image>(typeof(MainWindow), window => window.Name == "imageRes");
            chartSrcLink = Container<ChartControl>.GetElement(control => control.Name == "chartSrc");
            chartResLink= Container<ChartControl>.GetElement(control => control.Name == "chartRes");
            Lab2Grip simpleGrip = new Lab2Grip();
            simpleGrip.OnHistogramPoints += SimpleGrip_OnHistogramPoints;
            simpleGrip.OnOriginalResult += SimpleGrip_OnOriginalResult;
            simpleGrip.OnHistogramResultPoints += SimpleGrip_OnHistogramResultPoints;
            simpleGrip.OnResultImage += SimpleGrip_OnResultImage;
            await simpleGrip.DoProcessAsync();

            await base.InitializeAsync();
        }

        private void SimpleGrip_OnResultImage(object sender, Infrastructure.FrameCapture.CaptureArgs<Image<Bgr, float>> e)
        {
            imageResLink.Dispatcher.Invoke(() => {
                ContextChanged = e.Result.ToBitmap().Convert();
            });
        }

        private void SimpleGrip_OnHistogramResultPoints(object sender, Infrastructure.FrameCapture.CaptureArgs<List<Point>> e)
        {
            chartResLink.Dispatcher.Invoke(() => {
                chartResLink.BeginInit();
                PointsRes = e.Result;
                chartResLink.EndInit();
            });
        }

        private void SimpleGrip_OnHistogramPoints(object sender,
            Infrastructure.FrameCapture.CaptureArgs<List<System.Windows.Point>> e)
        {
            chartSrcLink.Dispatcher.Invoke(() => {
                chartSrcLink.BeginInit();
                Points = e.Result;
                chartSrcLink.EndInit();
            });
        }

        private void SimpleGrip_OnOriginalResult(object sender, Infrastructure.FrameCapture.CaptureArgs<Image<Bgr, float>> e)
        {
            imageSrcLink.Dispatcher.Invoke(() => {
                Context = e.Result.ToBitmap().Convert();
            });
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
