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

        public int LeaveOneOut(Func<T, Dictionary<T, C>, C> evaluate, Dictionary<T, C> precedentsTest)
        {
            int countMissmatch = 0;

            foreach (var t in Precedents)
            {
                var precedentsTestCopy = new Dictionary<T, C>(precedentsTest);
                if(precedentsTestCopy.ContainsKey(t.Key))
                    precedentsTestCopy.Remove(t.Key);
                var res = evaluate(t.Key, precedentsTestCopy);
                if (!res.Equals(t.Value))
                {
                    countMissmatch++;
                }
            }

            return countMissmatch;
        }

        public Dictionary<T,C> EjectionDetect(double distanceToDetectEjection)
        {
            Dictionary<T, C> precedentsWithoutEjection = new Dictionary<T, C>(Precedents);
            foreach(var t in Precedents)
            {
                foreach(var p in Precedents)
                {
                    if (t.Key.Distance(p.Key) < distanceToDetectEjection)
                    {
                        if (!t.Value.Equals(p.Value))
                        {
                            precedentsWithoutEjection.Remove(p.Key);
                        }
                    }
                }
            }
            return precedentsWithoutEjection;
        }

    }
}
