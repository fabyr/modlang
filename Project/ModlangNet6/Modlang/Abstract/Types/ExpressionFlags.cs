using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Types
{
    public enum ExpressionFlags : uint
    {
        NONE = 0,
        BRACED = 0b00000001
    }
}
