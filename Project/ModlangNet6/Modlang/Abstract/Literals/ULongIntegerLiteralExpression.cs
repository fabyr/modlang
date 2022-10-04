using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class ULongIntegerLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public ulong Value;
        public ULongIntegerLiteralExpression() { }
        public ULongIntegerLiteralExpression(ulong value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(ULONG:{Value})";
        }

        public override string ToCodeString()
        {
            return Value.ToString();
        }
    }
}
