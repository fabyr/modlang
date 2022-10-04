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
    public class ClassExpression : Expression
    {
        public override object RuntimeFunc(Core.ForRuntime.IExpressionExecutor a, object b) { return a.Execute(this, b); }

        public AccessModifier Modifier;
        public string Name;
        public Expression[] Inner;
        public Expression[] InheritorExpressions;

        public bool IsPrimitive = false;

        public ClassExpression[] NestedTypes;

        public FunctionExpression[] StaticFunctions;
        public FunctionExpression[] NonStaticFunctions;

        public ConstructorExpression[] Constructors;
        public OperatorFunctionExpression[] OperatorFunctions;

        public DeclarationExpression[] StaticAttributes;
        public DeclarationExpression[] NonStaticAttributes;
        public ClassExpression() { }
        public ClassExpression(AccessModifier mod, string name, Expression[] innerExpressions, Expression[] inheritorExpressions)
        {
            Modifier = mod;
            Name = name;
            Inner = innerExpressions;
            InheritorExpressions = inheritorExpressions;
        }

        public override string ToString()
        {
            return $"{Modifier} {(IsPrimitive ? "primitive " : string.Empty)}class {Name}{((InheritorExpressions == null || InheritorExpressions.Length == 0) ? "" : " : " + string.Join(", ", InheritorExpressions.Cast<object>()))}\r\n{{\r\n\t"+
                $"{string.Join("\r\n", Inner.Cast<object>()).Replace("\n", "\n\t")}\r\n}}";
        }

        public override string ToCodeString()
        {
            return $"{Modifier} {(IsPrimitive ? "primitive " : string.Empty)}class {Name}{((InheritorExpressions == null || InheritorExpressions.Length == 0) ? "" : " : " + string.Join(", ", InheritorExpressions.Select(_ => _.ToCodeString())))}\r\n{{\r\n\t" +
                $"{string.Join("\r\n", Inner.Select(_ => _.ToCodeString())).Replace("\n", "\n\t")}\r\n}}";
        }

        public override void Process()
        {
            if(InheritorExpressions != null)
            foreach(Expression inhexp in InheritorExpressions)
            {
                if (!Util.IsIdentifyingExpression(inhexp))
                    throw new ParserException(inhexp.Origin, "Invalid superclass.");
            }

            List<ClassExpression> nestedTypes = new List<ClassExpression>();
            List<FunctionExpression> staticFuncs = new List<FunctionExpression>();
            List<FunctionExpression> nonStaticFuncs = new List<FunctionExpression>();
            List<OperatorFunctionExpression> opFuncs = new List<OperatorFunctionExpression>();
            List<DeclarationExpression> staticDecls = new List<DeclarationExpression>();
            List<DeclarationExpression> nonStaticDecls = new List<DeclarationExpression>();
            List<ConstructorExpression> consList = new List<ConstructorExpression>();

            foreach(Expression exp in Inner)
            {
                if(exp is ClassExpression cexp)
                {
                    nestedTypes.Add(cexp);
                } else if(exp is FunctionExpression fexp)
                {
                    if (fexp.IsStatic)
                        staticFuncs.Add(fexp);
                    else
                        nonStaticFuncs.Add(fexp);
                } else if(exp is DeclarationExpression dexp)
                {
                    if (dexp.IsStatic || dexp.IsConst)
                        staticDecls.Add(dexp);
                    else
                        nonStaticDecls.Add(dexp);
                } else if(exp is OperatorFunctionExpression opexp)
                {
                    opFuncs.Add(opexp);
                }else if(exp is ConstructorExpression conexp)
                {
                    consList.Add(conexp);
                } 
                else
                    throw new ParserException(exp.Origin, "Invalid expression inside class.");
            }

            NestedTypes = nestedTypes.ToArray();
            StaticFunctions = staticFuncs.ToArray();
            NonStaticFunctions = nonStaticFuncs.ToArray();
            OperatorFunctions = opFuncs.ToArray();
            StaticAttributes = staticDecls.ToArray();
            NonStaticAttributes = nonStaticDecls.ToArray();
            Constructors = consList.ToArray();

            foreach (Expression exp in Inner)
            {
                exp.Process();
            }

            /*Type[] validTypes = new[] 
            { 
                typeof(ClassExpression),
                typeof(DeclarationExpression), 
                typeof(ConstructorExpression),
                typeof(FunctionExpression),
                typeof(OperatorFunctionExpression)
            };

            foreach(Expression exp in Inner)
            {
                if (!validTypes.Contains(exp.GetType()))
                    throw new ParserException(exp.Origin, "Invalid expression inside class.");
                exp.Validate();
            }*/
        }
    }
}
