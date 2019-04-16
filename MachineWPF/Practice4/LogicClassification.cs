using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    class LogicClassification
    {

        public List<Rule> AreaMerge(char identefier, int _class, FisherInformationCriterion creterion, int areaCount, double informationMin)
        {
            List<double> threshold = new List<double>();
            var enumer = creterion.Precedents.GetEnumerator();
            enumer.MoveNext();
            var last = enumer.Current;
            while (enumer.MoveNext())
            {
                var lastprop = last.Key.GetType().GetProperty(identefier.ToString()).GetValue(last.Key, null);
                var currprop = enumer.Current.Key.GetType().GetProperty(identefier.ToString()).GetValue(enumer.Current.Key, null);
                if (lastprop != currprop)
                {
                    if (!((last.Value == _class) == (enumer.Current.Value == _class)))
                    {
                        threshold.Add(((double)lastprop + (double)currprop) / 2);
                    }
                }
                last = enumer.Current;
            }
            var resRules = new List<Rule>();
            if (threshold.Capacity != 0)
            {
                var enumerD = threshold.GetEnumerator();
                enumerD.MoveNext();
                var lastD = enumerD.Current;
                resRules.Add(ToRule(Int16.MinValue, lastD, identefier, _class));
                while (enumerD.MoveNext())
                {
                    resRules.Add(ToRule(lastD, enumerD.Current, identefier, _class));
                    lastD = enumerD.Current;
                }
                resRules.Add(ToRule(lastD, Int16.MaxValue, identefier, _class));
            }


            bool isMerge = true;
            creterion.SetPositiveNegativeTotal(_class);
            while (isMerge)
            {
                isMerge = false;

                if (resRules.Capacity > 2)
                {
                   
                    var enumerRule = resRules.GetEnumerator();
                    enumerRule.MoveNext();
                    var firstArea = enumerRule.Current;
                    enumerRule.MoveNext();
                    var secondArea = enumerRule.Current;
                    double deltaMax = 0;
                    Rule firstBest = null;
                    Rule secondBest = null;
                    Rule thirdBest = null;
                    Rule unionBest = null;
                    while (enumerRule.MoveNext())
                    {
                        creterion.SetPositiveNegative(new List<Rule> { firstArea }, _class);
                        var firstinfo = creterion.Information();

                        creterion.SetPositiveNegative(new List<Rule> { secondArea }, _class);
                        var secondinfo = creterion.Information();

                        creterion.SetPositiveNegative(new List<Rule> { enumerRule.Current }, _class);
                        var thirdinfo = creterion.Information();

                        var maxoneinfo = Maximum(firstinfo, secondinfo, thirdinfo);

                        Rule union = new Rule(Minimum(firstArea.StartPoint, secondArea.StartPoint, enumerRule.Current.StartPoint), Maximum(firstArea.EndPoint, secondArea.EndPoint, enumerRule.Current.EndPoint), identefier,_class);
                        creterion.SetPositiveNegative(new List<Rule> { union }, _class);
                        var unionInfo = creterion.Information();

                        var deltaInfo = unionInfo - maxoneinfo;
                        if (deltaInfo > informationMin)
                        {
                            if (deltaInfo > deltaMax)
                            {
                                deltaMax = deltaInfo;
                                firstBest = firstArea;
                                secondBest = secondArea;
                                thirdBest = enumerRule.Current;
                                unionBest = union;
                            }
                            isMerge = true;    
                        }

                        firstArea = secondArea;
                        secondArea = enumerRule.Current;
                    }
                    if (isMerge)
                    {
                        resRules.Remove(firstBest);
                        resRules.Remove(secondBest);
                        resRules.Remove(thirdBest);
                        resRules.Add(unionBest);
                        var sortedRules = resRules.OrderBy(key => key.StartPoint);
                        resRules = sortedRules.ToList();
                    }
                }
            }
            return resRules;
        }

        private double Maximum(double first, double second, double third)
        {
            return first > second ? (first > third ? first : third) : (second > third ? second : third);
        }

        private double Minimum(double first, double second, double third)
        {
            return first < second ? (first < third ? first : third) : (second < third ? second : third);
        }

        public Rule ToRule(double from, double to, char identefier, int _class)
        {
            return new Rule(from, to, identefier, _class);
        }
    }
}
