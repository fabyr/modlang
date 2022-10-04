using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public enum MlObjectAccess
    {
        LOCAL,
        GLOBAL,
        CLASS_CONTEXT_PRIVATE,
        CLASS_CONTEXT_PROTECTED,
        CLASS_CONTEXT_PUBLIC
    }
}
