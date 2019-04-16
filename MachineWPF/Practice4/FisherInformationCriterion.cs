using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MachineWPF.Practice4
{
    class FisherInformationCriterion : AbstractInformationCriterion
    {
        //  CP-p * CN-n
        //     CP+N - p+n

            //n!/(n-k)!*k!

        public double Information()
        {
            return -1*Math.Log(Combination(PositiveTotal,Positive)*Combination(NegativeTotal,Negative)/Combination(PositiveTotal+NegativeTotal,Positive+Negative));
        }

        private double Combination(int n, int k)
        {
            if(n-k > k)
            {
                return (FastFactorial(n - k + 1, n) / FastFactorial(1, k));
            }
            return (FastFactorial(k + 1, n) / FastFactorial(1, n-k));
        }

        private double FastFactorial(int from, int to)
        {
            double res = 1;
            for(int i = from; i<= to; i++)
            {
                res *= i;
            }
            if(res == 0)
            {
                return 1;
            }
            return res;
        }
    }

}
