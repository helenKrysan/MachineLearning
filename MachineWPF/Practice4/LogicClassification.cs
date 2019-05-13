using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    class LogicClassification
    {

        public List<Rule> AreaMerge(char identefier, int _class, AbstractInformationCriterion creterion, int areaCount, double informationMin)
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

                        Rule union = new Rule(Minimum(firstArea.StartPoint, secondArea.StartPoint, enumerRule.Current.StartPoint), Maximum(firstArea.EndPoint, secondArea.EndPoint, enumerRule.Current.EndPoint), identefier, _class);
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

        public List<Rule> GradientSynthesisOfConjunctions(AbstractInformationCriterion cretrion, List<Rule> allTerms, int t, int _class, int stepsToStop, double eps)
        {
            var currentConjuction = new List<Rule>();
            cretrion.SetPositiveNegativeTotal(_class);
            cretrion.SetPositiveNegative(currentConjuction, _class);
            var bestInformation = cretrion.Information();
            var bestT = 0;
            for (int i = 1; i <= t; i++)
            {
                var currentConjuctionNeighborhoods = CurrentConjuctionNeighborhoods(currentConjuction, allTerms);
                double bestCurrentInfo = 0;
                List<Rule> bestCurrentConjuction = new List<Rule>();
                foreach (var conjuction in currentConjuctionNeighborhoods)
                {
                    cretrion.SetPositiveNegative(conjuction, _class);
                    var currInfo = cretrion.Information();
                    if (currInfo > bestCurrentInfo)
                    {
                        bestCurrentInfo = currInfo;
                        bestCurrentConjuction = new List<Rule>(conjuction);
                    }
                }
                if (bestCurrentInfo > bestInformation)
                {
                    bestT = i;
                    bestInformation = bestCurrentInfo;
                    currentConjuction = new List<Rule>(bestCurrentConjuction);
                }

                if (bestT - i > stepsToStop) break;
            }
            return currentConjuction;
        }

        public List<List<Rule>> CurrentConjuctionNeighborhoods(List<Rule> conjuction, List<Rule> allTerms)
        {
            List<List<Rule>> neighborhoods = new List<List<Rule>>();
            //add
            foreach (var term in allTerms)
            {
                if (!conjuction.Contains(term))
                {
                    var newConjuction = new List<Rule>(conjuction);
                    newConjuction.Add(term);
                    if (!neighborhoods.Contains(newConjuction))
                    {
                        neighborhoods.Add(newConjuction);
                    }
                }
            }
            //delete
            if (conjuction.Capacity != 0)
            {
                foreach (var term in conjuction)
                {
                    var newConjuction = new List<Rule>(conjuction);
                    newConjuction.Remove(term);
                    if (!neighborhoods.Contains(newConjuction))
                    {
                        neighborhoods.Add(newConjuction);
                    }
                }
            }
            //change
            if (conjuction.Capacity != 0)
            {
                foreach (var term in conjuction)
                {
                    var newConjuction = new List<Rule>(conjuction);
                    newConjuction.Remove(term);
                    foreach (var termAll in allTerms)
                    {
                        if (!conjuction.Contains(term))
                        {
                            var newConjuctionAll = new List<Rule>(newConjuction);
                            newConjuction.Add(termAll);
                            if (!neighborhoods.Contains(newConjuctionAll))
                            {
                                neighborhoods.Add(newConjuctionAll);
                            }
                        }
                    }
                }
            }
            return neighborhoods;
        }

        public int Evaluate(Dictionary<List<Rule>, int> solvingList, Point testPoint)
        {
            foreach (var rules in solvingList)
            {
                bool ruleRes = true;
                foreach (var r in rules.Key)
                {
                    switch (r.Identifier)
                    {
                        case 'X':
                            {
                                ruleRes = ruleRes && r.CheckRule(testPoint.X);
                                break;
                            }
                        case 'Y':
                            {
                                ruleRes = ruleRes && r.CheckRule(testPoint.Y);
                                break;
                            }
                        default:
                            {
                                throw new InvalidOperationException();
                            }
                    }
                }
                if (ruleRes) return rules.Value;
            }
            return 0;
        }

        public BinaryTree InductionOfDecisionTree(List<Rule> allTerms, Dictionary<Point, int> precedents)
        {
            var creterion = new FisherInformationCriterion();
            creterion.Precedents = precedents;
            bool isAll = true;
            int lastClass = 0;
            foreach (var p in precedents)
            {
                if ((lastClass != 0) && (p.Value != lastClass))
                {
                    isAll = false;
                }
                lastClass = p.Value;
            }
            if (isAll)
            {
                return new BinaryTree(lastClass);
            }
            double bestInfo = 0;
            Rule bestRule = null;
            foreach (var term in allTerms)
            {
                creterion.SetPositiveNegativeTotal(term.Class);
                creterion.SetPositiveNegative(new List<Rule> { term }, term.Class);
                var info = creterion.Information();
                if(info > bestInfo)
                {
                    bestRule = term;
                    bestInfo = info;
                }
            }
            Dictionary<Point, int> truePart = new Dictionary<Point, int>();
            Dictionary<Point, int> falsePart = new Dictionary<Point, int>();

            foreach (var p in precedents)
            {
                var prop = p.Key.GetType().GetProperty(bestRule.Identifier.ToString()).GetValue(p.Key, null);

                if (bestRule.CheckRule((double)prop))
                {
                    truePart.Add(p.Key, p.Value);
                }
                else
                {
                    falsePart.Add(p.Key, p.Value);
                }
            }

            if(falsePart.Count == 0)
            {
                Dictionary<int, int> classCount = new Dictionary<int, int>();
                foreach(var p in truePart)
                {
                    if (classCount.ContainsKey(p.Value))
                    {
                        classCount[p.Value]++;
                    }
                    else
                    {
                        classCount.Add(p.Value, 1);
                    }
                }
                int bestClass = 0;
                int bestClassCount = 0;
                foreach(var c in classCount)
                {
                    if(c.Value > bestClassCount)
                    {
                        bestClassCount = c.Value;
                        bestClass = c.Key;
                    }
                }
                return new BinaryTree(bestClass);
            }
            else if(truePart.Count == 0)
            {
                Dictionary<int, int> classCount = new Dictionary<int, int>();
                foreach (var p in falsePart)
                {
                    if (classCount.ContainsKey(p.Value))
                    {
                        classCount[p.Value]++;
                    }
                    else
                    {
                        classCount.Add(p.Value, 1);
                    }
                }
                int bestClass = 0;
                int bestClassCount = 0;
                foreach (var c in classCount)
                {
                    if (c.Value > bestClassCount)
                    {
                        bestClassCount = c.Value;
                        bestClass = c.Key;
                    }
                }
                return new BinaryTree(bestClass);
            }
            BinaryTree bt = new BinaryTree(bestRule);
            allTerms.Remove(bestRule);
            bt._leftSon = InductionOfDecisionTree(allTerms, truePart);
            bt._leftSon = InductionOfDecisionTree(allTerms, falsePart);
            return bt;
        }
    }
}
