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
            if(_type == 9)
            {
                Console.WriteLine("Heuristic information for epsilon = 0: " + _testModel.HeuristicInformationCriterion.Information());
                Console.WriteLine("Fisher information for epsilon = 0: " + _testModel.FisherInformationCriterion.Information());
                Console.WriteLine("Entropy information for epsilon = 0: " + _testModel.EntropyInformationCriterion.Information());              
                Console.WriteLine("------------------------------------------------------");

                _testModel.ChangeRule(_testModel.HeuristicInformationCriterion, 1,(((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX))/10);
                Console.WriteLine("Heuristic information for epsilon = +1/10: " + _testModel.HeuristicInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.FisherInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 10);
                Console.WriteLine("Fisher information for epsilon = +1/10: " + _testModel.FisherInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.EntropyInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 10);
                Console.WriteLine("Entropy information for epsilon = +1/10: " + _testModel.EntropyInformationCriterion.Information());
                Console.WriteLine("------------------------------------------------------");

                _testModel.ChangeRule(_testModel.HeuristicInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) /5);
                Console.WriteLine("Heuristic information for epsilon = +1/5: " + _testModel.HeuristicInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.FisherInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 5);
                Console.WriteLine("Fisher information for epsilon = +1/5: " + _testModel.FisherInformationCriterion.Information());
                _testModel.ChangeRule(_testModel.EntropyInformationCriterion, 1, (((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX)) / 5);
                Console.WriteLine("Entropy information for epsilon = +1/5: " + _testModel.EntropyInformationCriterion.Information());         
                Console.WriteLine("------------------------------------------------------");

                LogicClassification logicClassification = new LogicClassification();

 
                var sortedDistance = _testModel.FisherInformationCriterion.Precedents.OrderBy(key => key.Key.X);
                _testModel.FisherInformationCriterion.Precedents = sortedDistance.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
                for (int i = 1; i <= 30; i++) {
                    var listRul = logicClassification.AreaMerge('X', i, _testModel.FisherInformationCriterion, 4, 0);

                    foreach (var r in listRul)
                    {
                        Console.WriteLine("From: " + r.StartPoint + ", To: " + r.EndPoint);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();

                sortedDistance = _testModel.FisherInformationCriterion.Precedents.OrderBy(key => key.Key.Y);
                _testModel.FisherInformationCriterion.Precedents = sortedDistance.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
                for (int i = 1; i <= 30; i++)
                {
                    var listRul = logicClassification.AreaMerge('Y', i, _testModel.FisherInformationCriterion, 4, 0);

                    foreach (var r in listRul)
                    {
                        Console.WriteLine("From: " + r.StartPoint + ", To: " + r.EndPoint);
                    }

                    Console.WriteLine();
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
