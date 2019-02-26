using MachineWPF.Practice2.ViewModels;
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

namespace MachineWPF.Practice2
{
    /// <summary>
    /// Interaction logic for PlotPractice2.xaml
    /// </summary>
    public partial class PlotPractice2 : Window
    {
        private PlotPractice2ViewModel _viewModel;

        public PlotPractice2(int type)
        {
            _viewModel = new PlotPractice2ViewModel(type);
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
