using Modlang.Abstract.Literals;
using Modlang.Abstract.Types;
using Modlang.Exceptions;
using Modlang.Lexing;
using Modlang.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract
{
    public class OperatorFunctionExpression : FunctionBaseExpression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public enum OperatorModifiers : byte
        {
            NORMAL = 0,
            PREFIX = 1,
            POSTFIX = 2,
            IMPLICIT = 3,
            EXPLICIT = 4,
            GET = 5,
            SET = 6
        }

        public OperatorModifiers OpModifier;
        public AccessModifier Modifier;
        public BraceType BraceType;
        public new OperatorLiteralExpression Name;

        public bool IsStatic;
        public OperatorFunctionExpression() { }
        public OperatorFunctionExpression(OperatorLiteralExpression name, OperatorModifiers opMod, AccessModifier modifier, BraceType bt, Expression[] arguments, Expression inner, Expression returnType)
        {
            Name = name;
            OpModifier = opMod;
            Modifier = modifier;
            BraceType = bt;
            Arguments = arguments;
            Inner = inner;
            ReturnType = returnType;
        }
        public override string ToString()
        {
            string brOpen = string.Empty, brClose = string.Empty;
            switch (BraceType)
            {
                case BraceType.ROUND:
                    brOpen = "(";
                    brClose = ")";
                    break;
                case BraceType.SQUARE:
                    brOpen = "[";
                    brClose = "]";
                    break;
                case BraceType.CURLY:
                    brOpen = " { ";
                    brClose = " }";
                    break;
            }

            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}{(IsStatic ? "static " : "")}{ReturnType?.ToString() ?? "void"} {(OpModifier == OperatorModifiers.NORMAL ? "" : OpModifier.GetString() + " ")}operator {Name}{brOpen}{string.Join(", ", Arguments.Cast<object>())}{brClose}" +
                $"\r\n{(Inner is CodeBlock ? Inner.ToString() : "\t" + Inner.ToString().Replace("\n", "\n\t"))}";
        }

        public override void Process()
        {
            bool isThis = Name is OperatorLiteralExpression idexp && idexp.Operator.Equals("this");

            if (isThis && OpModifier == OperatorModifiers.SET && BraceType == BraceType.SQUARE)
            {
                Expression[] newArgs = new Expression[Arguments.Length + 1];
                // insert the "value" parameter
                newArgs[0] = new DeclarationExpression(ReturnType, new IdentifierExpression("value"));
                Arguments.CopyTo(newArgs, 1);
                Arguments = newArgs;
            }

            /*if (!(Name is OperatorLiteralExpression) && !Util.IsIdentifyingExpression(Name))
                throw new ParserException(Origin, "Invalid 'name' for this operator overload.");*/

            if(!Util.IsIdentifyingExpression(ReturnType, allowArray: true))
                throw new ParserException(Origin, "Invalid return type.");

            if (!isThis)
            {
                if (BraceType == BraceType.SQUARE)
                    throw new ParserException(Origin, "Invalid operator construct."); // TODO: Perhaps better error message#

                switch (OpModifier)
                {
                    case OperatorModifiers.NORMAL:
                        if (Arguments.Length != 1)
                            throw new ParserException(Origin, "Invalid amount of parameters. Must be 1.");
                        break;
                    case OperatorModifiers.PREFIX:
                    case OperatorModifiers.POSTFIX:
                        if (Arguments.Length != 0)
                            throw new ParserException(Origin, "This kind of operator mustn't have any parameters.");
                        break;
                    default:
                        break;
                }
            }
            bool castingOp = OpModifier == OperatorModifiers.IMPLICIT || OpModifier == OperatorModifiers.EXPLICIT;
            bool indexingOp = OpModifier == OperatorModifiers.GET && OpModifier == OperatorModifiers.SET;

            if(isThis && indexingOp && BraceType == BraceType.ROUND)
                throw new ParserException(Origin, $"Invalid modifier '{OpModifier}' for a calling-operator.");

            // This is not needed anymore because of the arg-insert on top but i'll leave it in anyways :^)
            if (isThis && OpModifier == OperatorModifiers.SET && BraceType == BraceType.SQUARE && Arguments.Length < 1)
                throw new ParserException(Origin, "Indexing operators with the 'set' modifier have to have at least 1 argument (the first argument is the setting-value).");
            
            if (!isThis && indexingOp)
                throw new ParserException(Origin, "Invalid operator name. For non-casting (calling and indexing) 'this'-operators the operator name must be 'this'");
            
            /*if ((isThis && !castingOp && !indexingOp))
                throw new ParserException(Origin, "Any non-casting (calling and indexing) 'this'-operator must be either a set or get operator");*/
            
            if (!isThis && castingOp)
                throw new ParserException(Origin, "Invalid operator name. For cast overloads the operator name must be 'this'");
            if(isThis && castingOp && Arguments.Length != 0)
                throw new ParserException(Origin, "Invalid amount of parameters. Must be 0.");

            Validator.ValidateFunctionArguments(Origin, Arguments);
            Validator.Validate(Inner);

            EffectiveArgcount = Arguments.Count(_ => _ is IdentifierExpression);
        }

        public override string ToCodeString()
        {
            string brOpen = string.Empty, brClose = string.Empty;
            switch (BraceType)
            {
                case BraceType.ROUND:
                    brOpen = "(";
                    brClose = ")";
                    break;
                case BraceType.SQUARE:
                    brOpen = "[";
                    brClose = "]";
                    break;
                case BraceType.CURLY:
                    brOpen = " { ";
                    brClose = " }";
                    break;
            }

            return $"{(Modifier == AccessModifier.DEFAULT ? string.Empty : Modifier.GetString() + " ")}{(IsStatic ? "static " : "")}{ReturnType?.ToCodeString() ?? "void"} {(OpModifier == OperatorModifiers.NORMAL ? "" : OpModifier.GetString() + " ")}operator {Name.ToCodeString()}{brOpen}{string.Join(", ", Arguments.Select(_ => _.ToCodeString()))}{brClose}" +
                $"\r\n{(Inner is CodeBlock ? Inner.ToCodeString() : "\t" + Inner.ToCodeString().Replace("\n", "\n\t"))}";
        }
    }

    public static class OperatorModifierExtensions
    {
        public static string GetString(this OperatorFunctionExpression.OperatorModifiers opMod)
        {
            switch (opMod)
            {
                case OperatorFunctionExpression.OperatorModifiers.NORMAL:
                    return "[normal]";
                case OperatorFunctionExpression.OperatorModifiers.PREFIX:
                    return "prefix";
                case OperatorFunctionExpression.OperatorModifiers.POSTFIX:
                    return "postfix";
                case OperatorFunctionExpression.OperatorModifiers.IMPLICIT:
                    return "implicit";
                case OperatorFunctionExpression.OperatorModifiers.EXPLICIT:
                    return "explicit";
                case OperatorFunctionExpression.OperatorModifiers.GET:
                    return "get";
                case OperatorFunctionExpression.OperatorModifiers.SET:
                    return "set";
                default:
                    return string.Empty;
            }
        }
    }
}
