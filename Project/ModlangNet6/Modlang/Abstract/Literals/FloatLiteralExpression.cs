using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Literals
{
    public class FloatLiteralExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public float Value;
        public FloatLiteralExpression()
        {

        }
        public FloatLiteralExpression(float value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"(FLOAT:{Value.ToString(CultureInfo.InvariantCulture)})";
        }
        public override string ToCodeString()
        {
            return Value.ToString();
        }
    }
}
