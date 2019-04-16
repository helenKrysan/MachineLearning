using MachineWPF.Tools;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2.ViewModels
{

    class PlotPractice2ViewModel : INotifyPropertyChanged
    {
        private Point _testPoint;
        private int _type;

        private int _bestK;
        public int BestK
        {
            get
            {
                return _bestK;
            }
            set
            {
                _bestK = value;
                BestParameter = value.ToString();
            }
        }

        private double _bestS;
        public double BestS
        {
            get
            {
                return _bestS;
            }
            set
            {
                _bestS = value;
                BestParameter = value.ToString();
            }
        }

        private double _bestH;
        public double BestH
        {
            get
            {
                return _bestH;
            }
            set
            {
                _bestH = value;
                BestParameter = value.ToString();
            }
        }

        private bool _isRandom;

        private TestModelPractice2 _testModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private int _testPointClass;
        public int TestPointClass
        {
            get { return _testPointClass; }
            set
            {
                _testPointClass = value;
                OnPropertyChanged();
            }
        }

        private string __bestParameter;
        public string BestParameter
        {
            get { return __bestParameter; }
            set
            {
                __bestParameter = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _changeTestPoint;
        public RelayCommand ChangeTestPoint
        {
            get
            {
                return _changeTestPoint ?? (_changeTestPoint = new RelayCommand(ChangeTestPointImpl));
            }
            set
            {
                _changeTestPoint = value;
            }
        }

        private string _testPointText;
        public string TestPointText
        {
            get
            {
                return _testPointText;
            }
            set
            {
                _testPointText = value;
                OnPropertyChanged();
            }
        }

        private void ChangeTestPointImpl(object obj)
        {
            string[] s = _testPointText.Split(',');
            _testPoint = new Point(double.Parse(s[0], CultureInfo.InvariantCulture), double.Parse(s[1], CultureInfo.InvariantCulture));
            PlotBuild();
        }

        private RelayCommand _generateRandom;
        public RelayCommand GenerateRandom
        {
            get
            {
                return _generateRandom ?? (_generateRandom = new RelayCommand(GenerateRandomImpl));
            }
            set
            {
                _generateRandom = value;
            }
        }

        private void GenerateRandomImpl(object obj)
        {
            _isRandom = true;
            _testModel = new TestModelPractice2(_isRandom);
            PlotBuild();
        }

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


        public PlotPractice2ViewModel(int type)
        {
            _type = type;
            _testPoint = new Point(7.3, 8.2);
            _testModel = new TestModelPractice2(_isRandom);
            PlotBuild();
        }

        private void PlotBuild()
        {
            var plot = new PlotModel();

            if (_type == 4 || _type == 5 || _type == 6)
            {

                //test data, form list (point,class)
                var learningPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                TestPointClass = -1;
                double maxD = 0;
                //sort data by distance
                //  Point testPoint = new Point(7.48, 8.24);

                if (_type == 4)
                {
                    //(point,distance)
                    BestK = 0;
                    int loo = 10000000;

                    for (int i = 1; i < _testModel.KNearestNeighbors.Precedents.Count; i++)
                    {
                        var missmatch = _testModel.KNearestNeighbors.LeaveOneOut((x, y) => _testModel.KNearestNeighbors.Evaluate(x, y, i), _testModel.KNearestNeighbors.Precedents);

                        Console.WriteLine("k = " + i);
                        Console.WriteLine("Missmatch = " + missmatch);
                        if (missmatch < loo)
                        {
                            loo = missmatch;
                            BestK = i;
                        }
                    }
                    TestPointClass = _testModel.KNearestNeighbors.Evaluate(_testPoint, _testModel.KNearestNeighbors.Precedents, _bestK);
                    foreach (var p in _testModel.KNearestNeighbors.Precedents)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 2));
                    }
                }

                if (_type == 5)
                {
                    //(point,distance)
                    BestH = 0;
                    int loo = 10000000;

                    for (double i = 10; i > 0; i = i - 0.2)
                    {
                        var missmatch = _testModel.ParzenWindow.LeaveOneOut((x, y) => _testModel.ParzenWindow.Evaluate(x, y, new Kernel('C'), i), _testModel.ParzenWindow.Precedents);

                        Console.WriteLine("h = " + i);
                        Console.WriteLine("Missmatch = " + missmatch);
                        if (missmatch < loo)
                        {
                            loo = missmatch;
                            BestH = Math.Round(i, 3);
                        }
                    }
                    TestPointClass = _testModel.ParzenWindow.Evaluate(_testPoint, _testModel.ParzenWindow.Precedents, new Kernel('C'), _bestH);
                    maxD = BestH;
                    foreach (var p in _testModel.ParzenWindow.Precedents)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 2));
                    }
                }

                if (_type == 6)
                {
                    BestH = 0.6;
                    int loo = 10000000;
                    BestS = 0;
                    var withoutEjection = _testModel.Standart.EjectionDetect(0.2);

                    for (double j = 1; j > 0; j = j - 0.05)
                    {
                        var standartPrecedentsLearn = _testModel.Standart.EtalonStandartization(withoutEjection,j);
                        var missmatch = _testModel.ParzenWindow.LeaveOneOut((x, y) => _testModel.ParzenWindow.Evaluate(x, y, new Kernel('C'), BestH), standartPrecedentsLearn);

                        Console.WriteLine("s = " + j);
                        Console.WriteLine("Missmatch = " + missmatch);
                        if (missmatch < loo)
                        {
                            loo = missmatch;
                            BestS = Math.Round(j, 3);

                        }

                    }
                    var standartPrecedents = _testModel.Standart.EtalonStandartization(withoutEjection,BestS);



                    foreach (var p in _testModel.Standart.Precedents)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 1.2));
                    }
                    foreach (var p in standartPrecedents)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 5));
                    }
                    foreach(var p in withoutEjection)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 3));
                    }
                    TestPointClass = _testModel.ParzenWindow.Evaluate(_testPoint, standartPrecedents, new Kernel('C'), BestH);
                    maxD = BestH;
                }

                plot.Axes.Add(new LinearAxis { AbsoluteMaximum = _testModel.MaxXY.Y, AbsoluteMinimum = _testModel.MinXY.Y });
                plot.Series.Add(learningPoints);
                var pointdraw = new ScatterSeries { MarkerType = MarkerType.Circle };
                pointdraw.Points.Add(new ScatterPoint(_testPoint.X, _testPoint.Y, 3));
                plot.Series.Add(pointdraw);

                for (int i = 0; i < (_testModel.PartsY + 1); i++)
                {
                    var rValue = _testModel.MinXY.Y + ((_testModel.MaxXY.Y - _testModel.MinXY.Y) / _testModel.PartsY) * i;
                    Func<double, double> yAxe = (x) =>
                    {
                        return rValue;
                    };
                    var xLineAxes = new FunctionSeries(yAxe, _testModel.MinXY.X, _testModel.MaxXY.X, 0.1);
                    xLineAxes.Color = OxyColors.Gray;
                    plot.Series.Add(xLineAxes);
                }

                for (int i = 0; i < (_testModel.PartsX + 1); i++)
                {
                    var rValue = _testModel.MinXY.X + ((_testModel.MaxXY.X - _testModel.MinXY.X) / _testModel.PartsX) * i;
                    Func<double, double> yAxe = (x) =>
                    {
                        return rValue;
                    };
                    Func<double, double> xAxe = (y) =>
                    {
                        return y;
                    };
                    var yLineAxes = new FunctionSeries(yAxe, xAxe, _testModel.MinXY.Y, _testModel.MaxXY.Y, 0.1);
                    yLineAxes.Color = OxyColors.Gray;
                    plot.Series.Add(yLineAxes);
                }

                Func<double, double> circleFP = (x) =>
                {
                    return Math.Sqrt(Math.Max(maxD * maxD - Math.Pow((_testPoint.X - x), 2), 0)) + _testPoint.Y;
                };
                Func<double, double> circleFN = (x) =>
                {
                    return -Math.Sqrt(Math.Max(maxD * maxD - Math.Pow((_testPoint.X - x), 2), 0)) + _testPoint.Y;
                };
                var circleP = new FunctionSeries(circleFP, _testPoint.X - maxD, _testPoint.X + maxD, 0.001);
                var circleN = new FunctionSeries(circleFN, _testPoint.X - maxD, _testPoint.X + maxD, 0.001);
                circleP.Color = OxyColors.Red;
                plot.Series.Add(circleP);
                circleN.Color = OxyColors.Red;
                plot.Series.Add(circleN);
            }

            this.Model = plot;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
