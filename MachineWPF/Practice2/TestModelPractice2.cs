using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    class TestModelPractice2
    {
        public Point MaxXY { get; set; }
        public Point MinXY { get; set; }
        public int PartsX { get; set; }
        public int PartsY { get; set; }
        private double[] _testX;
        private double[] _testY;
        private const int _size = 100;

        private ParsenWindow<Point, int> _parzenWindow;
        private KNearestNeighbors<Point, int> _kNearestNeighbors;
        private Standart<Point, int> _standart;

        public ParsenWindow<Point, int> ParzenWindow { get { return _parzenWindow; } }
        public KNearestNeighbors<Point, int> KNearestNeighbors { get { return _kNearestNeighbors; } }
        public Standart<Point, int> Standart { get { return _standart; } }

        public TestModelPractice2(bool isRandom)  : this(isRandom, new Point(10, 10), new Point(5, 5), 5, 6)
        {
        }

        public TestModelPractice2(bool isRandom, Point maxXY, Point minXY) : this(isRandom, maxXY, minXY, 5, 6)
        {
        }

        public TestModelPractice2(bool isRandom, int partsX, int partsY) : this(isRandom, new Point(10, 10), new Point(5, 5), partsX, partsY)
        {
        }

        public TestModelPractice2(bool isRandom, Point maxXY, Point minXY, int partsX, int partsY)
        {
            MaxXY = new Point(maxXY);
            MinXY = new Point(minXY);
            PartsX = partsX;
            PartsY = partsY;
            if (!isRandom)
            {
                _testX = new double[_size] { 5.21, 5.38, 5.73, 5.67, 5.7, 9.95, 5.11, 5.41, 8.37, 5.14, 5.48, 8.32, 9.76, 9.97, 9.88, 5.69, 7.7, 6.42, 6.42, 9.7, 5.98, 7.6, 6.42, 7.42, 7.84, 8.79, 6.58, 9.94, 6.32, 8.25, 5.06, 9.24, 7.75, 8.85, 6.5, 7.22, 9.46, 6.62, 7.91, 9.53, 8.15, 7.6, 7.97, 9.96, 5.11, 6.87, 5.15, 6.42, 6.62, 9.79, 6.61, 8.52, 8.95, 5.68, 7.27, 9.95, 7.45, 5.73, 9.35, 5.6, 6.61, 6.36, 8.99, 6.84, 6.37, 5.41, 8.88, 5.11, 5.16, 8.11, 6.82, 9.24, 7.38, 8.61, 7.6, 6.65, 5.44, 5.23, 7.29, 7.42, 8.55, 8.02, 6.2, 6.58, 6.07, 6.93, 5.6, 9.5, 7.61, 6.93, 9.29, 5.48, 6.65, 8.19, 6.22, 8.02, 8.58, 9.5, 9.44, 7.75 };
                _testY = new double[_size] { 5.55, 5.9, 6, 9.15, 5.59, 8.38, 5.72, 6.29, 6.46, 7.21, 5.1, 9.42, 6.5, 5.05, 5.02, 6.61, 9.41, 6.6, 7.6, 5.12, 6.78, 6.99, 6.42, 6.71, 7.22, 8.2, 9.18, 9.39, 9.55, 8.49, 6.04, 5.37, 8.26, 5.72, 8.1, 7.64, 7.04, 7.5, 7.82, 5.14, 5.33, 8.91, 8.83, 6.07, 9.31, 6.51, 5.41, 9.47, 5.29, 7.59, 8.39, 8.25, 6.31, 8.72, 9.04, 7.66, 6.59, 6.18, 9.41, 8.66, 7.26, 8.07, 6.14, 7.64, 5.11, 6.62, 6.83, 8.9, 8.43, 8.52, 5.3, 8.24, 7.65, 5.55, 7.68, 5.94, 8.5, 8.21, 8.54, 9.43, 9.54, 9.48, 6.56, 7.86, 7.18, 5.36, 7.14, 5.2, 6.7, 5.37, 6.03, 8.35, 5.03, 8.87, 8.33, 5.65, 6.58, 6.54, 7.55, 9.75 };
                //     double[] testX = { 9.22, 8.98, 6.51, 8.57, 8.43, 8.08, 7.54, 9.28, 8.67, 7.81, 9.19, 8.14, 9.38, 9.05, 9.48, 5.59, 7.32, 6.37, 5.48, 5.42 };
                //     double[] testY = { 9.21, 6.16, 8.28, 5.84, 9.29, 7.37, 6.23, 7.76, 9.07, 8.02, 8.55, 6.75, 7.53, 7.64, 8.38, 6.34, 5.25, 6.54, 7.93, 8.82 };
                //     double[] testX = { 9.04, 5.64, 5.17, 7.77, 6.36, 7.69, 9.83, 8.51, 6.28, 5.23, 7.44, 9.29, 9.74, 9.48, 5.84, 9.76, 6.99, 5.64, 9.49, 6.02, 5.87, 5.13, 9.88, 6.99, 7.96, 7.71, 7.65, 6.16, 7.77, 9.21, 8.35, 5.05, 5.64, 5.1, 7.66, 5.78, 8.61, 8.07, 7.69, 9.72, 9.47, 9.05, 5.34, 6.28, 5.06, 5.59, 7.69, 7.38, 9.29, 5.17, 7.29, 8.69, 9.16, 7.32, 8.45, 6.36, 6.13, 8.1, 6.95, 9.61, 6.71, 7.91, 9.61, 8.72, 7.57, 9.24, 6.66, 9.23, 7.45, 9.37, 6.02, 9.35, 7.61, 7.69, 8.26, 6.8, 7.49, 6.84, 9.56, 8.84, 5.38, 6.99, 7.01, 7.74, 7.47, 5.12, 9.39, 8.06, 5.4, 5.01, 7.21, 5.6, 9.27, 9.63, 7.97, 7.37, 7.1, 9.77, 8.64, 5.05 };
            }
            else
            {
                Random random = new Random();
                _testX = new double[_size];
                _testY = new double[_size];
                for (int i = 0; i < _testX.Length; i++)
                {
                    _testX[i] = random.NextDouble() * (MaxXY.X - MinXY.X) + MinXY.X;
                }
                for (int i = 0; i < _testY.Length; i++)
                {
                    _testY[i] = random.NextDouble() * (MaxXY.Y - MinXY.Y) + MinXY.Y;
                }
            }
            _parzenWindow = new ParsenWindow<Point, int>();
            _kNearestNeighbors = new KNearestNeighbors<Point, int>();
            _standart = new Standart<Point, int>();
            Initialize(_parzenWindow.Precedents);
            Initialize(_kNearestNeighbors.Precedents);
            Initialize(_standart.Precedents);
        }

        private void Initialize(Dictionary<Point, int> precedents)
        {
            List<Point> dataN = new List<Point>();

            for (int i = 0; i < _size; i++)
            {
                dataN.Add(new Point(_testX[i], _testY[i]));
                int classX = 1;
                while (dataN[i].X > (MinXY.X + ((MaxXY.X - MinXY.X) / PartsX) * classX))
                {
                    classX++;
                }
                int classY = 1;
                while (dataN[i].Y > (MinXY.Y + ((MaxXY.Y - MinXY.Y) / PartsY) * classY))
                {
                    classY++;
                }
                int k = classX + PartsX * (classY - 1);
                precedents.Add(dataN[i], k);
            }
        }
    }
}
