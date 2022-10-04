using Modlang.Abstract;
using Modlang.Abstract.Types;
using Modlang.Runtime.CoreLib;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlTypeObject : MlObject
    {
        public ClassExpression Class;
        public MlTypeObject ParentClassObj;

        public List<MlStoredObject> NestedTypes = new List<MlStoredObject>();

        public MlStoredObject FindNestedType(string name)
        {
            return NestedTypes.Find(_ => (_.Value as MlTypeObject).Class.Name == name);
        }

        public string GetClassTypeStr()
        {
            return (ParentClassObj == null ? string.Empty : (ParentClassObj.GetClassTypeStr() + ".")) + Class.Name;
        }

        private string GetTypeString(Environment env)
        {
            MlTypeObject mlto = TypeObject;
            if (mlto == null)
                mlto = this;//return "<NO TYPE INFORMATION>";
            StringBuilder sb = new StringBuilder();

            const string NoneStr = "<None>";

            void WriteDecl(DeclarationExpression decl)
            {
                sb.Append("\t");
                sb.Append(decl.Modifier.GetString());
                sb.Append(" ");
                sb.Append(string.Join(", ", decl.Assignments.Select(Util.AssignmentExtractName)));
                sb.Append(" : ");
                sb.Append(Util.DatatypeExpressionToMlType(decl.Datatype, env));
                sb.AppendLine();
            }

            void WriteFexp(FunctionExpression fexp)
            {
                sb.Append("\t");
                sb.Append(fexp.Modifier.GetString());
                sb.Append(" ");
                sb.Append(fexp.Name);
                sb.Append("(");
                sb.Append(string.Join(", ", Util.ExtractTypesFromArglist(fexp.Arguments, env).Select(_ => $"{{{_}}}")));
                sb.Append(")");
                sb.Append(" : ");
                sb.Append(fexp.ReturnType == null ? "void" : Util.DatatypeExpressionToMlType(fexp.ReturnType, env).ToString());
                sb.AppendLine();
            }

            sb.Append("Inherits from: ");
            sb.AppendLine(mlto.Class.InheritorExpressions == null || mlto.Class.InheritorExpressions.Length == 0 ? (mlto.GetClassTypeStr() == CoreLib.Constants.TYPE_OBJECT ? NoneStr : CoreLib.Constants.TYPE_OBJECT) : string.Join<MlType>(", ", Util.ExtractTypesFromArglist(mlto.Class.InheritorExpressions, env)));

            sb.AppendLine("Static Attributes:");
            foreach (DeclarationExpression decl in mlto.Class.StaticAttributes)
                WriteDecl(decl);
            if (mlto.Class.StaticAttributes.Length == 0)
                sb.AppendLine($"\t{NoneStr}");

            sb.AppendLine("NonStatic Attributes:");
            foreach (DeclarationExpression decl in mlto.Class.NonStaticAttributes)
                WriteDecl(decl);
            if (mlto.Class.NonStaticAttributes.Length == 0)
                sb.AppendLine($"\t{NoneStr}");


            sb.AppendLine("Static Functions:");
            foreach (FunctionExpression fexp in mlto.Class.StaticFunctions)
                WriteFexp(fexp);
            if (mlto.Class.StaticFunctions.Length == 0)
                sb.AppendLine($"\t{NoneStr}");

            sb.AppendLine("NonStatic Functions:");
            foreach (FunctionExpression fexp in mlto.Class.NonStaticFunctions)
                WriteFexp(fexp);
            if (mlto.Class.NonStaticFunctions.Length == 0)
                sb.AppendLine($"\t{NoneStr}");



            sb.AppendLine("Overloaded Operators:");
            foreach (OperatorFunctionExpression fexp in mlto.Class.OperatorFunctions)
            {
                sb.Append("\t");
                sb.Append(fexp.Modifier.GetString());
                sb.Append(" ");
                sb.Append(fexp.OpModifier.GetString());
                sb.Append(" operator ");
                sb.Append(fexp.Name.Operator);
                sb.Append(fexp.BraceType == Lexing.BraceType.SQUARE ? "[" : "(");
                sb.Append(string.Join(", ", Util.ExtractTypesFromArglist(fexp.Arguments, env).Select(_ => $"{{{_}}}")));
                sb.Append(fexp.BraceType == Lexing.BraceType.SQUARE ? "]" : ")");
                sb.Append(" : ");
                sb.Append(fexp.ReturnType == null ? "void" : Util.DatatypeExpressionToMlType(fexp.ReturnType, env).ToString());
                sb.AppendLine();
            }
            if (mlto.Class.OperatorFunctions.Length == 0)
                sb.AppendLine($"\t{NoneStr}");


            return sb.ToString();
        }

        public override string ToString(Environment env)
        {
            return $"{UnderlyingType}:{GetClassTypeStr()} \r\n{{\r\n\t{GetTypeString(env).Replace("\n", "\n\t")}\r\n}}";
        }

        public override int GetHashCode()
        {
            return GetClassTypeStr().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj != null && obj is MlTypeObject mlto)
                return GetClassTypeStr() == mlto.GetClassTypeStr(); // This equality works for now
            return false;
        }

        public void Init()
        {
            //_env.EnterClassContext(this, true);
            InitCore();
            //_env.ExitClassContext(true);
        }

        private void InitCore()
        {
            for (int i = 0; i < Class.NestedTypes.Length; i++)
            {
                ClassExpression nClass = Class.NestedTypes[i];
                MlTypeObject mlto = new MlTypeObject(nClass, _env, this);
                NestedTypes.Add(new MlStoredObject() { Value = mlto, Access = nClass.Modifier.ToStoredObjectAccess(), IsConst = true, Type = CoreLib.Constants.TYPE_TYPE.GetNonArrayType() });
                mlto.Init();
            }

            for (int i1 = 0; i1 < Class.StaticFunctions.Length; i1++)
            {
                FunctionExpression fexp = Class.StaticFunctions[i1];

                Functions.Add(fexp);
            }

            for (int i = 0; i < Class.StaticAttributes.Length; i++)
            {
                DeclarationExpression dexp = Class.StaticAttributes[i];
                //MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                for (int i1 = 0; i1 < dexp.Assignments.Length; i1++)
                {
                    Expression aexp = dexp.Assignments[i1];
                    MlType dat = Util.DatatypeExpressionToMlType(dexp.Datatype, _env);
                    if (aexp is IdentifierExpression idexp)
                    {
                        Attributes.Add(idexp.Identifier, new MlStoredObject()
                        {
                            Value = MlNull,
                            Access = dexp.Modifier.ToStoredObjectAccess(),
                            Type = dat
                        });
                        continue;
                    }
                    BinaryOperatorExpression boexp = aexp as BinaryOperatorExpression;
                    string ident = (boexp.Left as IdentifierExpression).Identifier;
                    _env.EnterClassContext(this);
                    MlObject right = boexp.Right.Execute(_env);
                    _env.ExitClassContext();
                    Execution.TypeAssert(_env, boexp, right, dat);
                        //throw new RuntimeException(boexp.Origin, $"Datatype mismatch. Expected {datatype}; got {right.WorkingType}");
                    Attributes.Add(ident, new MlStoredObject()
                    {
                        Value = right,
                        Access = dexp.Modifier.ToStoredObjectAccess(),
                        Type = dat
                    });
                }
            }

            /*foreach (Expression exp in cexp.Inner)
            {
                if(exp is ClassExpression ncexp)
                {
                    NestedTypes.Add(new MlStoredObject() { Value = new MlTypeObject(ncexp, env, this), Access = ncexp.Modifier.ToStoredObjectAccess(), IsConst = true, Type = CoreLib.Constants.TYPE_TYPE.GetNonArrayType() });
                } else
                if (exp is FunctionExpression fexp && fexp.IsStatic)
                {
                    // Should already be checked by parser
                    //if (Functions.Any(_ => _.Name == fexp.Name)) throw new RuntimeException(fexp.Origin, "A Function with the same name already exists");
                    Functions.Add(fexp);
                }
                else if (exp is OperatorFunctionExpression opexp && opexp.IsStatic)
                {
                    throw new RuntimeException(opexp.Origin, "Static Operators are not supported.");
                    //Operators.Add(opexp);
                } else if (exp is DeclarationExpression dexp && (dexp.IsStatic || dexp.IsConst))
                {
                    MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                    foreach (Expression aexp in dexp.Assignments)
                    {
                        if (aexp is IdentifierExpression idexp)
                        {
                            Attributes.Add(idexp.Identifier, new MlStoredObject()
                            {
                                Value = MlNull,
                                Access = dexp.Modifier.ToStoredObjectAccess(),
                                Type = Util.DatatypeExpressionToMlType(dexp.Datatype)
                            });
                            continue;
                        }
                        BinaryOperatorExpression boexp = aexp as BinaryOperatorExpression;
                        string ident = (boexp.Left as IdentifierExpression).Identifier;
                        MlObject right = boexp.Right.Execute(env);
                        if (right.WorkingType != datatype)
                            throw new InternalRuntimeException($"Datatype mismatch. Expected {datatype}; got {right.WorkingType}");
                        Attributes.Add(ident, new MlStoredObject()
                        {
                            Value = right,
                            Access = dexp.Modifier.ToStoredObjectAccess(),
                            Type = Util.DatatypeExpressionToMlType(dexp.Datatype)
                        });
                    }
                }
            }*/

            /*foreach (Expression exp in cexp.Inner)
            {
                if (exp is DeclarationExpression dexp && (dexp.IsStatic || dexp.IsConst))
                {
                    MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                    foreach (Expression aexp in dexp.Assignments)
                    {
                        if (aexp is IdentifierExpression idexp)
                        {
                            Attributes.Add(idexp.Identifier, new MlStoredObject()
                            {
                                Value = MlNull,
                                Access = dexp.Modifier.ToStoredObjectAccess(),
                                Type = Util.DatatypeExpressionToMlType(dexp.Datatype)
                            });
                            continue;
                        }
                        BinaryOperatorExpression boexp = aexp as BinaryOperatorExpression;
                        string ident = (boexp.Left as IdentifierExpression).Identifier;
                        MlObject right = boexp.Right.Execute(env);
                        if (right.WorkingType != datatype)
                            throw new InternalRuntimeException($"Datatype mismatch. Expected {datatype}; got {right.WorkingType}");
                        Attributes.Add(ident, new MlStoredObject()
                        {
                            Value = right,
                            Access = dexp.Modifier.ToStoredObjectAccess(),
                            Type = Util.DatatypeExpressionToMlType(dexp.Datatype)
                        });
                    }
                }
            }*/

            // Static constructors are not supported

            /*int c = 0;
            foreach (Expression exp in cexp.Inner)
            {
                if (exp is ConstructorExpression coexp)
                {
                    c++;
                    if (coexp.Arguments.Length == args.Length)
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            Expression arg = coexp.Arguments[i];
                            if (arg is DeclarationExpression dexp)
                            {
                                MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                                if (datatype != args[i].WorkingType)
                                    throw new InternalRuntimeException($"Datatype mismatch. Expected {datatype}; got {args[i].WorkingType}");
                            }
                        }
                        env.EnterClassContext(obj);
                        coexp.Call(env, args);
                        env.ExitClassContext();
                        return obj;
                    }
                }
            }
            if (c == 0 && args.Length == 0)
                return obj;*/
        }

        public MlTypeObject(ClassExpression cexp, Environment env, MlTypeObject parent = null)
        {
            _env = env;
            Class = cexp;
            UnderlyingType = CoreLib.Constants.TYPE_TYPE.GetNonArrayType();
            WorkingType = UnderlyingType;
            ParentClassObj = parent;

            //if(cexp.Name != CoreLib.Constants.TYPE_OBJECT)  InheritFrom(this, env.FindNativeClass(CoreLib.Constants.TYPE_OBJECT), env);
            EnvironmentInitializer.ObjectifyMlObject(this, env);
        }
    }
}
