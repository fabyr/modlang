using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.ControlStructures
{
    public class ForExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression[] StartingExpressions;
        public Expression[] RoundExpressions;
        public Expression Condition;

        public Expression Inner;
        public ForExpression()
        {

        }
        public ForExpression(Expression[] startingExpressions, Expression[] roundExpressions, Expression condition, Expression inner)
        {
            StartingExpressions = startingExpressions;
            RoundExpressions = roundExpressions;
            Condition = condition;
            Inner = inner;
        }

        public override string ToString()
        {
            return $"for({string.Join(", ", StartingExpressions.Cast<object>())}; {Condition}; {string.Join(", ", RoundExpressions.Cast<object>())})\r\n{(Inner is CodeBlock ? Inner.ToString() : "\t" + Inner.ToString().Replace("\n", "\n\t"))}";
        }

        public override string ToCodeString()
        {
            return $"for({string.Join(", ", StartingExpressions.Select(_ => _?.ToCodeString()))}; {Condition.ToCodeString()}; {string.Join(", ", RoundExpressions.Select(_ => _?.ToCodeString()))})\r\n{(Inner is CodeBlock ? Inner.ToCodeString() : "\t" + Inner.ToCodeString().Replace("\n", "\n\t"))}";
        }

        public override void Process()
        {
            Validator.Validate(Inner);
        }
    }
}
