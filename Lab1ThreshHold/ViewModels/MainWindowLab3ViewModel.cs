using System.Windows.Media.Imaging;
using Lab1ThreshHold.Infrastructure.Captures;

namespace Lab1ThreshHold.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;
    using Infrastructure;
    using Image = System.Windows.Controls.Image;
    using System.Threading.Tasks;
    using System.Windows;
    using Views;

    public class MainWindowLab3ViewModel :ViewModelBase
    {
        public MainWindowLab3ViewModel()
        {
        }

        #region MainResult property

        public BitmapSource MainResult
        {
            get { return GetValue<BitmapSource>(MainResultProperty); }
            set { SetValue(MainResultProperty, value); }
        }

        public static readonly PropertyData MainResultProperty = RegisterProperty("MainResult", typeof(BitmapSource));

        #endregion


        #region SobelResult property

        public BitmapSource SobelResult
        {
            get { return GetValue<BitmapSource>(SobelResultProperty); }
            set { SetValue(SobelResultProperty, value); }
        }

        public static readonly PropertyData SobelResultProperty = RegisterProperty("SobelResult", typeof(BitmapSource));
        private Image imageSrcLink;
        Image imageMainLink;

        #endregion
        protected override async Task InitializeAsync()
        {
            imageSrcLink = Container<Window>.FindElement<Image>(typeof(MainWindowLab3), window => window.Name == "imageSobel");
            imageMainLink = Container<Window>.FindElement<Image>(typeof(MainWindowLab3), window => window.Name == "imageMain");
            Lab3Grip lab3Grip=new Lab3Grip();
            lab3Grip.OnSobelResult += Lab3Grip_OnSobelResult;
            lab3Grip.OnOriginalResult += Lab3Grip_OnOriginalResult;
            //lab3Grip.OnNoisedImage += Lab3Grip_OnSobelResult;
            //lab3Grip.OnResultImage += Lab3Grip_OnOriginalResult;
            await lab3Grip.DoProcessAsync();
            await base.InitializeAsync();}


        private void Lab3Grip_OnOriginalResult(object sender, Infrastructure.FrameCapture.CaptureArgs<Emgu.CV.Image<Emgu.CV.Structure.Bgr, float>> e)
        {
            imageMainLink.Dispatcher.Invoke(() => {
                MainResult = e.Result.ToBitmap().Convert();
            });
        }

        private void Lab3Grip_OnSobelResult(object sender, Infrastructure.FrameCapture.CaptureArgs<Emgu.CV.Image<Emgu.CV.Structure.Bgr, float>> e)
        {
            imageSrcLink.Dispatcher.Invoke(() => {
                SobelResult = e.Result.ToBitmap().Convert();
            });
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
