using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class CastExpression : Expression // Special Unary
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public Expression CastingExpression; // the expression inside braces before the thing supposed to be cast
        public Expression Subject;
        public CastExpression() { }
        public CastExpression(Expression castingExpression, Expression subject)
        {
            CastingExpression = castingExpression;
            Subject = subject;
        }

        public override string ToString()
        {
            return $"(({CastingExpression}) {Subject})";
        }

        public override string ToCodeString()
        {
            return $"({CastingExpression.ToCodeString()}){Subject.ToCodeString()}";
        }

        public override void Process()
        {
            // NOTE: I don't think this is actually called ever. Because the normal operator expressions,
            //       where this would reside in, are never checked (currently)
            if (!Util.IsIdentifyingExpression(CastingExpression, allowArray: true))
                throw new ParserException(Origin, "Invalid cast body.");
        }
    }
}
