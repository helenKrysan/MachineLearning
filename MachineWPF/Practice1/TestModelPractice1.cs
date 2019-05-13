using System;

namespace MachineWPF.Practice1
{
    class TestModelPractice1
    {
        public double MaxX { get; set; }
        public double MinX { get; set; }
        private int _size = 50;

        private InterpolateLagrangePolynomial _interpolateLagrangePolynomial;

        public InterpolateLagrangePolynomial InterpolateLagrangePolynomial { get { return _interpolateLagrangePolynomial; } }


        public TestModelPractice1(bool isRandom, double maxX, double minX)
        {
            MaxX = maxX;
            MinX = minX;
            _interpolateLagrangePolynomial = new InterpolateLagrangePolynomial();
            if (!isRandom)
            {
                for (int i = 0; i < _size; i++)
                {
                    double xValue = 4 * (i / (_size - 1.0)) - 2;
                    _interpolateLagrangePolynomial.Precedents.Add(xValue,TestF(xValue));
                }
            }
            else
            {
                Random random = new Random();
                for (int i = 0; i < _size; i++)
                {
                    double xValue = random.NextDouble() * (MaxX - MinX) + MinX;
                    _interpolateLagrangePolynomial.Precedents.Add(xValue, TestF(xValue));
                }
            }
        }

        public double TestF(double x)
        {
            return (1 / (1 + 25 * x * x));
            //return Math.Sin(x);
        }
    }
}
