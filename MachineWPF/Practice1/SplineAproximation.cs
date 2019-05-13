using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice1
{
    class SplineAproximation : AbstractRegression<double,double>
    {
        internal double Evaluate(double testObject, Dictionary<double, double> precedents, int n)
        {
            double res = 0;
            for (int i = 0; i < precedents.Count-1; i++)
            {
                if (testObject >= precedents.Keys.ElementAt(i) && testObject <= precedents.Keys.ElementAt(i+1))
                {
                    res = (testObject - precedents.Keys.ElementAt(i)) * (precedents.Values.ElementAt(i+1) - precedents.Values.ElementAt(i)) / (precedents.Keys.ElementAt(i+1) - precedents.Keys.ElementAt(i)) + precedents.Values.ElementAt(i);

                }
            }

            return res;
        }

    }
}
