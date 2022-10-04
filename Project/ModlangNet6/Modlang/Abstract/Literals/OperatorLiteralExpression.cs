using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class OperatorLiteralExpression : Expression // Note: this is only used for operator overloading functions
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public string Operator;
        public OperatorLiteralExpression() { }
        public OperatorLiteralExpression(string @operator)
        {
            Operator = @operator;
        }

        public override string ToString()
        {
            return $"({Operator})";
        }
        public override string ToCodeString()
        {
            return Operator.ToString();
        }
    }
}
