using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF
{
    using MachineWPF.Practice2;
    using MathNet.Numerics.LinearAlgebra.Double;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    class PlotViewModel
    {
        public static double eRisk = 0;
        public static double classX = 0;
        //       public double ERisk { get { return eRisk; } set { eRisk = value; } }
        private double minX = MainWindow.param.MinX;
        private double maxX = MainWindow.param.MaxX;
        private double[] xValues;
        private double[] yValues;
        private double[] testValues;
        private double[] testResultValues;
        private int size = MainWindow.param.Size;
        private double maxD;
        public static int bestK;
        public static double bestH;
        public static bool isRandom = false;
        public static Point testPoint = new Point(9.5, 7.9);

        public PlotViewModel(int type)
        {
            PlotBuild(type);
        }

        public void PlotBuild(int type)
        {
            // Create the plot model

            var tmp = new PlotModel();

            xValues = new double[size];
            yValues = new double[size];
            testValues = new double[size];
            testResultValues = new double[size];
            for (int i = 0; i < size; i++)
            {
                xValues[i] = 4 * (i / (size - 1.0)) - 2;
                yValues[i] = TestF(xValues[i]);
                if (i != size - 1)
                {
                    testValues[i] = 4 * ((i + 0.5) / (size - 1)) - 2;
                }
            }

            if (type == 1)
            {
                tmp.Title = "Lagrange";
                for (int i = 0; i < size; i++)
                {
                    testResultValues[i] = InterpolateLagrangePolynomial(testValues[i], xValues, yValues, size);

                }

                var seriesOriginal = new FunctionSeries(TestF, minX, maxX, 0.00001);

                var seriesLagrangePoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < size; i++)
                {
                    var data = new ScatterPoint(xValues[i], yValues[i], 2);
                    seriesLagrangePoints.Points.Add(data);
                }

                var seriesLagrangeRes = new FunctionSeries(LagrangePolynomial, minX, maxX, 0.0001);

                var seriesLagrangeResPoint = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < size - 1; i++)
                {
                    seriesLagrangeResPoint.Points.Add(new ScatterPoint(testValues[i], testResultValues[i], 2));
                }

                tmp.Axes.Add(new LinearAxis { AbsoluteMaximum = 1, AbsoluteMinimum = -1 });
                tmp.Series.Add(seriesOriginal);
                tmp.Series.Add(seriesLagrangePoints);
                tmp.Series.Add(seriesLagrangeRes);
                tmp.Series.Add(seriesLagrangeResPoint);

            }
            if (type == 2)
            {
                var seriesLSPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < size; i++)
                {
                    var data = new ScatterPoint(xValues[i], yValues[i], 2);
                    seriesLSPoints.Points.Add(data);
                }

                tmp.Title = "Least square";
                var nLS = MainWindow.param.Lsn + 1;
                double[][] dd = new double[nLS][];
                for (int i = 0; i < nLS; i++)
                {
                    dd[i] = new double[xValues.Length];
                }
                for (int h = 0; h < xValues.Length; h++)
                {
                    for (int j = 0; j < nLS; j++)
                    {
                        dd[j][h] = Math.Pow(xValues[h], j);
                    }
                }
                // build matrices
                var X = DenseMatrix.OfColumnArrays(dd);
                var y = new DenseVector(yValues);

                // solve
                var p = X.QR().Solve(y);



                Func<double, double> ls = (x) =>
                {
                    double res = 0;
                    for (int i = 0; i < nLS; i++)
                    {
                        res += p[i] * Math.Pow(x, i);

                    }
                    return res;
                };

                var seriesLSResPoint = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < size - 1; i++)
                {
                    testResultValues[i] = (ls(testValues[i]));
                    seriesLSResPoint.Points.Add(new ScatterPoint(testValues[i], testResultValues[i], 2));
                }

                var seriesLestSquares = new FunctionSeries(ls, minX, maxX, 0.00001);
                var seriesOriginal = new FunctionSeries(TestF, minX, maxX, 0.00001);

                tmp.Axes.Add(new LinearAxis { AbsoluteMaximum = 1, AbsoluteMinimum = -1 });
                tmp.Series.Add(seriesOriginal);
                tmp.Series.Add(seriesLSPoints);
                tmp.Series.Add(seriesLestSquares);
                tmp.Series.Add(seriesLSResPoint);
            }
            if (type == 3)
            {
                tmp.Title = "Spline aproximation";
                for (int i = 0; i < size; i++) testResultValues[i] = SplineAproximation(testValues[i], xValues, yValues, size);

                var seriesSpline = new LineSeries { Title = "seriesLearning", MarkerType = MarkerType.Circle };
                for (int i = 0; i < size - 1; i++)
                {
                    seriesSpline.Points.Add(new DataPoint(xValues[i], yValues[i]));
                }

                var seriesSplineRes = new LineSeries { Title = "seriesLagrangeResult", MarkerType = MarkerType.Circle };
                for (int i = 0; i < size - 1; i++)
                {
                    seriesSplineRes.Points.Add(new DataPoint(testValues[i], testResultValues[i]));
                }

                var seriesLagrange = new FunctionSeries(TestF, minX, maxX, 0.00001);

                tmp.Axes.Add(new LinearAxis { AbsoluteMaximum = 1, AbsoluteMinimum = -1 });
                tmp.Series.Add(seriesLagrange);
                tmp.Series.Add(seriesSpline);
                tmp.Series.Add(seriesSplineRes);


            }
            if (type == 4 || type == 5 || type == 6)
            {
                Random random = new Random();
                //test data, form list (point,class)
                double maxX = 10;
                double minX = 5;
                double maxY = 10;
                double minY = 5;
                int partsX = 5;
                int partsY = 6;
                double[] testX = { 8.21, 5.38, 6.73, 5.67, 8.7, 9.95, 8.11, 5.41, 8.37, 5.14, 5.48, 8.32, 9.76, 9.97, 9.88, 5.69, 7.7, 6.42, 6.42, 9.7, 5.98, 7.6, 6.42, 7.42, 7.84, 8.79, 6.58, 9.94, 6.32, 8.25, 5.06, 9.24, 7.75, 8.85, 6.5, 7.22, 9.46, 6.62, 7.91, 9.53, 8.15, 7.6, 7.97, 9.96, 5.11, 6.87, 5.15, 6.42, 6.62, 9.79, 6.61, 8.52, 8.95, 5.68, 7.27, 9.95, 7.45, 5.73, 9.35, 5.6, 6.61, 6.36, 8.99, 6.84, 6.37, 5.41, 8.88, 5.11, 5.16, 8.11, 6.82, 9.24, 7.38, 8.61, 7.6, 6.65, 5.44, 5.23, 7.29, 7.42, 8.55, 8.02, 6.2, 6.58, 6.07, 6.93, 5.6, 9.5, 7.61, 6.93, 9.29, 5.48, 6.65, 8.19, 6.22, 8.02, 8.58, 9.5, 9.44, 7.75 };
                double[] testY = { 6.55, 6.07, 7.82, 9.15, 5.59, 8.38, 7.72, 6.29, 6.46, 7.21, 5.1, 9.42, 6.5, 5.05, 5.02, 6.61, 9.41, 6.6, 7.6, 5.12, 6.78, 6.99, 6.42, 6.71, 7.22, 8.2, 9.18, 9.39, 9.55, 8.49, 6.04, 5.37, 8.26, 5.72, 8.1, 7.64, 7.04, 7.5, 7.82, 5.14, 5.33, 8.91, 8.83, 6.07, 9.31, 6.51, 5.41, 9.47, 5.29, 7.59, 8.39, 8.25, 6.31, 8.72, 9.04, 7.66, 6.59, 6.18, 9.41, 8.66, 7.26, 8.07, 6.14, 7.64, 5.11, 6.62, 6.83, 8.9, 8.43, 8.52, 5.3, 8.24, 7.65, 5.55, 7.68, 5.94, 8.5, 8.21, 8.54, 9.43, 9.54, 9.48, 6.56, 7.86, 7.18, 5.36, 7.14, 5.2, 6.7, 5.37, 6.03, 8.35, 5.03, 8.87, 8.33, 5.65, 6.58, 6.54, 7.55, 9.75 };

                //     double[] testX = { 9.22, 8.98, 6.51, 8.57, 8.43, 8.08, 7.54, 9.28, 8.67, 7.81, 9.19, 8.14, 9.38, 9.05, 9.48, 5.59, 7.32, 6.37, 5.48, 5.42 };
                //     double[] testY = { 9.21, 6.16, 8.28, 5.84, 9.29, 7.37, 6.23, 7.76, 9.07, 8.02, 8.55, 6.75, 7.53, 7.64, 8.38, 6.34, 5.25, 6.54, 7.93, 8.82 };
                //     double[] testX = { 9.04, 5.64, 5.17, 7.77, 6.36, 7.69, 9.83, 8.51, 6.28, 5.23, 7.44, 9.29, 9.74, 9.48, 5.84, 9.76, 6.99, 5.64, 9.49, 6.02, 5.87, 5.13, 9.88, 6.99, 7.96, 7.71, 7.65, 6.16, 7.77, 9.21, 8.35, 5.05, 5.64, 5.1, 7.66, 5.78, 8.61, 8.07, 7.69, 9.72, 9.47, 9.05, 5.34, 6.28, 5.06, 5.59, 7.69, 7.38, 9.29, 5.17, 7.29, 8.69, 9.16, 7.32, 8.45, 6.36, 6.13, 8.1, 6.95, 9.61, 6.71, 7.91, 9.61, 8.72, 7.57, 9.24, 6.66, 9.23, 7.45, 9.37, 6.02, 9.35, 7.61, 7.69, 8.26, 6.8, 7.49, 6.84, 9.56, 8.84, 5.38, 6.99, 7.01, 7.74, 7.47, 5.12, 9.39, 8.06, 5.4, 5.01, 7.21, 5.6, 9.27, 9.63, 7.97, 7.37, 7.1, 9.77, 8.64, 5.05 };
                if (isRandom)
                {
                    for (int i = 0; i < testX.Length; i++)
                    {
                        testX[i] = random.NextDouble() * (maxX - minX) + minX;
                    }
                    for (int i = 0; i < testY.Length; i++)
                    {
                        testY[i] = random.NextDouble() * (maxY - minY) + minY;
                    }
                }
                int sizeData = testX.Length;
                List<Point> dataN = new List<Point>();
                Dictionary<Point, int> precedents = new Dictionary<Point, int>();
                var testPointClass = -1;
                for (int i = 0; i < sizeData; i++)
                {
                    dataN.Add(new Point(testX[i], testY[i]));
                    int kX = 1;
                    while (dataN[i].X > (minX + ((maxX - minX) / partsX) * kX))
                    {
                        kX++;
                    }
                    int kY = 1;
                    while (dataN[i].Y > (minY + ((maxY - minY) / partsY) * kY))
                    {
                        kY++;
                    }
                    int k = kX + partsX * (kY - 1);
                    precedents.Add(dataN[i], k);
                }
                //sort data by distance
                //  Point testPoint = new Point(7.48, 8.24);

                if (type == 4)
                {
                    //(point,distance)
                    bestK = 0;
                    int loo = 10000000;

                    for (int i = 1; i < precedents.Count; i++)
                    {
                        int count = 0;


                        foreach (var t in precedents)
                        {
                            var precedentsT = new Dictionary<Point, int>(precedents);
                            precedentsT.Remove(t.Key);
                            var res = KNearestNeighbors<Point, int>.Instance.Evaluate(t.Key, precedentsT, i);
                            if (res != t.Value)
                            {
                                count++;
                            }
                        }
                        Console.WriteLine("k = " + i);
                        Console.WriteLine("Missmatch = " + count);
                        if (count < loo)
                        {
                            loo = count;
                            bestK = i;
                        }
                    }

                    testPointClass = KNearestNeighbors<Point, int>.Instance.Evaluate(testPoint, precedents, bestK);
                }

                if (type == 5)
                {
                    //(point,distance)
                    bestH = 0;
                    int loo = 10000000;

                    for (double i = 10; i > 0; i = i - 0.2)
                    {
                        int count = 0;

                        foreach (var t in precedents)
                        {
                            var precedentsT = new Dictionary<Point, int>(precedents);
                            precedentsT.Remove(t.Key);
                            var res = ParsenWindow<Point, int>.Instance.Evaluate(t.Key, precedentsT, new Core('C'), i);
                            if (res != t.Value)
                            {
                                count++;
                            }
                        }
                        Console.WriteLine("h = " + i);
                        Console.WriteLine("Missmatch = " + count);
                        if (count < loo)
                        {
                            loo = count;
                            bestH = i;
                        }
                    }
                    testPointClass = ParsenWindow<Point, int>.Instance.Evaluate(testPoint, precedents, new Core('C'), bestH);
                    maxD = bestH;
                }

                
                var learningPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                for (int i = 0; i < sizeData; i++)
                {
                    learningPoints.Points.Add(new ScatterPoint(testX[i], testY[i], 2));
                }

                tmp.Axes.Add(new LinearAxis { AbsoluteMaximum = maxY, AbsoluteMinimum = minY });
                tmp.Series.Add(learningPoints);
                var pointdraw = new ScatterSeries { MarkerType = MarkerType.Circle };
                pointdraw.Points.Add(new ScatterPoint(testPoint.X, testPoint.Y, 3));
                tmp.Series.Add(pointdraw);



                for (int i = 0; i < (partsY + 1); i++)
                {
                    var rValue = minY + ((maxY - minY) / partsY) * i;
                    Func<double, double> yAxe = (x) =>
                    {
                        return rValue;
                    };
                    var xLineAxes = new FunctionSeries(yAxe, minX, maxX, 0.1);
                    xLineAxes.Color = OxyColors.Gray;
                    tmp.Series.Add(xLineAxes);
                }

                for (int i = 0; i < (partsX + 1); i++)
                {
                    var rValue = minX + ((maxX - minX) / partsX) * i;
                    Func<double, double> yAxe = (x) =>
                    {
                        return rValue;
                    };
                    Func<double, double> xAxe = (y) =>
                    {
                        return y;
                    };
                    var yLineAxes = new FunctionSeries(yAxe, xAxe, minY, maxY, 0.1);
                    yLineAxes.Color = OxyColors.Gray;
                    tmp.Series.Add(yLineAxes);
                }

                Func<double, double> circleFP = (x) =>
                {
                    return Math.Sqrt(Math.Max(maxD * maxD - Math.Pow((testPoint.X - x), 2), 0)) + testPoint.Y;
                };
                Func<double, double> circleFN = (x) =>
                {
                    return -Math.Sqrt(Math.Max(maxD * maxD - Math.Pow((testPoint.X - x), 2), 0)) + testPoint.Y;
                };
                var circleP = new FunctionSeries(circleFP, testPoint.X - maxD, testPoint.X + maxD, 0.001);
                var circleN = new FunctionSeries(circleFN, testPoint.X - maxD, testPoint.X + maxD, 0.001);
                circleP.Color = OxyColors.Red;
                tmp.Series.Add(circleP);
                circleN.Color = OxyColors.Red;
                tmp.Series.Add(circleN);

                classX = testPointClass;


            }


            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            if (type == 4 || type == 5 || type == 6)
            {

            }
            else
            {
                eRisk = EmpericRisk();
            }
            this.Model = tmp;
        }

        /// <summary>
        /// Gets the plot model.
        /// </summary>
        public PlotModel Model { get; private set; }
        public double MaxD { get => maxD; set => maxD = value; }

        private double InterpolateLagrangePolynomial(double x, double[] xValues, double[] yValues, int size)
        {
            double lagrangePol = 0;
            for (int i = 0; i < size; i++)
            {

                double basicsPol = 1;
                for (int j = 0; j < size; j++)
                    if (j != i)
                    {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                lagrangePol += basicsPol * yValues[i];
            }
            return lagrangePol;
        }

        private double LagrangePolynomial(double x)
        {

            return InterpolateLagrangePolynomial(x, xValues, yValues, size);
        }

        static double SplineAproximation(double x, double[] xValues, double[] yValues, int size)
        {
            double ans = 0;
            for (int i = 0; i < size - 1; i++)
            {
                if (x >= xValues[i] && x <= xValues[i + 1])
                {
                    ans = (x - xValues[i]) * (yValues[i + 1] - yValues[i]) / (xValues[i + 1] - xValues[i]) + yValues[i];

                }
            }

            return ans;
        }

        private double EmpericRisk()
        {
            double res = 0;
            for (int i = 0; i < size - 1; i++)
            {
                res += Math.Pow(testResultValues[i] - TestF(testValues[i]), 2);
            }
            return res;
        }



        private double TestF(double x)
        {
            return (1 / (1 + 25 * x * x));
        }
    }
}
