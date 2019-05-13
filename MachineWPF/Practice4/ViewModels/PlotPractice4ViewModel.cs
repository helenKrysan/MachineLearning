using OxyPlot;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineWPF.Practice4.ViewModels
{
    class PlotPractice4ViewModel : INotifyPropertyChanged
    {
        int _type;

        TestModelPractice4 _testModel;

        private PlotModel _model;
        public PlotModel Model
        {
            get { return _model; }
            private set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        public PlotPractice4ViewModel(int type)
        {
            _type = type;
            _testModel = new TestModelPractice4(true);
            PlotBuild();
        }

        private void PlotBuild()
        {
            _model = new PlotModel();
            if (_type == 9)
            {
                Console.WriteLine("Heuristic information for epsilon = 0: " + _testModel.HeuristicInformationCriterion.Information());
                Console.WriteLine("Fisher information for epsilon = 0: " + _testModel.FisherInformationCriterion.Information());
                Console.WriteLine("Entropy information for epsilon = 0: " + _testModel.EntropyInformationCriterion.Information());
                Console.WriteLine("------------------------------------------------------");

                _testModel.ChangeRule(_testModel.HeuristicInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 10);
                Console.WriteLine("Heuristic information for epsilon = +1/10: " + _testModel.HeuristicInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.FisherInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 10);
                Console.WriteLine("Fisher information for epsilon = +1/10: " + _testModel.FisherInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.EntropyInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 10);
                Console.WriteLine("Entropy information for epsilon = +1/10: " + _testModel.EntropyInformationCriterion.Information());
                Console.WriteLine("------------------------------------------------------");

                _testModel.ChangeRule(_testModel.HeuristicInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 5);
                Console.WriteLine("Heuristic information for epsilon = +1/5: " + _testModel.HeuristicInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.FisherInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 5);
                Console.WriteLine("Fisher information for epsilon = +1/5: " + _testModel.FisherInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.EntropyInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 5);
                Console.WriteLine("Entropy information for epsilon = +1/5: " + _testModel.EntropyInformationCriterion.Information());
                Console.WriteLine("------------------------------------------------------");

                LogicClassification logicClassification = new LogicClassification();

                List<Rule> allAllTerms = new List<Rule>();

                for (int i = 1; i <= 30; i++)
                {
                    Console.WriteLine("\nClass : " + i);
                    var sortedDistance = _testModel.FisherInformationCriterion.Precedents.OrderBy(key => key.Key.X);
                    _testModel.FisherInformationCriterion.Precedents = sortedDistance.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                    var listRulX = logicClassification.AreaMerge('X', i, _testModel.FisherInformationCriterion, 4, 0);

                    _testModel.FisherInformationCriterion.SetPositiveNegativeTotal(i);
                    foreach (var r in listRulX)
                    {
                        _testModel.FisherInformationCriterion.SetPositiveNegative(new List<Rule> { r } ,i);
                        Console.WriteLine("From: " + r.StartPoint + ", To: " + r.EndPoint + ", Info: " +  _testModel.FisherInformationCriterion.Information());
                    }

                    sortedDistance = _testModel.FisherInformationCriterion.Precedents.OrderBy(key => key.Key.Y);
                    _testModel.FisherInformationCriterion.Precedents = sortedDistance.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                    var listRulY = logicClassification.AreaMerge('Y', i, _testModel.FisherInformationCriterion, 4, 0);
                    Console.WriteLine("");
                    foreach (var r in listRulY)
                    {
                        _testModel.FisherInformationCriterion.SetPositiveNegative(new List<Rule> { r }, i);
                        Console.WriteLine("From: " + r.StartPoint + ", To: " + r.EndPoint + ", Info: " + _testModel.FisherInformationCriterion.Information());
                    }
                    var allTerms = listRulX.Concat(listRulY).ToList();

                    allAllTerms = allAllTerms.Concat(allTerms).ToList();

                    var conjRes = logicClassification.GradientSynthesisOfConjunctions(_testModel.FisherInformationCriterion, allTerms, 10, i, 2, 0);

                    Console.WriteLine("");

                    foreach (var r in conjRes)
                    {
                        Console.WriteLine("Identifier - " + r.Identifier + " From: " + r.StartPoint + ", To: " + r.EndPoint);
                    }

                    _testModel.FisherInformationCriterion.SolvingList.Add(conjRes,i);
                }

                var sortedInformation = _testModel.FisherInformationCriterion.SolvingList.ToList();
                sortedInformation.Sort((d1, d2) =>
                {
                    _testModel.FisherInformationCriterion.SetPositiveNegativeTotal(d1.Value);
                    _testModel.FisherInformationCriterion.SetPositiveNegative(d1.Key, d1.Value);
                    var infoFirst = _testModel.FisherInformationCriterion.Information();
                    _testModel.FisherInformationCriterion.SetPositiveNegativeTotal(d2.Value);
                    _testModel.FisherInformationCriterion.SetPositiveNegative(d2.Key, d2.Value);
                    var infoSecond = _testModel.FisherInformationCriterion.Information();
                    return (int)infoSecond - (int)infoFirst;
                });
                _testModel.FisherInformationCriterion.SolvingList = sortedInformation.ToDictionary(key => key.Key, element => element.Value);
                Console.WriteLine();
                foreach (var p in _testModel.FisherInformationCriterion.SolvingList)
                {
                    foreach (var r in p.Key)
                    {
                        Console.WriteLine("Identifier - " + r.Identifier + " From: " + r.StartPoint + ", To: " + r.EndPoint);
                    }
                    Console.WriteLine("Class: "+p.Value);
                    _testModel.FisherInformationCriterion.SetPositiveNegativeTotal(p.Value);
                    _testModel.FisherInformationCriterion.SetPositiveNegative(p.Key, p.Value);

                    Console.WriteLine("Info: " + _testModel.FisherInformationCriterion.Information());
                    Console.WriteLine();
                }
                Point testPoint = new Point(6.7, 5.6);
                Console.WriteLine(testPoint);
                Console.WriteLine(logicClassification.Evaluate(_testModel.FisherInformationCriterion.SolvingList, testPoint));

                Console.WriteLine(logicClassification.InductionOfDecisionTree(allAllTerms,_testModel.FisherInformationCriterion.Precedents));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
