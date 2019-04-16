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

                eRisk = EmpericRisk();
            
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
            //return Math.Sin(x);
        }
    }
}
