using Modlang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class BraceExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Subject;
        public Expression[] Arguments;

        public BraceType Kind;
        public BraceExpression() { }
        public BraceExpression(Expression subject, Expression[] arguments, BraceType kind)
        {
            Subject = subject;
            Arguments = arguments;
            Kind = kind;
        }

        public override string ToString()
        {
            string brOpen = string.Empty, brClose = string.Empty;
            switch (Kind)
            {
                case BraceType.ROUND:
                    brOpen = "(";
                    brClose = ")";
                    break;
                case BraceType.SQUARE:
                    brOpen = "[";
                    brClose = "]";
                    break;
                case BraceType.CURLY:
                    brOpen = " { ";
                    brClose = " }";
                    break;
            }
            return $"({Subject}{brOpen}{string.Join(", ", Arguments.Cast<object>())}{brClose})";
        }

        public override string ToCodeString()
        {
            string brOpen = string.Empty, brClose = string.Empty;
            switch (Kind)
            {
                case BraceType.ROUND:
                    brOpen = "(";
                    brClose = ")";
                    break;
                case BraceType.SQUARE:
                    brOpen = "[";
                    brClose = "]";
                    break;
                case BraceType.CURLY:
                    brOpen = " { ";
                    brClose = " }";
                    break;
            }
            return $"({Subject.ToCodeString()}{brOpen}{string.Join(", ", Arguments.Select(_ => _ is BinaryOperatorExpression boexp ? boexp.ToCodeString(true) : _.ToCodeString()))}{brClose})";
        }
    }
}
