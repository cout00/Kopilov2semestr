using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Charts;
using Lab1ThreshHold.Infrastructure;
using Ninject;

namespace Lab1ThreshHold.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Container<Window>.AddToContainer(this);
            Container<ChartControl>.AddToContainer(chartRes);
            Container<ChartControl>.AddToContainer(chartSrc);
        }
    }
}
