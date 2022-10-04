using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class LongIntegerLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public long Value;
        public LongIntegerLiteralExpression()
        {

        }
        public LongIntegerLiteralExpression(long value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(LONG:{Value})";
        }
        public override string ToCodeString()
        {
            return Value.ToString();
        }
    }
}
