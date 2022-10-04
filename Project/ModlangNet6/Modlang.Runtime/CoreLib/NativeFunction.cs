using Modlang.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.CoreLib
{
    public class NativeFunction : FunctionExpression
    {
        public new EnvironmentInitializer.NativeCodeFunction Inner;

        public NativeFunction(string name, int argcount, EnvironmentInitializer.NativeCodeFunction exec) : base(name, Abstract.Types.AccessModifier.PUBLIC, null, null, null)
        {
            Inner = exec;
            IsStatic = true;
        }
    }
}
