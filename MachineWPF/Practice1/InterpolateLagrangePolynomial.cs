using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice1
{
    class InterpolateLagrangePolynomial : AbstractRegression<double, double>
    {

        internal double Evaluate(double testObject, Dictionary<double, double> precedents)
        {
            double lagrangePolynomial = 0;
            foreach (var p1 in precedents)
            {
                double basicsPolynomial = 0;
                foreach (var p2 in precedents)
                {
                    if(p1.Key.Equals(p2.Key))
                    {
                        basicsPolynomial *= (testObject - p2.Key) / (p1.Key - p2.Key);
                    }
                }
                lagrangePolynomial += basicsPolynomial * p1.Value;
            }
            return lagrangePolynomial;
        }

    }
}
