using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MathExpresionEngine
{

    public interface IContext
    {
        public double Resolvevariable(string name);
        public void AddVariable(string name, double valor);
    }

    public class Context : IContext
    {
        private Dictionary<string, double> _variablesDictionary;

        public Context()
        {
            _variablesDictionary = new Dictionary<string, double>();
            LoadDefaultLiterals();
        }

        private void LoadDefaultLiterals()
        {
            _variablesDictionary.Add("PI", Math.PI);
        }


        public void AddVariable(string name, double value)
        {
            _variablesDictionary.Add(name.ToUpperInvariant(), value);
        }

        public double Resolvevariable(string name)
        {
            if (_variablesDictionary.ContainsKey(name))
            {
                return _variablesDictionary[name];
            }
            else
            {
                throw new InvalidOperationException($"Variable '{name}' not recognized!!!");
            }
        }
    }



    public interface IParser
    {
        public INode GetExpresionTree();
        public IContext Context { get; set; }
    }

    public class Parser : IParser
    {
        private Tokenizer _tokenizer;

        public IContext Context { get; set; }

        public Parser(string expresion, IContext context = default(IContext) )
        {
            _tokenizer = new Tokenizer(expresion);
            Context = context ?? new Context();
        }


        public INode GetExpresionTree()
        {
            var node = ParseAddAndSubstract();
            return node;
        }


        INode ParseUnary()
        {
            _tokenizer.Next();
            if (_tokenizer.Token == Token.Add)
            {
                return ParseUnary();
            }
            if (_tokenizer.Token == Token.Subtract)
            {
                var rhs = ParseUnary();
                return new NodeUnary(rhs, (a) => -a);
            }
            return ParseLeaf();
        }



        private INode ParsePowAndRootSquare()
        {
            var leftOperationNode = ParseMultiplyAndDivide();
            while (true)
            {
                //_tokenizer.Next();
                Func<double, double, double> op = null;
                if (_tokenizer.Token == Token.Pow)
                {
                    op = (a, b) => Math.Pow( a , b);
                }
                else if (_tokenizer.Token == Token.SquareRoot)
                {
                    throw new NotImplementedException();
                }

                if (op == null)
                {
                    return leftOperationNode;
                }


                var rigthOperationNode = ParseMultiplyAndDivide();

                leftOperationNode = new NodeBinary(leftOperationNode, rigthOperationNode, op);
            }

        }


        private INode ParseMultiplyAndDivide()
        {
            var leftOperationNode = ParseUnary();
            while (true)
            {
                _tokenizer.Next();
                Func<double, double, double> op = null;
                if (_tokenizer.Token == Token.Multiply)
                {
                    op = (a, b) => a * b;
                }
                else if (_tokenizer.Token == Token.Divide)
                {
                    op = (a, b) => a / b;
                }

                if (op == null)
                {
                    return leftOperationNode;
                }


                var rigthOperationNode = ParseUnary();

                leftOperationNode = new NodeBinary(leftOperationNode, rigthOperationNode, op);
            }

        }

        private INode ParseAddAndSubstract()
        {
            var leftOperationNode = ParsePowAndRootSquare();

            while (true)
            {


                Func<double, double, double> op = null;
                if (_tokenizer.Token == Token.Add)
                {
                    op = (a, b) => a + b;
                }
                else if (_tokenizer.Token == Token.Subtract)
                {
                    op = (a, b) => a - b;
                }


                if (op == null)
                {
                    return leftOperationNode;
                }


                var rigthOperationNode = ParsePowAndRootSquare();

                leftOperationNode = new NodeBinary(leftOperationNode, rigthOperationNode, op);
            }
        }




        private INode ParseLeaf()
        {
            //_tokenizer.Next();
            if (_tokenizer.Token == Token.Number)
            {
                return new NodeNumber(_tokenizer.Number);
            }
            else if (_tokenizer.Token == Token.OpenParens)
            {
                var node = ParseAddAndSubstract();
                if (_tokenizer.Token != Token.CloseParens)
                {
                    throw new InvalidDataException($"Missing close parenthesis");
                }
                return node;
            }
            else if(_tokenizer.Token == Token.Identifier)
            {
                var node = new NodeVariable(_tokenizer.Identifier, Context);
                return node;
            }
            throw new InvalidDataException($"Unexpect token: {_tokenizer.Token}");
        }


    }
}
