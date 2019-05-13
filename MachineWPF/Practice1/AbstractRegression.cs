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

        internal double EmpericRisk(Func<T, C> function)
        {
            double res = 0;
            foreach (var p in _precedents)
            {
                res += Math.Pow(Convert.ToDouble(p.Value) - Convert.ToDouble(function(p.Key)), 2);
            }
            return res;
        }
    }
}
