using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice1
{
    abstract class AbstractRegression<T,C>
    {
        private Dictionary<T, C> _precedents = new Dictionary<T, C>();

        public Dictionary<T, C> Precedents { get { return _precedents; } }

/*        private double EmpericRisk()
        {
            double res = 0;
            for (int i = 0; i < size - 1; i++)
            {
                res += Math.Pow(testResultValues[i] - TestF(testValues[i]), 2);
            }
            return res;
        }*/
    }
}
