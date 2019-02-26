using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    class Sample<T, C> : AbstractClassification<T, C> where T : IMetric<T> where C : IComparable
    {

       
    }
}
