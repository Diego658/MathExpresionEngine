using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpresionEngine
{
    public interface INode
    {
        double Eval();
    }

    public class NodeNumber : INode
    {
        private double _number;

        public NodeNumber(double number)
        {
            _number = number;
        }

        public double Eval()
        {
            return _number;
        }
    }

    public class NodeBinary : INode
    {
        private readonly INode _leftNode;
        private readonly INode _rigthNode;
        private readonly Func<double, double, double> _func;

        public NodeBinary(INode leftNode, INode rigthNode, Func<double, double, double> func )
        {
            _leftNode = leftNode;
            _rigthNode = rigthNode;
            _func = func;
        }

        public double Eval()
        {
            return _func(_leftNode.Eval(), _rigthNode.Eval());
        }
    }

    public class NodeUnary : INode
    {
        private readonly INode _node;
        private readonly Func<double, double> _func;

        public NodeUnary(INode node, Func<double, double> func)
        {
            _node = node;
            _func = func;
        }

        public double Eval()
        {
            return _func(_node.Eval());
        }
    }


    public class NodeVariable:INode
    {
        private string _name;
        private readonly IContext _context;

        public NodeVariable(string name, IContext context)
        {
            _name = name;
            _context = context;
        }

        public double Eval()
        {
            return _context.Resolvevariable(_name);
        }
    }


}
