using Modlang.Abstract;
using Modlang.Abstract.Literals;
using Modlang.Abstract.Types;
using Modlang.CommonUtilities;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public static class Util
    {
        private static readonly Type[] litTypes = new[]
        {
            typeof(CharLiteralExpression),
            typeof(DoubleLiteralExpression),
            typeof(FloatLiteralExpression),
            typeof(IntegerLiteralExpression),
            typeof(LongIntegerLiteralExpression),
            typeof(StringLiteralExpression),
            typeof(UIntegerLiteralExpression),
            typeof(ULongIntegerLiteralExpression),
        };

        public static bool InArray(this string[] haystack, string needle)
        {
            for (int i = 0; i < haystack.Length; i++)
                if (haystack[i] == needle)
                    return true;
            return false;
        }

        public static bool InArray<T>(this T[] haystack, T needle)
        {
            for (int i = 0; i < haystack.Length; i++)
                if (haystack[i].Equals(needle))
                    return true;
            return false;
        }

        /*internal static string[] GetOperatorExpressionHashStrings(string op)
        {
            const string th = "this";
            if (op == th)
                return new string[]
                {
                    string.Concat(th, OperatorFunctionExpression.OperatorModifiers.EXPLICIT, Lexing.BraceType.ROUND, 0),
                    string.Concat(th, OperatorFunctionExpression.OperatorModifiers.IMPLICIT, Lexing.BraceType.ROUND, 0)
                };
            else
                return new string[]
                {
                    string.Concat(op, OperatorFunctionExpression.OperatorModifiers.PREFIX, Lexing.BraceType.ROUND, 0),
                    string.Concat(op, OperatorFunctionExpression.OperatorModifiers.POSTFIX, Lexing.BraceType.ROUND, 0),
                    string.Concat(op, OperatorFunctionExpression.OperatorModifiers.NORMAL, Lexing.BraceType.ROUND, 1),
                };
        }*/

        /*public static string GetHashString(this OperatorFunctionExpression opexp)
        {
            return string.Concat(opexp.Name.Operator, opexp.OpModifier, opexp.BraceType, string.Join<MlType>(",", Util.ExtractTypesFromArglist(opexp.Arguments)));
        }

        public static string GetHashString(string op, OperatorFunctionExpression.OperatorModifiers mod, Lexing.BraceType bt, MlType[] args)
        {
            return string.Concat(op, mod, bt, string.Join<MlType>(",", args));
        }*/

        /*public static bool LSequenceEqual<T>(this List<T> list, List<T> other)
        {
            if (list.Count != other.Count)
                return false;
            for (int i = 0; i < list.Count; i++)
                if (!list[i].Equals(other[i]))
                    return false;
            return true;
        }

        public static bool ASequenceEqual<T>(this T[] list, T[] other)
        {
            if (list.Length != other.Length)
                return false;
            for (int i = 0; i < list.Length; i++)
                if (!list[i].Equals(other[i]))
                    return false;
            return true;
        }*/

        public static bool ASequenceEqual(this MlType[] list, MlType[] other)
        {
            if (list.Length != other.Length)
                return false;
            for (int i = 0; i < list.Length; i++)
                if (!list[i].Equals(other[i]))
                    return false;
            return true;
        }

        public static MlType[] ExtractTypesFromArglist(Expression[] exp, Environment env)
        {
            MlType[] typeArray = new MlType[exp.Length];
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] is DeclarationExpression dexp)
                {
                    typeArray[i] = DatatypeExpressionToMlType(dexp.Datatype, env);
                }
                else if (exp[i] is IdentifierExpression iexp)
                {
                    typeArray[i] = DatatypeExpressionToMlType(iexp, env);
                }

                else if (exp[i] is BinaryOperatorExpression boexp && boexp.Operator == Constants.PeriodOpStr)
                {
                    typeArray[i] = DatatypeExpressionToMlType(boexp, env);
                }

                else
                    throw new ArgumentException("Invalid Expression-Type in arglist.");
            }
            return typeArray;
        }

        public static bool TypeCheck(MlObject subject, MlType otherType)
        {
            return subject.WorkingType == otherType;
        }

        public static bool TypeCheck(MlStoredObject subject, MlType otherType)
        {
            return subject.Type == otherType;
        }

        public static bool CanImplicitTypeConvert(MlObject subject, MlType toType, Environment env, bool doConv = true)
        {
            //if(doConv) toType = Execution.ConvertLocalTypeToGlobalType(env, toType);
            if (subject.UnderlyingType.IsNull() && toType.TypeStr == CoreLib.Constants.TYPE_OBJECT)
                return true;
            if (CoreLib.Constants.NativeOpTypes.InArray(subject.UnderlyingType.TypeStr) &&
                CoreLib.Constants.NativeOpTypes.InArray(toType.TypeStr) && (!ObjectOperations.Eq(subject.NativeValue.Value, 0) || 
                CoreLib.Constants.NativeTypeInformationLevel[subject.UnderlyingType.TypeStr]
                <= CoreLib.Constants.NativeTypeInformationLevel[toType.TypeStr]))
                return true;
            return subject.IsTypeInHierarchy(toType) || subject.FindOperatorFunctions("this").Any(_ => (_.OpModifier == OperatorFunctionExpression.OperatorModifiers.IMPLICIT)
                                                                && DatatypeExpressionToMlType(_.ReturnType, env) == toType);
        }

        public static MlType DatatypeExpressionToMlType(Expression exp, Environment env)
        {
            bool isArray = false;
            if (exp is BraceExpression bexp)
            {
                if (bexp.Kind != Lexing.BraceType.SQUARE)
                    throw new RuntimeException(exp.Origin, $"Invalid datatype '{exp}'");
                isArray = true;
                exp = bexp.Subject;
            }

            //StringBuilder sb = new StringBuilder();
            //TraversePeriodOperatorExpression(sb, exp);
            //return new MlType(sb.ToString(), isArray);
            MlStoredObject mso = env.ResolveObjectPath(exp);
            if(mso.Value is MlTypeObject mlto)
            {
                return new MlType(mlto.GetClassTypeStr(), isArray);
            }
            throw new RuntimeException(exp.Origin, "Invalid type");
        }

        public static bool IsLiteralExpression(Expression exp)
        {
            return litTypes.InArray(exp.GetType());
        }

        /*private static void TraversePeriodOperatorExpression(StringBuilder sb, Expression exp)
        {
            if (exp is IdentifierExpression iexp)
            {
                sb.Append(iexp.Identifier);
                return;
            }
            BinaryOperatorExpression boexp = exp as BinaryOperatorExpression;
            TraversePeriodOperatorExpression(sb, boexp.Left);
            sb.Append(".");
            TraversePeriodOperatorExpression(sb, boexp.Right);
        }*/

        public static MlObjectAccess ToStoredObjectAccess(this AccessModifier mod)
        {
            switch (mod)
            {
                case AccessModifier.PROTECTED:
                    return MlObjectAccess.CLASS_CONTEXT_PROTECTED;
                case AccessModifier.PUBLIC:
                    return MlObjectAccess.CLASS_CONTEXT_PUBLIC;

                case AccessModifier.PRIVATE:
                case AccessModifier.DEFAULT:
                default:
                    return MlObjectAccess.CLASS_CONTEXT_PRIVATE;
            }
        }

        public static string AssignmentExtractName(Expression assignment)
        {
            if (assignment is IdentifierExpression idexp)
                return idexp.Identifier;
            if (assignment is BinaryOperatorExpression boexp)
                return AssignmentExtractName(boexp.Left);
            return string.Empty;
        }
    }
}
