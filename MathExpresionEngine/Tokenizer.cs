using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace MathExpresionEngine
{

    public interface ITokenizer
    {
        public string Expresion { get; }
        void Next();
        Token Token { get; }
        public double Number { get; }
        public string Identifier { get; }
    }

    public class Tokenizer : ITokenizer
    {

        public string Expresion { get; }
        public Token Token { get; private set; }
        public double Number { get; private set; }
        public string Identifier { get; private set; }
        private int position;

        private bool started;
        public Tokenizer(string expresion)
        {
            position = -1;
            Expresion = expresion;
        }

        public void Next()
        {
            position++;
            if (started && Token == Token.EOF)
            {
                throw new InvalidOperationException("It's already at the end.");
            }
            started = true;

            if (position >= Expresion.Length)
            {
                Token = Token.EOF;
                Number = 0;
                return;
            }
            var c = Expresion[position];
            if (char.IsWhiteSpace(c))
            {
                Next();
            }
            else if (char.IsNumber(c))
            {
                Token = Token.Number;
                ProcessNumber();
            }
            else if (char.IsLetter(c) || c == Constants.UnderscoreSign )
            {
                Token = Token.Identifier;
                ProccessIdentifier();

            }
            else if (char.IsSymbol(c) || char.IsPunctuation(c))
            {
                if (c == Constants.PlusSign)
                {
                    Token = Token.Add;

                }
                else if (c == Constants.SubtractSign)
                {
                    Token = Token.Subtract;
                }
                else if (c == Constants.MultiplySign)
                {
                    Token = Token.Multiply;
                }
                else if (c == Constants.DivideSign)
                {
                    Token = Token.Divide;
                }
                else if (c == Constants.PowSign)
                {
                    Token = Token.Pow;
                }
                else if (c == Constants.SquareRootSign)
                {
                    Token = Token.Pow;
                }
                else if (c == Constants.OpenParensSign)
                {
                    Token = Token.OpenParens;
                }
                else if (c == Constants.CloseParensSign)
                {
                    Token = Token.CloseParens;
                }
                else
                {
                    throw new InvalidDataException($"Token \"{c}\" is not recognized. ({position})");
                }
            }
            else
            {
                throw new InvalidDataException($"Token \"{c}\" is not recognized. ({position})");
            }
        }


        private void ProcessNumber()
        {
            var sb = new StringBuilder();
            var c = Expresion[position];
            //bool canAcceptSimbol = true;
            bool containsDecimal = false;
            while (true)
            {
                if (position >= Expresion.Length)
                {
                    break;
                }
                c = Expresion[position];
                if (char.IsNumber(c))
                {
                    sb.Append(c);
                    //canAcceptSimbol = false;
                }
                else if (c == Constants.DecimalSign)
                {
                    if (!containsDecimal)
                    {
                        sb.Append(c);
                        containsDecimal = true;
                    }
                    else
                    {
                        throw new InvalidDataException($"Unspected token  \"{c}\". ({position})");
                    }
                }
                else
                {
                    break;
                }
                position++;
            }
            position--;
            Number = double.Parse(sb.ToString());
        }


        private void ProccessIdentifier()
        {
            var sb = new StringBuilder();
            var c = Expresion[position];
            while (true)
            {
                if (position >= Expresion.Length)
                {
                    break;
                }
                c = Expresion[position];
                if (char.IsNumber(c))
                {
                    sb.Append(c);
                }
                else if (char.IsLetter(c))
                {
                    sb.Append(char.ToUpperInvariant(c));
                }
                else if (c == Constants.UnderscoreSign )
                {
                    sb.Append(c);
                }
                else if (char.IsWhiteSpace(c))
                {
                    break;
                }
                else
                {
                    break;
                }
                position++;
            }
            position--;
            Identifier = sb.ToString();
        }

    }
}