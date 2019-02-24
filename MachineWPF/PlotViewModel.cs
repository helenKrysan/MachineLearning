using System;
using System.Collections.Generic;

namespace MachineWPF
{
    using MachineWPF.Practice2;
    using MathNet.Numerics.LinearAlgebra.Double;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    class PlotViewModel
    {
        private TestModelPractice2 _testModel;

        //trash
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
            _testModel = new TestModelPractice2(isRandom);
            PlotBuild(type);
        }

        public void ButtonClick(int type)
        {
            _testModel = new TestModelPractice2(isRandom);
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

                //test data, form list (point,class)
                var learningPoints = new ScatterSeries { MarkerType = MarkerType.Circle };
                var testPointClass = -1;
                //sort data by distance
                //  Point testPoint = new Point(7.48, 8.24);

                if (type == 4)
                {
                    //(point,distance)
                    bestK = 0;
                    int loo = 10000000;

                    for (int i = 1; i < _testModel.KNearestNeighbors.Precedents.Count; i++)
                    {
                        var missmatch = _testModel.KNearestNeighbors.LeaveOneOut((x, y) => _testModel.KNearestNeighbors.Evaluate(x, y, i));
                       
                        Console.WriteLine("k = " + i);
                        Console.WriteLine("Missmatch = " + missmatch);
                        if (missmatch < loo)
                        {
                            loo = missmatch;
                            bestK = i;
                        }
                    }
                    testPointClass = _testModel.KNearestNeighbors.Evaluate(testPoint, _testModel.KNearestNeighbors.Precedents, bestK);
                    foreach (var p in _testModel.KNearestNeighbors.Precedents)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 2));
                    }
                }

                if (type == 5)
                {
                    //(point,distance)
                    bestH = 0;
                    int loo = 10000000;

                    for (double i = 10; i > 0; i = i - 0.2)
                    {
                        var missmatch = _testModel.ParzenWindow.LeaveOneOut((x, y) => _testModel.ParzenWindow.Evaluate(x, y,new Kernel('C'), i));
 
                        Console.WriteLine("h = " + i);
                        Console.WriteLine("Missmatch = " + missmatch);
                        if (missmatch < loo)
                        {
                            loo = missmatch;
                            bestH = i;
                        }
                    }
                    testPointClass = _testModel.ParzenWindow.Evaluate(testPoint, _testModel.ParzenWindow.Precedents, new Kernel('C'), bestH);
                    maxD = bestH;
                    foreach (var p in _testModel.ParzenWindow.Precedents)
                    {
                        learningPoints.Points.Add(new ScatterPoint(p.Key.X, p.Key.Y, 2));
                    }
                }

                tmp.Axes.Add(new LinearAxis { AbsoluteMaximum = _testModel.MaxXY.Y, AbsoluteMinimum = _testModel.MinXY.Y });
                tmp.Series.Add(learningPoints);
                var pointdraw = new ScatterSeries { MarkerType = MarkerType.Circle };
                pointdraw.Points.Add(new ScatterPoint(testPoint.X, testPoint.Y, 3));
                tmp.Series.Add(pointdraw);

                for (int i = 0; i < (_testModel.PartsY + 1); i++)
                {
                    var rValue = _testModel.MinXY.Y + ((_testModel.MaxXY.Y - _testModel.MinXY.Y) / _testModel.PartsY) * i;
                    Func<double, double> yAxe = (x) =>
                    {
                        return rValue;
                    };
                    var xLineAxes = new FunctionSeries(yAxe, _testModel.MinXY.X, _testModel.MaxXY.X, 0.1);
                    xLineAxes.Color = OxyColors.Gray;
                    tmp.Series.Add(xLineAxes);
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
            //return (1 / (1 + 25 * x * x));
            return Math.Sin(x);
        }
    }
}
