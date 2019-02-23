using System.Collections.Generic;
using System.Linq;

namespace MachineWPF.Practice2
{
    class KNearestNeighbors<T, C> : AbstractSingleton<KNearestNeighbors<T,C>> where T : IMetric<T>
    {

        internal C Evaluate(T testObject, Dictionary<T, C> precedents, int k)
        {
            // sort objects by distance to testObject
            Dictionary<T, double> allObjectDistance = new Dictionary<T, double>();
            foreach (var obj in precedents.Keys)
            {
                allObjectDistance.Add(obj, testObject.Distance(obj));
            }

            var sortedDistance = allObjectDistance.ToList();
            sortedDistance.Sort((d1, d2) => d1.Value.CompareTo(d2.Value));

            //(class,count)

            Dictionary<C, int> nearestClassCount = new Dictionary<C, int>();

            for (int i = 0; i < k; i++)
            {
                if (!nearestClassCount.ContainsKey(precedents[sortedDistance[i].Key]))
                {
                    nearestClassCount.Add(precedents[sortedDistance[i].Key], 1);
                }
                else
                {
                    nearestClassCount[precedents[sortedDistance[i].Key]] += 1;
                }
            }

            //detect class

            C testPointClass = default(C);
            int count = 0;            

            foreach (var classCount in nearestClassCount)
            {
                if (count < classCount.Value)
                {
                    testPointClass = classCount.Key;
                    count = classCount.Value;
                }
            }
            return testPointClass;
        }
    }
}
