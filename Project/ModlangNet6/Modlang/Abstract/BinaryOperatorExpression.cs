using Modlang.Abstract.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class BinaryOperatorExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Left;
        public Expression Right;
        public OperatorLiteralExpression OpExp;
        public string Operator;
        public BinaryOperatorExpression() { }
        public BinaryOperatorExpression(Expression left, Expression right, string op)
        {
            Left = left;
            Right = right;
            Operator = op;
        }

        public BinaryOperatorExpression(Expression left, Expression right, OperatorLiteralExpression op) : this(left, right, op.Operator)
        {
            OpExp = op;
        }

        public override string ToString()
        {
            return $"({Left} {Operator} {Right})";
        }

        public string ToCodeString(bool noBrace)
        {
            bool special = Operator == ".";
            string seperator = string.Empty;
            if (!special)
                seperator = " ";
            if (Util.IsAssignmentOperator(Operator) || special || noBrace)
                return $"{Left.ToCodeString()}{seperator}{Operator}{seperator}{Right.ToCodeString()}";
            return $"({Left.ToCodeString()}{seperator}{Operator}{seperator}{Right.ToCodeString()})";
        }

        public override string ToCodeString()
        {
            return ToCodeString(false);
        }
    }
}
