using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    class HeuristicInformationCriterion : AbstractInformationCriterion
    {

        public double Information()
        {
            return ((double)Positive / ((double)Positive + (double)Negative));
        }

    }
}
