using MachineWPF.Practice2;
using System;
using System.Collections.Generic;

namespace MachineWPF.Practice3
{
    abstract class AbstractNonParametricRegression<T, C> where T : IMetric<T>
    {
        private Dictionary<T, C> _precedents = new Dictionary<T, C>();

        public Dictionary<T, C> Precedents { get { return _precedents; } set { _precedents = value; } }

        internal double EmpericRisk(Dictionary<T, C> testResultValues, Func<T, C> function)
        {
            double res = 0;
            foreach (var p in testResultValues)
            {
                res += Math.Pow(Convert.ToDouble(p.Value) - Convert.ToDouble(function(p.Key)), 2);
            }
            return res;
        }

        public double LeaveOneOut(Func<T, Dictionary<T, C>, C> evaluate, Func<T, C> function)
        {
            Dictionary<T, C> resultPrecedents = new Dictionary<T, C>();
            foreach (var p in Precedents)
            {
                Dictionary<T, C> precedentsTestCopy = new Dictionary<T, C>(Precedents);
                if (precedentsTestCopy.ContainsKey(p.Key))
                    precedentsTestCopy.Remove(p.Key);
                var res = evaluate(p.Key, precedentsTestCopy);
                resultPrecedents.Add(p.Key, res);
            }
            return EmpericRisk(resultPrecedents, function);
        }

        public Dictionary<T, double> Lowess(Func<T, Dictionary<T, C>, Dictionary<T, double>, C> evaluate, Dictionary<T, double> gammas, Kernel kernel)
        {
            Dictionary<T, double> resultGammas = new Dictionary<T, double>();
            foreach (var p in Precedents)
            {
                Dictionary<T, C> precedentsTestCopy = new Dictionary<T, C>(Precedents);
                if (precedentsTestCopy.ContainsKey(p.Key))
                    precedentsTestCopy.Remove(p.Key);
                var res = evaluate(p.Key, precedentsTestCopy, gammas);
                double newGamma = kernel.KernelExec(Math.Abs(Convert.ToDouble(res) - Convert.ToDouble(p.Value)));
                resultGammas.Add(p.Key, newGamma);
            }

            return resultGammas;
        }

        public bool CheckGamma(Dictionary<T, double> gammas, Dictionary<T, double> newGammas, double eps)
        {
            var enumer = gammas.GetEnumerator();        
            while (enumer.MoveNext()) 
            {
                if(Math.Abs(newGammas[enumer.Current.Key] - enumer.Current.Value)>eps)
                {
                    return false;
                }
            } 
            return true;
        }

    }


}
