using Modlang.Abstract.Types;
using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class ConstructorExpression : FunctionBaseExpression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public AccessModifier Modifier;
        public ConstructorExpression() { }
        public ConstructorExpression(AccessModifier modifier, Expression[] arguments, Expression inner)
        {
            Modifier = modifier;
            Arguments = arguments;
            Inner = inner;
        }

        public override string ToString()
        {
            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}this({string.Join(", ", Arguments.Cast<object>())})" +
                $"\r\n{(Inner is CodeBlock ? Inner.ToString() : "\t" + Inner.ToString().Replace("\n", "\n\t"))}";
        }

        public override void Process()
        {
            Validator.ValidateFunctionArguments(Origin, Arguments);
            Validator.Validate(Inner);

            EffectiveArgcount = Arguments.Count(_ => _ is IdentifierExpression);
        }

        public override string ToCodeString()
        {
            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}this({string.Join(", ", Arguments.Select(_ => _.ToCodeString()))})" +
                $"\r\n{(Inner is CodeBlock ? Inner.ToCodeString() : "\t" + Inner.ToCodeString().Replace("\n", "\n\t"))}";
        }
    }
}
