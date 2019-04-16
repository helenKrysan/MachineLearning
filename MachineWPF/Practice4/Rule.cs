using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    class Rule
    {
        private double _startPoint;
        private double _endPoint;
        private char _identifier;
        private int _class;

        public Rule(double startPoint, double endPoint, char identifier, int _class)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _identifier = identifier;
            this._class = _class;
        }

        public double StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        public double EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        public char Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public int Class
        {
            get { return _class;  }
            set { _class = value; }
        }

        public bool CheckRule(double x)
        {
            return (_startPoint <= x) && (x < _endPoint);
        }
    }
}
