using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Parsing
{
    public enum ExpressionMakerFlags : uint
    {
        NONE = 0,
        COMMA_HANDLED = 0b00000001,
        ALLOW_CURLY_BRACE_EXPRESSION = 0b00000010,
    }
}
