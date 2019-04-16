using MachineWPF.Practice2;
using System;
using System.Collections.Generic;

namespace MachineWPF.Practice3
{
    class NadarayaWatson<T, C> : AbstractNonParametricRegression<T,C> where T : IMetric<T>
    {
        internal C Evaluate(T testObject, Dictionary<T, C> precedents, Kernel kernel, double h, Dictionary<T, double> gammas)
        {
            dynamic result = default(C);
            double kernelSum = 0;
            double kernelYSum = 0;
            foreach(var p in precedents)
            {
                kernelSum += kernel.KernelExec(Math.Abs(testObject.Distance(p.Key)) / h) * gammas[p.Key];
                kernelYSum += kernel.KernelExec(Math.Abs(testObject.Distance(p.Key)) / h) * Convert.ToDouble(p.Value) * gammas[p.Key];
            }
            result = kernelYSum / kernelSum;
            return result;
        }
    }
}
