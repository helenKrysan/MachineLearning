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
using MachineWPF.Practice2;

namespace MachineWPF.Practice3.ViewModels
{

    class PlotPractice3ViewModel : INotifyPropertyChanged
    {
        private double[] _testPointXValues;
        int _type;
        private bool _isRandom;

        private TestModelPractice3 _testModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private int _testPointY;
        public int TestPointY
        {
            get { return _testPointY; }
            set
            {
                _testPointY = value;
                OnPropertyChanged();
            }
        }

        private string _empericRisk;
        public string EmpericRisk
        {
            get { return _empericRisk; }
            set
            {
                _empericRisk = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _changeTestPointX;
        public RelayCommand ChangeTestPointX
        {
            get
            {
                return _changeTestPointX ?? (_changeTestPointX = new RelayCommand(ChangeTestPointXImpl));
            }
            set
            {
                _changeTestPointX = value;
            }
        }

        private string _testPointXText;
        public string TestPointXText
        {
            get
            {
                return _testPointXText;
            }
            set
            {
                _testPointXText = value;
                OnPropertyChanged();
            }
        }

        private void ChangeTestPointXImpl(object obj)
        {
            _testPointXValues = new double[1];
            _testPointXValues[0] = double.Parse(_testPointXText, CultureInfo.InvariantCulture);
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


        public PlotPractice3ViewModel(int type)
        {
            _isRandom = false;
            _type = type;
            _testPointXValues = new double[size];
            testResultValues = new double[size];
            for (int i = 0; i < size; i++)
            {
                if (i != size - 1)
                {
                    _testPointXValues[i] = 4 * ((i + 0.5) / (size - 1)) - 2;
                }
            }
            _testModel = new TestModelPractice3(_isRandom,-2,2);
            PlotBuild();
        }

        private double[] testResultValues;
        int size = 50;

        private void PlotBuild()
        {
            var plot = new PlotModel();
            if (_type == 7)
            {
                plot.Title = "Nadaraya-Watson";
                double h = 1;
                double bestH = 0;
                double bestEmperic = Double.MaxValue;
                Dictionary<Real, double> gammas = new Dictionary<Real, double>();
                foreach (var p in _testModel.NadarayaWatson.Precedents)
                {
                    gammas.Add(p.Key, 1);
                }
                while (h > 0)
                {
                    var emperic = _testModel.NadarayaWatson.LeaveOneOut((x, y) => _testModel.NadarayaWatson.Evaluate(x,y,new Kernel('C'), h, gammas),_testModel.TestF);

                    Console.WriteLine("For h = " + h + " emperic risk is "  + emperic);
                    if (emperic < bestEmperic)
                    {
                        bestEmperic = emperic;
                        bestH = h;
                    }
                    h -= 0.05;
                }
                for (int i = 0; i < size; i++)
                {
                    testResultValues[i] = _testModel.NadarayaWatson.Evaluate(_testPointXValues[i], _testModel.NadarayaWatson.Precedents, new Kernel('C'), bestH, gammas);
                }

                var seriesOriginal = new FunctionSeries(_testModel.TestF, -2, 2, 0.05);
   //           var seriesNadarayaResult = new FunctionSeries(NadarayaWatson, -2, 2, 0.0001);

                var seriesLearningPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                foreach (var p in _testModel.NadarayaWatson.Precedents)
                {
                    var data = new ScatterPoint(p.Key, p.Value, 2);
                    seriesLearningPoints.Points.Add(data);
                }

                var seriesNadarayaResultPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < size - 1; i++)
                {
                    seriesNadarayaResultPoints.Points.Add(new ScatterPoint(_testPointXValues[i], testResultValues[i], 2));
                }

                plot.Axes.Add(new LinearAxis { AbsoluteMaximum = 1, AbsoluteMinimum = -1 });
                plot.Series.Add(seriesOriginal);
                plot.Series.Add(seriesLearningPoints);
                plot.Series.Add(seriesNadarayaResultPoints);

            }
            if (_type == 8)
            {
                _testModel.NadarayaWatson.Precedents.Add(-1.25,InterpolateLagrangePolynomial(-1.25,_testModel.NadarayaWatson.Precedents));
                _testModel.NadarayaWatson.Precedents.Add(-1.75, InterpolateLagrangePolynomial(-1.75, _testModel.NadarayaWatson.Precedents));
                _testModel.NadarayaWatson.Precedents.Add(1.25, InterpolateLagrangePolynomial(1.25, _testModel.NadarayaWatson.Precedents));
                _testModel.NadarayaWatson.Precedents.Add(1.75, InterpolateLagrangePolynomial(1.75, _testModel.NadarayaWatson.Precedents));

                plot.Title = "Lowess";
                double eps = 0.1;
                Dictionary<Real, double> gammas = new Dictionary<Real, double>();
                Dictionary<Real, double> newGammas = new Dictionary<Real, double>();
                int ii = 1;
                foreach (var p in _testModel.NadarayaWatson.Precedents)
                {
                    gammas.Add(p.Key, 1);
                }
                foreach (var g in gammas)
                {
                    Console.WriteLine("Gamma for point " + g.Key + " -> " + g.Value);
                }
                Console.WriteLine("-------------------------------------------------------------------");

                newGammas = _testModel.NadarayaWatson.Lowess((x, y, z) => _testModel.NadarayaWatson.Evaluate(x, y, new Kernel('C'), 0.1, z), gammas, new Kernel('C'));
                foreach (var g in newGammas)
                {
                    Console.WriteLine("Gamma for point " + g.Key + " -> " + g.Value);
                }
                Console.WriteLine("-------------------------------------------------------------------");

                while (!_testModel.NadarayaWatson.CheckGamma(gammas,newGammas,eps))
                {
                    gammas = newGammas;
                    newGammas = _testModel.NadarayaWatson.Lowess((x, y, z) => _testModel.NadarayaWatson.Evaluate(x, y, new Kernel('C'), 0.5, z), gammas ,new Kernel('C'));
                    foreach (var g in newGammas)
                    {
                        Console.WriteLine("Gamma for point " + g.Key + " -> " + g.Value);
                    }
                    Console.WriteLine("-------------------------------------------------------------------");

                    ii++;
                }
                for (int i = 0; i < size; i++)
                {
                    testResultValues[i] = _testModel.NadarayaWatson.Evaluate(_testPointXValues[i], _testModel.NadarayaWatson.Precedents, new Kernel('C'), 0.1, newGammas);
                }

                var seriesOriginal = new FunctionSeries(_testModel.TestF, -2, 2, 0.001);
                //var seriesNadarayaResult = new FunctionSeries(NadarayaWatson, -2, 2, 0.0001);

                var seriesLearningPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                foreach (var p in _testModel.NadarayaWatson.Precedents)
                {
                    var data = new ScatterPoint(p.Key, p.Value, 2);
                    seriesLearningPoints.Points.Add(data);
                }

                var seriesNadarayaResultPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < size - 1; i++)
                {
                    seriesNadarayaResultPoints.Points.Add(new ScatterPoint(_testPointXValues[i], testResultValues[i], 2));
                }

                plot.Axes.Add(new LinearAxis { AbsoluteMaximum = 1, AbsoluteMinimum = -1 });
                plot.Series.Add(seriesOriginal);
                plot.Series.Add(seriesLearningPoints);
                plot.Series.Add(seriesNadarayaResultPoints);

            }
            Dictionary<Real, Real> resultValues = new Dictionary<Real, Real>();
            for(int i=0; i<size-1; i++)
            {
                resultValues.Add(_testPointXValues[i], testResultValues[i]);
            }
            EmpericRisk = _testModel.NadarayaWatson.EmpericRisk(resultValues,_testModel.TestF).ToString();
            this.Model = plot;
        }



        private double InterpolateLagrangePolynomial(double x, Dictionary<Real, Real> prec)
        {
            double lagrangePol = 0;
            foreach (var p in prec)
            {

                double basicsPol = 1;
                foreach (var p1 in prec)
                    if (!p1.Equals(p))
                    {
                        basicsPol *= (x - p.Key) / (p.Key - p1.Key);
                    }
                lagrangePol += basicsPol * p.Value;
            }
            return lagrangePol;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
