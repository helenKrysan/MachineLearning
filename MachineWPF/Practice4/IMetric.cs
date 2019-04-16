using System;

namespace MachineWPF.Practice4
{
    interface IMetric<T> 
    {
       double Distance(T p);
        
    }
}
