using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice1
{
    class LeastSquare
    {
        internal double Evaluate(double testObject, Dictionary<double, double> precedents, int n)
        {
            double[][] dd = new double[n][];
            for (int i = 0; i < n; i++)
            {
                dd[i] = new double[precedents.Count];
            }
            for (int h = 0; h < precedents.Count; h++)
            {
                for (int i = 0; i < n; i++)
                {
                    dd[i][h] = Math.Pow(precedents.Keys.ElementAt(h), i);
                }
            }
            // build matrices
            var X = DenseMatrix.OfColumnArrays(dd);
            var precedentsValue = new double[precedents.Count];
            for (int h = 0; h < precedents.Count; h++)
            {
                precedentsValue[h] = precedents.Values.ElementAt(h);
            }
            var y = new DenseVector(precedentsValue);

            // solve
            var p = X.QR().Solve(y);
            double res = 0;
            for (int i = 0; i < n; i++)
            {
                res += p[i] * Math.Pow(testObject, i);

            }
            return res;
        }
    }
}
