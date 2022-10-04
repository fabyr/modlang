using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Imperative
{
    public class ReturnExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Inner;
        public ReturnExpression()
        {

        }
        public ReturnExpression(Expression inner)
        {
            Inner = inner;
        }

        public override string ToString()
        {
            return $"(return {Inner})";
        }
        public override string ToCodeString()
        {
            return $"return{(Inner != null ? " " + Inner.ToCodeString() : "")}";
        }
    }
}
