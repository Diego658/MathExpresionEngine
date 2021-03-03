using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpresionEngine.Tests
{
    public class TestTokenizer
    {

        private Dictionary<string, List<Token>> basicExpresionAssertValues;
        private string basicExpresion;

        [SetUp]
        public void Setup()
        {
            basicExpresion = "";
            basicExpresionAssertValues = new Dictionary<string, List<Token>>
            {
                {
                    $"1{MathExpresionEngine.Constants.DecimalSign}0*1{MathExpresionEngine.Constants.DecimalSign}0", 
                    new List<Token>
                    {
                        Token.Number,
                        Token.Multiply,
                        Token.Number,
                        Token.EOF
                    }
                },
                {
                    $"(  ( Cantidad * Precio )  -  ( Desc_Aut + Desc_Manual )  )  * 0{MathExpresionEngine.Constants.DecimalSign}12",
                    new List<Token>
                    {
                        Token.OpenParens,
                        Token.OpenParens,
                        Token.Identifier,
                        Token.Multiply,
                        Token.Identifier,
                        Token.CloseParens,
                        Token.Subtract,
                        Token.OpenParens,
                        Token.Identifier,
                        Token.Add,
                        Token.Identifier,
                        Token.CloseParens,
                        Token.CloseParens,
                        Token.Multiply,
                        Token.Number
                    }
                },
                {
                    "2^5",
                    new List<Token>
                    {
                        Token.Number,
                        Token.Pow,
                        Token.Number
                    }
                },
                {
                    "(2+6)*8-(5*(6-9))",
                    new List<Token>
                    {
                        Token.OpenParens,
                        Token.Number,
                        Token.Add,
                        Token.Number,
                        Token.CloseParens,
                        Token.Multiply,
                        Token.Number,
                        Token.Subtract,
                        Token.OpenParens,
                        Token.Number,
                        Token.Multiply,
                        Token.OpenParens,
                        Token.Number,
                        Token.Subtract,
                        Token.Number,
                        Token.CloseParens,
                        Token.CloseParens,
                        Token.EOF
                    }
                },
                {
                    "2*-5",
                    new List<Token>
                    {
                        Token.Number,
                        Token.Multiply,
                        Token.Subtract,
                        Token.Number,
                        Token.EOF
                    }
                },
                {
                    "10+25+58+14- 15",
                    new  List<Token>
                    {
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Subtract,
                            Token.Number,
                            Token.EOF
                    }
                },
                {
                    "225+50+87-568-987*8",
                    new  List<Token>
                    {
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Subtract,
                            Token.Number,
                            Token.Subtract,
                            Token.Number,
                            Token.Multiply,
                            Token.Number,
                            Token.EOF
                    }
                },
                {
                    "225+50+87-568-987*8/9",
                    new  List<Token>
                    {
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Add,
                            Token.Number,
                            Token.Subtract,
                            Token.Number,
                            Token.Subtract,
                            Token.Number,
                            Token.Multiply,
                            Token.Number,
                            Token.Divide,
                            Token.Number,
                            Token.EOF
                    }
                }


            };





        }

        [Test]
        public void Test()
        {

            foreach (var expr in basicExpresionAssertValues)
            {
                ITokenizer tokenizer = new Tokenizer(expr.Key);

                for (int i = 0; i < expr.Value.Count; i++)
                {
                    tokenizer.Next();
                    var token = expr.Value[i];
                    Assert.AreEqual(token, tokenizer.Token);
                }
            }




        }

    }
}
