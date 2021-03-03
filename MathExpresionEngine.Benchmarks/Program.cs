using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Flee.PublicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathExpresionEngine.Benchmarks
{

    public class MathExpresionEngineBench
    {
        private Dictionary<string, double> _expresionsToTest;
        private List<IGenericExpression<double>> _compiledFlee;
        private List<INode> _pseudoCompiledExpressionContext;

        public MathExpresionEngineBench()
        {
            _expresionsToTest = new Dictionary<string, double>
            {
                { "1.0*1.0", 1d},
                {"2^5", 32d },
                {"2^(5*(3-2))", 32d },
                {"2^(5*3)", 32768d },
                {"(2+6)*8-(5*(6-9))", 79d },
                {"(2+6)*8", 64d },
                {"8*5", 40d },
                {"25+35-28+40-80+101+25+36+47+25", 226d },
                {"225+50+87-568-987", -1193d },
                {"225+50+87-568-987*8", -8102d }
            };

            var context = new Flee.PublicTypes.ExpressionContext();

            _compiledFlee = _expresionsToTest.Select(expr => context.CompileGeneric<double>(expr.Key)).ToList();


            var contex2 = new MathExpresionEngine.Context();

            _pseudoCompiledExpressionContext = _expresionsToTest.Select(expr =>
            {
                var parser = new MathExpresionEngine.Parser(expr.Key, contex2);
                var exprTree = parser.GetExpresionTree();
                return exprTree;
            }).ToList();

        }

        [Benchmark]
        public void MathEngine()
        {
            var contex = new MathExpresionEngine.Context();
            foreach (var expr in _expresionsToTest)
            {
                var parser = new MathExpresionEngine.Parser(expr.Key, contex);
                var result = parser.GetExpresionTree().Eval();
                if(result != expr.Value)
                {
                    throw new InvalidProgramException("Resultado fallido!!!");
                }
            }
        }


        [Benchmark]
        public void MathEnginePseudoCompiled()
        {

            foreach (var expr in _pseudoCompiledExpressionContext)
            {
                expr.Eval();
            }

            //var contex = new MathExpresionEngine.Context();
            //foreach (var expr in _expresionsToTest)
            //{
            //    var parser = new MathExpresionEngine.Parser(expr.Key, contex);
            //    var result = parser.GetExpresionTree().Eval();
            //    if (result != expr.Value)
            //    {
            //        throw new InvalidProgramException("Resultado fallido!!!");
            //    }
            //}
        }

        [Benchmark]
        public void FleeEngine()
        {
            //Flee.PublicTypes.ExpressionContext context = new Flee.PublicTypes.ExpressionContext();
            foreach (var expr in _compiledFlee)
            {
                var e = expr; //context.CompileGeneric<double>(expr.Key);// context.CompileDynamic(expr.Key);
                var result =  e.Evaluate();
                //if (result != expr.Value)
                //{
                //    throw new InvalidProgramException("Resultado fallido!!!");
                //}
            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {

            var summary = BenchmarkRunner.Run<MathExpresionEngineBench>();

        }
    }
}
