using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    abstract class AbstractInformationCriterion
    {
        private Dictionary<Point, int> _precedents = new Dictionary<Point, int>();

        private Dictionary<List<Rule>, int> _solvingList = new Dictionary<List<Rule>, int>();

        public Dictionary<Point, int> Precedents { get { return _precedents; } set { _precedents = value; } }

        public Dictionary<List<Rule>, int> SolvingList { get { return _solvingList; } set { _solvingList = value; } }

        private int _positive;

        public int Positive
        {
            get { return _positive; }
            set { _positive = value; }
        }

        private int _negative;

        public int Negative
        {
            get { return _negative; }
            set { _negative = value; }
        }

        private int _positiveTotal;

        public int PositiveTotal
        {
            get { return _positiveTotal; }
            set { _positiveTotal = value; }
        }

        private int _negativeTotal;

        public int NegativeTotal
        {
            get { return _negativeTotal; }
            set { _negativeTotal = value; }
        }


        public void SetPositiveNegative(List<Rule> rules, int _class)
        {
            Positive = 0;
            Negative = 0;
            foreach (var p in Precedents)
            {
                if (rules == null) break;
                var ruleRes = true;
                foreach (var r in rules)
                {
                    switch (r.Identifier)
                    {
                        case 'X':
                            {
                                ruleRes = ruleRes && r.CheckRule(p.Key.X);
                                break;
                            }
                        case 'Y':
                            {
                                ruleRes = ruleRes && r.CheckRule(p.Key.Y);
                                break;
                            }
                        default:
                            {
                                throw new InvalidOperationException();
                            }
                    }
                }
                if (ruleRes)
                {
                    if (p.Value == _class)
                    {
                        Positive++;
                    }
                    else
                    {
                        Negative++;
                    }

                }
            }
        }

        public void SetPositiveNegativeTotal(int _class)
        {
            NegativeTotal = 0;
            PositiveTotal = 0;
            foreach (var p in Precedents)
            {
                if (p.Value == _class)
                {
                    PositiveTotal++;
                }
                else
                {
                    NegativeTotal++;
                }
            }
        }

        abstract public double Information();
    }
}
