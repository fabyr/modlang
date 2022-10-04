using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.ControlStructures
{
    public class WhileExpression : Expression // If you think about it while is basically the same as an if statement (here copied code),
                                              // just that it repeats indefinitely until the condition becomes false
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Condition;
        public Expression Inner;
        public WhileExpression()
        {

        }
        public WhileExpression(Expression condition, Expression inner)
        {
            Condition = condition;
            Inner = inner;
        }

        public override string ToString()
        {
            return $"while({Condition})\r\n{(Inner is CodeBlock ? Inner.ToString() : "\t" + Inner.ToString().Replace("\n", "\n\t"))}";
        }

        public override void Process()
        {
            Validator.Validate(Inner);
        }

        public override string ToCodeString()
        {
            return $"while({Condition.ToCodeString()})\r\n{(Inner is CodeBlock ? Inner.ToCodeString() : "\t" + Inner.ToCodeString().Replace("\n", "\n\t"))}";
        }
    }
}
