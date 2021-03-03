using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpresionEngine.Tests
{

    
    public class TestParser
    {

        private class ExpresionWithLiterals
        {
            public string Expresion { get; set; }
            public Dictionary<string, double> Literals { get; set; }
            public double Result { get; set; }
        }

        private Dictionary<string, double> _expresionsToTest;
        private List<ExpresionWithLiterals> _expresionToTestWithLiterals;

        [SetUp]
        public void Setup()
        {
            _expresionsToTest = new Dictionary<string, double>
            {
                { $"1{MathExpresionEngine.Constants.DecimalSign}0*1{MathExpresionEngine.Constants.DecimalSign}0", 1d},
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

            _expresionToTestWithLiterals = new List<ExpresionWithLiterals>
            {
                new ExpresionWithLiterals
                {
                    Expresion = $"(  ( Cantidad * Precio )  -  ( Desc_Aut + Desc_Manual )  )  * 0{MathExpresionEngine.Constants.DecimalSign}12",
                    Literals = new Dictionary<string, double>
                    {
                        {"Cantidad", 1},
                        {"Precio", 1},
                        {"Desc_Aut", 0 },
                        {"Desc_Manual", 0 },
                    },
                    Result = 0.12d
                }
            };

        }

        [Test]
        public void Test()
        {
            var expresion = new NodeBinary(
                new NodeNumber(50),
                new NodeNumber(25),
                (n1,n2)=> n1 + n2
                );

            Assert.AreEqual(75d, expresion.Eval());


            foreach (var expr in _expresionsToTest)
            {
                var parser = new Parser(expr.Key );
                Assert.AreEqual(expr.Value, parser.GetExpresionTree().Eval());
            }

        }



        [Test]
        public void TestLiterals()
        {
            foreach (var exp in _expresionToTestWithLiterals)
            {
                var parser = new Parser(exp.Expresion);
                foreach (var variable in exp.Literals)
                {
                    parser.Context.AddVariable(variable.Key, variable.Value );
                }
                Assert.AreEqual(exp.Result, parser.GetExpresionTree().Eval());
            }
        }

    }
}
