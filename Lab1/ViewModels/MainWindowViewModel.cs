using System.Windows.Media.Imaging;
using Catel.Data;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Lab1.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel :ViewModelBase
    {
        volatile Capture capture=new Capture();
        public MainWindowViewModel()
        {

        }

        public BitmapImage Context
        {
            get { return GetValue<BitmapImage>(ContextProperty); }
            set { SetValue(ContextProperty, value); }
        }

        public async Task SetCapture()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Context = capture.QueryFrame().Bitmap.Convert();
                    Task.Delay(50).Wait();
                }                
            });
        }

        public static readonly PropertyData ContextProperty = RegisterProperty("Context", typeof(BitmapImage));

        protected override async Task InitializeAsync()
        {
            await SetCapture();
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
