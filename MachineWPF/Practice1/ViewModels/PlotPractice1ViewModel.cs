using MachineWPF.Tools;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MachineWPF.Practice1.ViewModels
{
    class PlotPractice1ViewModel
    {
        private double[] _testPointXValues;
        int _type;
        private bool _isRandom;

        private TestModelPractice1 _testModel;

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


        public PlotPractice1ViewModel(int type)
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
            _testModel = new TestModelPractice1(_isRandom, -2, 2);
            PlotBuild();
        }

        private double[] testResultValues;
        int size = 50;

        private void PlotBuild()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
