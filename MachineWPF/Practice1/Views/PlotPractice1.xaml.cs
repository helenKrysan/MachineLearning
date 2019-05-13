using System.Windows;
using MachineWPF.Practice1.ViewModels;

namespace MachineWPF.Practice1.Views
{
    /// <summary>
    /// Interaction logic for PlotPractice1.xaml
    /// </summary>
    public partial class PlotPractice1 : Window
    {
        private PlotPractice1ViewModel _viewModel;

        public PlotPractice1(int type)
        {
            _viewModel = new PlotPractice1ViewModel(type);
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
