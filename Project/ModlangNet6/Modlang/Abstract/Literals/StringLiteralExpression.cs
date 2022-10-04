using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class StringLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public string Value;
        public StringLiteralExpression() { }
        public StringLiteralExpression(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(STR:\"{Util.TokenDisplayEscapeContent(Value)}\")";
        }
        public override string ToCodeString()
        {
            return $"\"{Value}\"";
        }
    }
}
