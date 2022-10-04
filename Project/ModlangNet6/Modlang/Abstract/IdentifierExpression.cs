using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class IdentifierExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public string Identifier;
        public IdentifierExpression() { }
        public IdentifierExpression(string ident)
        {
            Identifier = ident;
        }

        public override string ToString()
        {
            return $"({Identifier})";
        }

        public override string ToCodeString()
        {
            return Identifier;
        }
    }
}
