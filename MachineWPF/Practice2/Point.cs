using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    class Point : IMetric<Point>
    {
        private double _x;
        private double _y;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Distance(Point p)
        {
            return Math.Sqrt(Math.Pow((this._x - p.X),2) + Math.Pow((this._y - p.Y), 2));
        }

        public Point(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public Point(Point p)
        {
            this._x = p.X;
            this._y = p.Y;
        }

    }
}