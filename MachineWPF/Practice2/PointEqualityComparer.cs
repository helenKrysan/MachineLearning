using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice2
{
    class PointEqualityComparer :  IEqualityComparer<Point>
    {
        public bool Equals(Point a, Point b)
        {
            if ((a.X == b.X) && (a.Y == b.Y))
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(Point obj)
        {
            return Convert.ToInt32(Math.Pow(obj.X, obj.Y));
        }
    }
}
