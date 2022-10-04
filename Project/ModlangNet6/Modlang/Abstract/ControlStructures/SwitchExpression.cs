using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.ControlStructures
{
    public class SwitchExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression Subject;
        public SwitchCase[] Cases; // Important that they are in correct order so the lack of "break;" proceeds to the correct next case
        public SwitchExpression()
        {

        }
        public SwitchExpression(Expression subject, SwitchCase[] cases)
        {
            Subject = subject;
            Cases = cases;
        }

        public override string ToString()
        {
            return $"switch({Subject})\r\n{{\r\n\t{string.Join("\r\n", Cases.Cast<object>()).Replace("\n", "\n\t")}\r\n}}";
        }

        public override string ToCodeString()
        {
            return $"switch({Subject.ToCodeString()})\r\n{{\r\n\t{string.Join("\r\n", Cases.Select(_ => _.ToCodeString())).Replace("\n", "\n\t")}\r\n}}";
        }

        public override void Process()
        {
            foreach(SwitchCase sc in Cases)
            {
                Validator.Validate(sc.Inner);
            }
        }
    }
}
