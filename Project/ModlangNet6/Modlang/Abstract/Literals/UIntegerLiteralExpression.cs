using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class UIntegerLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public uint Value;
        public UIntegerLiteralExpression() { }
        public UIntegerLiteralExpression(uint value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(UINT:{Value})";
        }
        public override string ToCodeString()
        {
            return Value.ToString();
        }
    }
}
