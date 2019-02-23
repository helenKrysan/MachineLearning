using MachineWPF.Practice2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    abstract class AbstractClassification<T, C> where T : IMetric<T> where C : IComparable
    {
        private Dictionary<T, C> _precedents = new Dictionary<T, C>();

        public Dictionary<T, C> Precedents { get { return _precedents; } }

        public int LeaveOneOut(Func<T, Dictionary<T, C>, C> evaluate)
        {
            int countMissmatch = 0;

            foreach (var t in Precedents)
            {
                var precedentsTest = new Dictionary<T, C>(Precedents);
                precedentsTest.Remove(t.Key);
                var res = evaluate(t.Key, precedentsTest);
                if (!res.Equals(t.Value))
                {
                    countMissmatch++;
                }
            }

            return countMissmatch;
        }

    }
}
