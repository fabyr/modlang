using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Imperative
{
    public class ContinueExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public override string ToString()
        {
            return "(continue)";
        }

        public override string ToCodeString()
        {
            return "continue;";
        }
    }
}
