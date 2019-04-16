using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    class Standart<T, C> : AbstractClassification<T, C> where T : IMetric<T> where C : IComparable
    {
        internal Dictionary<T,C> EtalonStandartization(Dictionary<T, C> precedents, double part)
        {
            Dictionary<C, List<T>> precedentsByClass = new Dictionary<C, List<T>>();
            foreach (var p in precedents)
            {
                if (!precedentsByClass.ContainsKey(p.Value))
                {
                    precedentsByClass.Add(p.Value, new List<T>());
                }
                precedentsByClass[p.Value].Add(p.Key);
            }
            Dictionary<T, C> standartPrecedents = new Dictionary<T, C>();
            foreach (var p in precedentsByClass)
            {
                var listOfStandart = Centroid(p.Value, part);
                foreach (var s in listOfStandart)
                {
                    standartPrecedents.Add(s, p.Key);
                }
            }
            return standartPrecedents;
        }

        private List<T> Centroid(List<T> precedents, double part)
        {
            Dictionary<T, double> distanceToAllObjects = new Dictionary<T, double>();
            foreach (var p in precedents)
            {
                List<T> precedentWithoutCurrentPoint = new List<T>(precedents);
                precedentWithoutCurrentPoint.Remove(p);
                double distance = 0;
                foreach (var p1 in precedentWithoutCurrentPoint)
                {
                    distance += p.Distance(p1);
                }
                distanceToAllObjects.Add(p, distance);
            }
            var sortedDistance = distanceToAllObjects.ToList();
            sortedDistance.Sort((d1, d2) => d1.Value.CompareTo(d2.Value));
            int k = Convert.ToInt32(Math.Floor(sortedDistance.Capacity * part));
            if (k == 0)
            {
                k = 1;
            }
            var resList = new List<T>();
            for (int i = 0; i < k; i++)
            {
                resList.Add(sortedDistance[i].Key);
            }
            return resList;
        }

    }
}
