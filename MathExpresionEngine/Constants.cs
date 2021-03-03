using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MathExpresionEngine
{
    public static class Constants
    {
        static Constants()
        {
            DecimalSign = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        }
        public const char PlusSign = '+';
        public const char SubtractSign = '-';
        public static  char DecimalSign =  '.';
        public const char MultiplySign = '*';
        public const char DivideSign = '/';
        public const char PowSign = '^';
        public const char SquareRootSign = '√';
        public const char OpenParensSign = '(';
        public const char CloseParensSign = ')';
        public const char PISign = 'π';
        public const char UnderscoreSign = '_';
    }
}