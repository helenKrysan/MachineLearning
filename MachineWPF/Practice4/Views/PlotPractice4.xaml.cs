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
using MachineWPF.Practice4.ViewModels;

namespace MachineWPF.Practice4.Views
{
    /// <summary>
    /// Interaction logic for PlotPractice4.xaml
    /// </summary>
    public partial class PlotPractice4 : Window
    {
        private PlotPractice4ViewModel _viewModel;

        public PlotPractice4(int type)
        {
            _viewModel = new PlotPractice4ViewModel(type);
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
