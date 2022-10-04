using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.ControlStructures
{
    public class IfExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Condition;
        public Expression Inner;
        public Expression ElseInner;
        public IfExpression()
        {

        }
        public IfExpression(Expression condition, Expression inner, Expression elseInner = null)
        {
            Condition = condition;
            Inner = inner;
            ElseInner = elseInner;
        }

        public override string ToString()
        {
            return $"if({Condition})\r\n{(Inner is CodeBlock ? Inner.ToString() : "\t" + Inner.ToString().Replace("\n", "\n\t"))}" +
                (ElseInner != null ? $"\r\nelse\r\n{(ElseInner is CodeBlock ? ElseInner.ToString() : "\t" + ElseInner.ToString().Replace("\n", "\n\t"))}" : "");
        }

        public override string ToCodeString()
        {
            return $"if({Condition.ToCodeString()})\r\n{(Inner is CodeBlock ? Inner.ToCodeString() : "\t" + Inner.ToCodeString().Replace("\n", "\n\t"))}" +
                (ElseInner != null ? $"\r\nelse\r\n{(ElseInner is CodeBlock ? ElseInner.ToCodeString() : "\t" + ElseInner.ToCodeString().Replace("\n", "\n\t"))}" : "");
        }

        public override void Process()
        {
            Validator.Validate(Inner);
            if(ElseInner != null)
                Validator.Validate(ElseInner);
        }
    }
}
