using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public abstract class FunctionBaseExpression : Expression
    {
        public object[] RuntimeParsedArgs = null;
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public int EffectiveArgcount; // This will be used when someday default parameters will be implemented

        public Expression[] Arguments;
        public Expression Inner;
        public Expression ReturnType;
        public string Name;
    }
}
