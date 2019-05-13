using MachineWPF.Practice3.ViewModels;
using System.Windows;

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
