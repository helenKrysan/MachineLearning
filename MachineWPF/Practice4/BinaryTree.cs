using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF.Practice4
{
    class BinaryTree
    {

        public BinaryTree _leftSon;
        public BinaryTree _rightSon;

        private int _leaf;

        public int Leaf
        {
            get { return _leaf; }
            set { _leaf = value; }
        }

        private Rule _rule;

        public Rule Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public override string ToString()
        {
            if (_rule != null)
            {
                return " ( " + _leftSon?.ToString() + _rule.ToString() + _rightSon?.ToString() + " ) ";
            }
            return " ( " + _leftSon?.ToString() + _leaf + _rightSon?.ToString() + " ) ";
        }

        public BinaryTree(int leaf)
        {
            _leaf = leaf;
            _leftSon = null;
            _rightSon = null;
        }

        public BinaryTree(Rule rule)
        {
            _rule = rule;
            _leftSon = null;
            _rightSon = null;
        }
    }

}
