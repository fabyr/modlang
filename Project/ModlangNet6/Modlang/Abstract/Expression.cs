using Modlang.Abstract.Types;
using Modlang.Core.ForRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public abstract class Expression
    {
        public abstract object RuntimeFunc(IExpressionExecutor exec, object b);

        public FromFileObject Origin;
        public ExpressionFlags Flags = 0u;

        public virtual void Process() // i didn't make this abstract because many don't require usage of this
        {

        }

        public abstract string ToCodeString();
    }
}
