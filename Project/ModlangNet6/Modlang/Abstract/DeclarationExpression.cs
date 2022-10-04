using Modlang.Abstract.Types;
using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class DeclarationExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public AccessModifier Modifier = AccessModifier.DEFAULT;
        public Expression Datatype;
        public Expression[] Assignments;
        public bool IsConst = false;
        public bool IsStatic = false;
        public DeclarationExpression() { }
        public DeclarationExpression(Expression datatype, params Expression[] assignments)
        {
            Datatype = datatype;
            Assignments = assignments;
        }

        public override string ToString()
        {
            return $"({(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}{(IsStatic ? "static " : "")}{(IsConst ? "const " : "")}{Datatype} {string.Join(", ", Assignments.Cast<object>())})";
        }

        public override string ToCodeString()
        {
            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}{(IsStatic ? "static " : "")}{(IsConst ? "const " : "")}{Datatype.ToCodeString()} {string.Join(", ", Assignments.Select(_ => _.ToCodeString()))}";
        }

        public override void Process()
        {
            if (!Util.IsIdentifyingExpression(Datatype, allowArray: true))
                throw new ParserException(Origin, "Invalid datatype.");
            foreach(Expression exp in Assignments)
            {
                if (exp is BinaryOperatorExpression bexp && bexp.Left is IdentifierExpression)
                    continue;
                if (exp is IdentifierExpression)
                    continue;
                throw new ParserException(exp.Origin, "Invalid identifier.");
            }
        }
    }
}
