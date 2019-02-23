using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    class ParsenWindow<T,C> : AbstractSingleton<ParsenWindow<T, C>> where T : IMetric<T>
    {

        internal C Evaluate(T testObject, Dictionary<T,C> precedents, Core core, double h)
        {
            Dictionary<C, double> allClassDistance = new Dictionary<C, double>();

            foreach (var obj in precedents.Keys)
            {
                double dist = testObject.Distance(obj);
                if (!allClassDistance.ContainsKey(precedents[obj]))
                {
                    allClassDistance.Add(precedents[obj], core.CoreExec(dist / h));
                }
                else
                {
                    allClassDistance[precedents[obj]] += core.CoreExec(dist / h);
                }
            }

            C objClass = default(C);

            double biggestWeight = 0;
            foreach (var classDistance in allClassDistance)
            {
                if (biggestWeight < classDistance.Value)
                {
                    objClass = classDistance.Key;
                    biggestWeight = classDistance.Value;
                }
            }

            return objClass;
        }
    }
}
