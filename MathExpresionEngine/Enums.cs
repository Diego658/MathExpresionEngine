using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpresionEngine
{
    
    public enum Token
    {
        EOF,
        Add,
        Subtract,
        Number,
        Multiply,
        Divide,
        Pow,
        SquareRoot,
        OpenParens,
        CloseParens,
        Identifier
    }
}
