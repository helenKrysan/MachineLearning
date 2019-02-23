using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF
{
    public class Parameters
    {
        private double minX;
        public double MinX { get { return minX; } set { minX = value; } }
        private double maxX;
        public double MaxX { get { return maxX; } set { maxX = value; } }
        private double[] xValues;
        public double[] XValues { get { return xValues; } set { xValues = value; } }
        private double[] yValues;
        public double[] YValues { get { return yValues; } set { yValues = value; } }
        private int size;
        public int Size { get { return size; } set { size = value; } }
        private int lsn;
        public int Lsn { get { return lsn; } set { lsn = value; } }

        public Parameters()
        {
            minX = -2;
            maxX = 2;
            size = 25;
            lsn = 2;
            xValues = new double[size];
            yValues = new double[size];
            for (int i = 0; i < size; i++)
            {
                xValues[i] = 4 * (i / (size - 1.0)) - 2;
                yValues[i] = TestF(xValues[i]);
            }
        }

        public Parameters(double minX, double maxX, double[] xValues, int size)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.xValues = xValues;
            this.size = size;
        }

        public Parameters(double minX, double maxX, double[] xValues, double[] yValues)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.xValues = xValues;
            this.yValues = yValues;
        }

        private double TestF(double x)
        {
            return 1 / (1 + 25 * x * x);
        }

    }
}
