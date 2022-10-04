using Modlang.Abstract.Types;
using Modlang.Exceptions;
using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class FunctionExpression : FunctionBaseExpression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public AccessModifier Modifier;
        

        public bool IsStatic;
        public FunctionExpression() { }
        public FunctionExpression(string name, AccessModifier modifier, Expression[] arguments, Expression inner, Expression returnType)
        {
            Name = name;
            Modifier = modifier;
            Arguments = arguments;
            Inner = inner;
            ReturnType = returnType;
        }

        public override string ToString()
        {
            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}{(IsStatic ? "static " : "")}{ReturnType?.ToString() ?? "void"} {Name}({string.Join(", ", Arguments.Cast<object>())})" + 
                $"\r\n{(Inner is CodeBlock ? Inner.ToString() : "\t" + Inner.ToString().Replace("\n", "\n\t"))}";
        }

        public override string ToCodeString()
        {
            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}{(IsStatic ? "static " : "")}{ReturnType?.ToCodeString() ?? "void"} {Name}({string.Join(", ", Arguments.Select(_ => _.ToCodeString()))})" +
                $"\r\n{(Inner is CodeBlock ? Inner.ToCodeString() : "\t" + Inner.ToCodeString().Replace("\n", "\n\t"))}";
        }

        public override void Process()
        {
            if (ReturnType != null && !Util.IsIdentifyingExpression(ReturnType, allowArray: true))
                throw new ParserException(Origin, "Invalid return type.");

            Validator.ValidateFunctionArguments(Origin, Arguments);
            Validator.Validate(Inner);

            EffectiveArgcount = Arguments.Count(_ => _ is IdentifierExpression);
        }
    }
}
