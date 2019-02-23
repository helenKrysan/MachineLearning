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

        private void ParsenWindowDisplayCommandImpl(object obj)
        {
            
        }

    }
}
