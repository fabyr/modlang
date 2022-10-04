using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class CodeBlock : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression[] Expressions;
        public CodeBlock() { }
        public CodeBlock(Expression[] expressions)
        {
            Expressions = expressions;
        }

        public override string ToString()
        {
            return $"{{\r\n\t{string.Join("\r\n", Expressions.Cast<object>()).Replace("\n", "\n\t")}\r\n}}";
        }

        public override string ToCodeString()
        {
            return $"{{\r\n\t{string.Join("\r\n", Expressions.Select(_ => _.ToCodeString() + ";")).Replace("\n", "\n\t")}\r\n}}";
        }

        public override void Process()
        {
            Validator.Validate(Expressions);
        }
    }
}
