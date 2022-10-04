using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.ControlStructures
{
    public class SwitchCase
    {
        public Expression Match { get; set; }
        public Expression[] Inner { get; set; } // The Case in the switch statement is special as it can contain multiple statements without the use of a code block

        public bool IsDefault { get; set; } = false;

        public SwitchCase(Expression match, Expression[] innerExpressions)
        {
            Match = match;
            Inner = innerExpressions;
        }

        public override string ToString()
        {
            return $"{(IsDefault ? "default" : "case " + Match.ToString())}:\r\n\t{string.Join("\r\n", Inner.Cast<object>()).Replace("\n", "\n\t")}";
        }

        public string ToCodeString()
        {
            return $"{(IsDefault ? "default" : "case " + Match.ToCodeString())}:\r\n\t{string.Join("\r\n", Inner.Select(_ => _.ToCodeString())).Replace("\n", "\n\t")}";
        }
    }
}
