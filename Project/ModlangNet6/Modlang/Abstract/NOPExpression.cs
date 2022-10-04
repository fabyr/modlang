using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class NOPExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public override string ToCodeString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return Constants.EOISTR;
        }
    }
}
