using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class IntegerLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public int Value;
        public IntegerLiteralExpression()
        {

        }
        public IntegerLiteralExpression(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(INT:{Value})";
        }
        public override string ToCodeString()
        {
            return Value.ToString();
        }
    }
}
