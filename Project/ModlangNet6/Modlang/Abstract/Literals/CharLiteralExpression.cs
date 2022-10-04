using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class CharLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public char Value;
        public CharLiteralExpression()
        {

        }
        public CharLiteralExpression(char value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(CHAR:'{Util.TokenDisplayEscapeContent(Value.ToString())}')";
        }
        public override string ToCodeString()
        {
            return $"'{Value}'";
        }
    }
}
