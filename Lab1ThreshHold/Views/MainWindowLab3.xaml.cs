using Lab1ThreshHold.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab1ThreshHold.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindowLab3.xaml
    /// </summary>
    public partial class MainWindowLab3
    {
        public MainWindowLab3()
        {
            InitializeComponent();
            Container<Window>.AddToContainer(this);
        }
    }
}
