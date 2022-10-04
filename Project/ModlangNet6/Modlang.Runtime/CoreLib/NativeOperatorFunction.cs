using Modlang.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.CoreLib
{
    public class NativeOperatorFunction : OperatorFunctionExpression
    {
        public new EnvironmentInitializer.NativeCodeFunction Inner;

        public NativeOperatorFunction(string name, OperatorModifiers opmod, int argcount, EnvironmentInitializer.NativeCodeFunction exec) : base(new Abstract.Literals.OperatorLiteralExpression(name), opmod, Abstract.Types.AccessModifier.PUBLIC, Lexing.BraceType.ROUND, null, null, null)
        {
            Inner = exec;
        }
    }
}
