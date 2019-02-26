using MachineWPF.Practice2;
using MachineWPF.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineWPF
{

    class MainViewModel
    {

        private RelayCommand _parsenWindowDisplayCommand;

        public RelayCommand ParsenWindowDisplayCommand
        {
            get
            {
                return _parsenWindowDisplayCommand ?? (_parsenWindowDisplayCommand = new RelayCommand(ParsenWindowDisplayCommandImpl));
            }
            set
            {
                _parsenWindowDisplayCommand = value;
            }
        }

        private RelayCommand _kNearestNeighborsDisplayCommand;

        public RelayCommand KNearestNeighborsDisplayCommand
        {
            get
            {
                return _kNearestNeighborsDisplayCommand ?? (_kNearestNeighborsDisplayCommand = new RelayCommand(KNearestNeighborsDisplayCommandImpl));
            }
            set
            {
                _kNearestNeighborsDisplayCommand = value;
            }
        }

        private void KNearestNeighborsDisplayCommandImpl(object obj)
        {
            PlotPractice2 plot = new PlotPractice2(4);
            plot.Show();
        }


        private void ParsenWindowDisplayCommandImpl(object obj)
        {
            PlotPractice2 plot = new PlotPractice2(5);
            plot.Show();
        }


    }
}
