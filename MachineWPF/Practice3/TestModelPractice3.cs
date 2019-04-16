using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice3
{
    class TestModelPractice3
    {
        public double MaxX { get; set; }
        public double MinX { get; set; }
        private double[] _testX;
        private double[] _testY;
        private const int _size = 50;

        private NadarayaWatson<Real, Real> _nadarayaWatson;

        public NadarayaWatson<Real, Real> NadarayaWatson { get { return _nadarayaWatson; } }


        public TestModelPractice3(bool isRandom, double maxX, double minX)
        {
            MaxX = maxX;
            MinX = minX;
            if (!isRandom)
            {
                _testX = new double[_size];
                _testY = new double[_size];
                for (int i = 0; i < _size; i++)
                {
                    _testX[i] = 4 * (i / (_size - 1.0)) - 2;
                    _testY[i] = TestF(_testX[i]);
                }
            }
            else
            {
                Random random = new Random();
                _testX = new double[_size];
                _testY = new double[_size];
                for (int i = 0; i < _testX.Length; i++)
                {
                    _testX[i] = random.NextDouble() * (MaxX - MinX) + MinX;
                }
                for (int i = 0; i < _testY.Length; i++)
                {
                    _testY[i] = random.NextDouble() * (MaxX - MinX) + MinX;
                }
            }
            _nadarayaWatson = new NadarayaWatson<Real, Real>();
            Initialize(_nadarayaWatson.Precedents);
        }

        private void Initialize(Dictionary<Real, Real> precedents)
        {
            for (int i = 0; i < _size; i++)
            { 
                precedents.Add(_testX[i],_testY[i]);
            }
        }

        public Real TestF(Real x)
        {
            return (1 / (1 + 25 * x * x));
            //return Math.Sin(x);
        }

        public double TestF(double x)
        {
            return (1 / (1 + 25 * x * x));
            //return Math.Sin(x);
        }
    }
}
