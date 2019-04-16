using MachineWPF.Practice3.ViewModels;
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

namespace MachineWPF.Practice3.Views
{
    public partial class PlotPractice3 : Window
    {
        private PlotPractice3ViewModel _viewModel;

        public PlotPractice3(int type)
        {
            _viewModel = new PlotPractice3ViewModel(type);
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
