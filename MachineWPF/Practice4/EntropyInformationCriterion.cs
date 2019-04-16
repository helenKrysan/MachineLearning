using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    class EntropyInformationCriterion : AbstractInformationCriterion
    {

        public double Information()
        {
            return -1 * ((double)Positive / (double)PositiveTotal) * Math.Log((double)Negative/(double)NegativeTotal);
        }

    }
}
