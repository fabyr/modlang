using Modlang.Abstract.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class UnaryOperatorExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Inner;
        public OperatorLiteralExpression OpExp = null;
        public string Operator;
        public bool IsPrefix;

        public UnaryOperatorExpression() { }
        public UnaryOperatorExpression(Expression inner, string op, bool isPrefix)
        {
            Inner = inner;
            Operator = op;
            IsPrefix = isPrefix;
        }

        public UnaryOperatorExpression(Expression inner, OperatorLiteralExpression op, bool isPrefix) : this(inner, op.Operator, isPrefix)
        {
            OpExp = op;
        }

        public override string ToString()
        {
            return $"({(IsPrefix ? Operator : string.Empty)}{Inner}{(!IsPrefix ? Operator : string.Empty)})";
        }

        public override string ToCodeString()
        {
            string seperator = string.Empty;
            if (Operator == "new")
                seperator = " ";
            return $"{(IsPrefix ? Operator + seperator : string.Empty)}{Inner.ToCodeString()}{(!IsPrefix ? seperator + Operator : string.Empty)}";
        }
    }
}
