using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class BoolLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public bool Value;
        public BoolLiteralExpression()
        {

        }
        public BoolLiteralExpression(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(BOOL:{Value})";
        }
        public override string ToCodeString()
        {
            return Value.ToString();
        }
    }
}
