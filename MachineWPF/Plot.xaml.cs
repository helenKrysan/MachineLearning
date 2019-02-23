using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MachineWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Plot : Window
    {
        private PlotViewModel viewModel;
        private int type;

        public Plot(int type)
        {
            this.type = type;
            viewModel = new PlotViewModel(type);
            DataContext = viewModel;

            InitializeComponent();
            switch (type)
            {
                case 1:
                case 2:
                case 3:
                    Risk.Content = "Emperical Risk : " + PlotViewModel.eRisk;
                    break;
                case 4:
                    Risk.Content = "Class : " + PlotViewModel.classX;
                    K.Content = "Best k : " +  PlotViewModel.bestK;
                    break;
                case 5:
                    Risk.Content = "Class : " + PlotViewModel.classX;
                    K.Content = "Best h : " + PlotViewModel.bestH;
                    break;
                case 6:

                    break;
                default: break;
            }

        }

        private void XButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            string[] s = XBox.Text.Split(',');
            Point p = new Point (double.Parse(s[0],CultureInfo.InvariantCulture), double.Parse(s[1], CultureInfo.InvariantCulture));
            PlotViewModel.testPoint = p;
            viewModel.PlotBuild(type);
            DataContext = viewModel;
            switch (type)
            {
                case 1:
                case 2:
                case 3:
                    Risk.Content = "Emperical Risk : " + PlotViewModel.eRisk;
                    break;
                case 4:
                    Risk.Content = "Class : " + PlotViewModel.classX;
                    K.Content = "Best k : " + PlotViewModel.bestK;
                    break;
                case 5:
                    Risk.Content = "Class : " + PlotViewModel.classX;
                    K.Content = "Best h : " + PlotViewModel.bestH;
                    break;
                case 6:

                    break;
                default: break;
            }
        }

        private void RButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            PlotViewModel.isRandom = true;
            viewModel.PlotBuild(type);
            DataContext = viewModel;
            switch (type)
            {
                case 1:
                case 2:
                case 3:
                    Risk.Content = "Emperical Risk : " + PlotViewModel.eRisk;
                    break;
                case 4:
                    Risk.Content = "Class : " + PlotViewModel.classX;
                    K.Content = "Best k : " + PlotViewModel.bestK;
                    break;
                case 5:
                    Risk.Content = "Class : " + PlotViewModel.classX;
                    K.Content = "Best h : " + PlotViewModel.bestH;
                    break;
                case 6:

                    break;
                default: break;
            }
        }
    }
}
