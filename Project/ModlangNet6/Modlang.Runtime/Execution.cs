using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.Abstract.Imperative;
using Modlang.Abstract.Literals;
using Modlang.Runtime.CoreLib;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public static class Execution
    {
        public static MlObject Execute(this Expression exp, Environment env)
        {
            if (env.CurrentStackFrame.DoesAny)
            {
                return null;
            }
            return (MlObject)exp.RuntimeFunc(Environment.GlobalExpressionExecutor, env);
        }

        private static MlObject ExtractValue(MlStoredObject mso, Environment env)
        {
            if (mso is MlObjectNative mon)
                return new MlObjectNative.MlObjectNativeContainer() { NativeValue = mon.Value };
            return mso.Value.IsPrimitive ? mso.Value.CastClone(env) : mso.Value;
        }

        public static MlObject[] FetchObjects(Expression[] args, Environment env)
        {
            MlObject[] res = new MlObject[args.Length];
            for(int i = 0; i < res.Length; i++)
            {
                res[i] = args[i].Execute(env);
            }
            return res;
        }

        private static bool FuncArgInit(Expression[] arguments, MlObject[] args, Environment env)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                Expression exp = arguments[i];
                if (exp is DeclarationExpression dexp)
                {
                    ExecuteInternal(dexp, env);
                    if (dexp.Assignments.Length == 1)
                    {
                        MlType expectedType = Util.DatatypeExpressionToMlType(dexp.Datatype, env);
                        if (dexp.Assignments[0] is IdentifierExpression idexp)
                            env.SetValue(idexp, CastObject(args[i], expectedType, env, true));
                        else if (dexp.Assignments[0] is BinaryOperatorExpression boexp && boexp.Operator == "=")
                            env.SetValue(boexp.Left, CastObject(args[i], expectedType, env, true));
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            return true;
        }

        /*
        // Converts the simple name of a nested type to a globally identified one (class-path connected with periods)
        public static MlType ConvertLocalTypeToGlobalType(Environment env, MlType to)
        {
            try
            {
                MlStoredObject mso = env.ResolveObjectPath(to.TypeStr);
                if (mso.Value is MlTypeObject mlto && mlto.Class.Name == to.TypeStr)
                    to.TypeStr = mlto.GetClassTypeStr();
            }
            catch (RuntimeException) { }
            return to;
        }*/

        public static void TypeAssert(Environment env, Expression at, MlObject subject, MlType type, bool strict = false)
        {
            //type = ConvertLocalTypeToGlobalType(env, type);
            if (env.ExecutionOptions.NoTypeAssert)
                return;
            if (!subject.UnderlyingType.IsNull() && !Util.TypeCheck(subject, type) && (!strict && !Util.CanImplicitTypeConvert(subject, type, env, false)))
                throw new RuntimeException(at.Origin, $"Type mismatch. Expected '{type}'; Got '{subject.WorkingType}'");
        }

        public static MlObject Call(this FunctionExpression subject, Environment env, params MlObject[] args)
        {
            if(subject is NativeFunction natfunc)
            {
                return natfunc.Inner(env, args);
            }
            env.EnterStackFrame(false);
            if (!FuncArgInit(subject.Arguments, args, env))
                throw new RuntimeException(subject.Origin, "Malformed function head."); // TODO: Proper Origin
            subject.Inner.Execute(env);
            MlObject returned = env.CurrentStackFrame.ReturnValue;
            if (subject.ReturnType != null)
                TypeAssert(env, subject, returned, Util.DatatypeExpressionToMlType(subject.ReturnType, env));
            env.ExitStackFrame(false);
            return returned;
        }

        public static MlObject Call(this ConstructorExpression subject, Environment env, params MlObject[] args)
        {
            env.EnterStackFrame(false);
            if (!FuncArgInit(subject.Arguments, args, env))
                throw new RuntimeException(subject.Origin, "Malformed function head."); // TODO: Proper Origin
            subject.Inner.Execute(env);
            MlObject returned = env.CurrentStackFrame.ReturnValue;
            if (returned != null)
                throw new RuntimeException(subject.Origin, "A return statement is invalid in a constructor"); // TODO: Proper Origin
            env.ExitStackFrame(false);
            return null;
        }

        public static MlObject Call(this OperatorFunctionExpression subject, Environment env, params MlObject[] args)
        {
            if (subject is NativeOperatorFunction natfunco)
            {
                return natfunco.Inner(env, args);
            }
            env.EnterStackFrame(false);
            if (!FuncArgInit(subject.Arguments, args, env))
                throw new RuntimeException(subject.Origin, "Malformed function head."); // TODO: Proper Origin
            subject.Inner.Execute(env);
            MlObject returned = env.CurrentStackFrame.ReturnValue;
            TypeAssert(env, subject, returned, Util.DatatypeExpressionToMlType(subject.ReturnType, env));
            env.ExitStackFrame(false);
            return returned;
        }

        public static MlObject Call(this MlObjectCallable subject, Environment env, params MlObject[] args)
        {
            if (subject.Func is FunctionExpression fexp)
                return Call(fexp, env, args);
            if (subject.Func is OperatorFunctionExpression ofexp)
                return Call(ofexp, env, args);
            if (subject.Func is ConstructorExpression ctorexp)
                return Call(ctorexp, env, args);

            throw new Exception("You should not be here");
        }

        #region Main

        internal static MlObject ExecuteInternal(ReturnExpression rexp, Environment env)
        {
            MlObject retObj = rexp.Inner.Execute(env);
            env.CurrentStackFrame.DoesReturn = true;
            env.CurrentStackFrame.ReturnValue = retObj;
            return retObj;
        }

        internal static MlObject ExecuteInternal(BreakExpression rexp, Environment env)
        {
            env.CurrentStackFrame.DoesBreak = true;
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(ContinueExpression rexp, Environment env)
        {
            env.CurrentStackFrame.DoesContinue = true;
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(UnaryOperatorExpression exp, Environment env, int arrayPrecomputedArgcount = -1)
        {
            if(exp.Operator == "new" && exp.IsPrefix)
            {
                if(exp.Inner is BraceExpression brexp)
                {
                    switch (brexp.Kind)
                    {
                        case Lexing.BraceType.ROUND:
                            {
                                MlTypeObject mlto = env.ResolveObjectPath(brexp.Subject, typesOnly: true).Value as MlTypeObject;
                                if (mlto == null)
                                    throw new RuntimeException(brexp.Origin, $"Undefined type '{brexp.Subject}'");
                                return MlObject.FromClass(mlto, env, FetchObjects(brexp.Arguments, env));
                            }
                        case Lexing.BraceType.SQUARE:
                            {
                                MlTypeObject mlto = env.ResolveObjectPath(brexp.Subject, typesOnly: true).Value as MlTypeObject;
                                MlType mlt = mlto.GetClassTypeStr().GetNonArrayType();
                                if (mlto == null)
                                    throw new RuntimeException(brexp.Origin, $"Undefined type '{brexp.Subject}'");
                                if (brexp.Arguments.Length != 0)
                                {
                                    if (arrayPrecomputedArgcount == -1 && brexp.Arguments.Length != 1)
                                        throw new RuntimeException(brexp.Origin, "Invalid argument count. Expected exactly 1.");
                                    MlObject param = brexp.Arguments[0].Execute(env);
                                    param = CastObject(param, CoreLib.Constants.TYPE_LONG.GetNonArrayType(), env, true);
                                    long val = Convert.ToInt64(param.NativeValue.Value);
                                    if(arrayPrecomputedArgcount != -1 && val != arrayPrecomputedArgcount)
                                        throw new RuntimeException(brexp.Origin, $"Array length doesn't match with count of supplied elements. ({val} != {arrayPrecomputedArgcount})");
                                    return new MlArray(mlt, val, env);
                                }
                                else
                                {
                                    if (arrayPrecomputedArgcount == -1)
                                        throw new RuntimeException(brexp.Origin, "Missing length-argument for array.");
                                    return new MlArray(mlt, arrayPrecomputedArgcount, env);
                                }
                            }
                        case Lexing.BraceType.CURLY:
                            {
                                if(brexp.Subject is BraceExpression brexpInner)
                                {
                                    MlObject subj = ExecuteInternal(new UnaryOperatorExpression(brexpInner, "new", true), env, arrayPrecomputedArgcount: brexp.Arguments.Length);
                                    if (brexp.Arguments.All(_ => _ is BinaryOperatorExpression))
                                    {
                                        env.EnterClassContext(subj, true); // keep access to local variables with specialSoft = true
                                        for (int i = 0; i < brexp.Arguments.Length; i++)
                                        {
                                            Expression argExp = brexp.Arguments[i];
                                            ExecuteInternal((BinaryOperatorExpression)argExp, env);
                                        }

                                        env.ExitClassContext(true);
                                        return subj;
                                    }
                                    if(subj is MlArray ar && brexp.Arguments.All(_ => !(_ is DeclarationExpression)))
                                    {
                                        if(ar.Collection.Length != brexp.Arguments.Length)
                                            throw new RuntimeException(brexp.Origin, "Value count and array length mismatch");

                                        for(int i = 0; i < ar.Collection.Length; i++)
                                        {
                                            MlObject obj = brexp.Arguments[i].Execute(env);
                                            ar.Collection[i].ChangeValue(obj, env);
                                        }
                                        return subj;
                                    }
                                }
                                throw new RuntimeException(brexp.Origin, "Invalid curly brace expression");
                            }
                        default:
                            throw new NotImplementedException();
                    }
                }
                throw new RuntimeException(exp.Inner.Origin, "Invalid operand for 'new'");
            }

            MlStoredObject mso = env.ResolveObjectPath(exp.Inner);

            if(CoreLib.Constants.NativeOpTypes.InArray(mso.Value.UnderlyingType.TypeStr) && !mso.Value.UnderlyingType.IsArray)
            {
                MlObject g;
                if ((g = EnvironmentInitializer.NativeOperatorOperation(env, exp.Operator, true, exp.IsPrefix, mso.Value)) != null)
                    return g;
            }

            List<OperatorFunctionExpression> opFuncList = mso.Value.FindOperatorFunctions(exp.Operator);
            OperatorFunctionExpression.OperatorModifiers opMod = exp.IsPrefix ? OperatorFunctionExpression.OperatorModifiers.PREFIX : OperatorFunctionExpression.OperatorModifiers.POSTFIX;
            for (int i1 = 0; i1 < opFuncList.Count; i1++)
            {
                OperatorFunctionExpression opFunc = opFuncList[i1];
                if (opFunc != null && opFunc.OpModifier == opMod)
                {
                    env.EnterClassContext(mso.Value);
                    MlObject res = Call(opFunc, env);
                    env.ExitClassContext();
                    return res;
                }
            }

            throw new RuntimeException(exp.OpExp == null ? exp.Origin : exp.OpExp.Origin, $"Found no matching overloaded unary operator for '{exp.Operator}' in type '{mso.Value.UnderlyingType}'");
        }

        internal static MlObject ExecuteInternal(OperatorFunctionExpression exp, Environment env)
        {
            throw new InvalidOperationException("This is handled in MlObject");
        }

        internal static MlObject ExecuteInternal(NOPExpression exp, Environment env)
        {
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(IdentifierExpression exp, Environment env)
        {
            return ExtractValue(env.ResolveObjectPath(exp), env);
        }

        internal static MlObject ExecuteInternal(FunctionExpression exp, Environment env)
        {
            if (env.CurrentClassContextObject == null && !env.InStackFrame)
            {
                var argtypes = Util.ExtractTypesFromArglist(exp.Arguments, env);
                exp.RuntimeParsedArgs = argtypes;
                
                if (env.GlobalFunctions.Select(_ => (_.Value as MlObjectCallable).Func).Any(_ => _.Name == exp.Name && argtypes.ASequenceEqual((MlType[])_.RuntimeParsedArgs ?? (MlType[])(_.RuntimeParsedArgs = Util.ExtractTypesFromArglist(_.Arguments, env)))))
                    throw new RuntimeException(exp.Origin, $"A function with the name '{exp.Name}' and same parameter list has already been defined.");
                env.AddGlobalFunction(exp);
                //env.GlobalFunctions.AddLast(new MlStoredObject() { Value = new MlObjectCallable(env) { Func = exp }, Access = MlObjectAccess.GLOBAL, Type = CoreLib.Constants.TYPE_FUNC.GetNonArrayType() });
                return new MlObjectCallable(env) { Func = exp };
            }
            throw new RuntimeException(exp.Origin, $"A function cannot be declared here.");
        }

        internal static MlObject ExecuteInternal(DeclarationExpression exp, Environment env)
        {
            void Add(string name, MlObject mlobj)
            {
                if (env.InStackFrame)
                    env.CurrentStackFrame.Add(name, new MlStoredObject() { Value = mlobj, Access = MlObjectAccess.LOCAL, Type = Util.DatatypeExpressionToMlType(exp.Datatype, env), IsConst = exp.IsConst, IsStatic = exp.IsStatic });
                else if (env.CurrentClassContextObject != null)
                    env.CurrentClassContextObject.Attributes.Add(name, new MlStoredObject() { Value = mlobj, Access = exp.Modifier.ToStoredObjectAccess(), Type = Util.DatatypeExpressionToMlType(exp.Datatype, env), IsConst = exp.IsConst, IsStatic = exp.IsStatic });
                else
                    env.GlobalVariables.Add(name, new MlStoredObject() { Value = mlobj, Access = MlObjectAccess.GLOBAL, Type = Util.DatatypeExpressionToMlType(exp.Datatype, env), IsConst = exp.IsConst, IsStatic = exp.IsStatic });
            }

            /*void Remove(string name)
            {
                if (env.InStackFrame)
                    env.CurrentStackFrame.Remove(name);
                else if (env.CurrentClassContextObject != null)
                    env.CurrentClassContextObject.Attributes.Remove(name);
                else
                    env.GlobalVariables.Remove(name);
            }*/

            try
            {
                Expression datatype = exp.Datatype;
                if (datatype is BraceExpression brexp && brexp.Kind == Lexing.BraceType.SQUARE && brexp.Arguments.Length == 0)
                    datatype = brexp.Subject;
                env.ResolveObjectPath(datatype, true);
            }
            catch(RuntimeException ex)
            {
                throw new RuntimeException(ex.Origin, $"'{Util.DatatypeExpressionToMlType(exp.Datatype, env)}' is not a type or has not been defined.");
            }

            for (int i = 0; i < exp.Assignments.Length; i++)
            {
                Expression assignment = exp.Assignments[i];
                if (assignment is IdentifierExpression idexp)
                    Add(idexp.Identifier, MlObject.MlNull);
                else if (assignment is BinaryOperatorExpression boexp)
                {
                    if (boexp.Left is IdentifierExpression boidexp)
                    {
                        Add(boidexp.Identifier, MlObject.MlNull);
                        //try  {
                            ExecuteInternal(boexp, env, true);
                        /*}
                        catch (Exception ex)
                        {
                            Remove(boidexp.Identifier); // If we can't assign the value, then we undeclare (Important for the Shell-Program)
                            throw ex;
                        }*/
                    }
                    else
                        throw new RuntimeException(boexp.Origin, "Invalid identifier for declaration.");
                }
                else
                    throw new RuntimeException(assignment.Origin, "Invalid expression in declaration.");
            }

            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(ConstructorExpression exp, Environment env)
        {
            throw new InvalidOperationException("This is handled in MlObject");
        }

        internal static MlObject ExecuteInternal(CodeBlock exp, Environment env)
        {
            env.EnterStackFrame(true);
            for (int i = 0; i < exp.Expressions.Length; i++)
            {
                Expression e = exp.Expressions[i];
                e.Execute(env);
                if (env.CurrentStackFrame.DoesAny)
                    break;
            }
            env.ExitStackFrame(true);

            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(ClassExpression exp, Environment env)
        {
            if (env.InStackFrame)
                throw new RuntimeException(exp.Origin, "Cannot create a 'local class'");
            if (env.CurrentClassContextObject == null)
            {
                MlTypeObject mlto = new MlTypeObject(exp, env);
                if (env.GlobalTypes.Any(_ => (_.Value as MlTypeObject).GetClassTypeStr() == mlto.GetClassTypeStr()))
                    throw new RuntimeException(exp.Origin, "A class with the same name already exists");
                env.AddGlobalType(mlto);
                mlto.Init();
                return mlto;
                //env.GlobalTypes.Add(new MlStoredObject() { Value = mlto, Access = exp.Modifier.ToStoredObjectAccess(), IsConst = true, Type = CoreLib.Constants.TYPE_TYPE.GetNonArrayType() });
                //return mlto;
            }
            else
                throw new InvalidOperationException(); // Nested classes are handled somewhere else
        }

        public static MlObject CastObject(MlObject mo, MlType to, Environment env, bool implicitOnly)
        {
            if (mo.WorkingType == to)
                return mo;
            MlType workType = mo.WorkingType,
                underType = mo.UnderlyingType;

            //to = ConvertLocalTypeToGlobalType(env, to);

            /*if(mo.TypeObject != null
                && mo.TypeObject.GetClassTypeStr() == to.TypeStr
                && mo.WorkingType.TypeStr == mo.TypeObject.Class.Name)
            {
                to.TypeStr = mo.TypeObject.Class.Name;
            }*/

            if(underType == to || mo.IsTypeInHierarchy(to) || (mo is MlArray && to.IsArray))
            {
                MlObject newObj = mo.CastClone(env, to);
                //newObj.WorkingType = to;
                return newObj;
            }

            if (underType.IsNull())
                return mo;

            if (CoreLib.Constants.NativeOpTypes.InArray(underType.TypeStr)
                && CoreLib.Constants.NativeOpTypes.InArray(to.TypeStr))
            {
                MlObject m;
                if ((m = EnvironmentInitializer.NativeCastOperation(env, mo, to, implicitOnly)) != null)
                    return m;
            }

            //string hashStr = Util.GetHashString();
            var opFuncList = mo.FindOperatorFunctions("this");
            for (int i = 0; i < opFuncList.Count; i++)
            {
                OperatorFunctionExpression opFunc = opFuncList[i];
                if (opFunc != null && Util.DatatypeExpressionToMlType(opFunc.ReturnType, env) == to && (opFunc.OpModifier == OperatorFunctionExpression.OperatorModifiers.IMPLICIT 
                    || (!implicitOnly && opFunc.OpModifier == OperatorFunctionExpression.OperatorModifiers.EXPLICIT)))
                {
                    env.EnterClassContext(mo);
                    MlObject res = Call(opFunc, env);
                    TypeAssert(env, opFunc, res, to, true);
                    env.ExitClassContext();
                    return res;
                }
            }

            if (implicitOnly)
                throw new InternalRuntimeException($"Could not convert type '{underType}' to '{to}'");
            else
                throw new InternalRuntimeException("Found no matching cast-operator overload");
        }

        internal static MlObject ExecuteInternal(CastExpression exp, Environment env)
        {
            MlStoredObject mso = env.ResolveObjectPath(exp.Subject);
            try
            {
                return CastObject(mso.Value, Util.DatatypeExpressionToMlType(exp.CastingExpression, env), env, false);
            } catch(InternalRuntimeException ex)
            {
                throw new RuntimeException(exp.Origin, ex.Message);
            }
        }

        internal static MlObject ExecuteInternal(BraceExpression exp, Environment env)
        {
            switch (exp.Kind)
            {
                case Lexing.BraceType.ROUND:
                    var args = FetchObjects(exp.Arguments, env);
                    var types = args.Select(_ => _.WorkingType).ToArray();
                    List<MlStoredObject> objList = env.ResolveObjectsFromPath(exp.Subject, executeAttributeFunction: false);
                    List<MlObjectCallable> impList = new List<MlObjectCallable>();
                    List<MlObjectCallable> natList = new List<MlObjectCallable>();
                    
                    for (int i = 0; i < objList.Count; i++)
                    {
                        MlStoredObject obj = objList[i];
                        if (obj.Value is MlObjectCallable cobj)
                        {
                            env.EnterClassContext(cobj.From);
                            FunctionBaseExpression funcExp = cobj.Func;
                            if(!(cobj.Func is NativeFunction)) // Native functions don't have checking
                            {
                                // Caching the created arguments once it is called the first time
                                if(funcExp.RuntimeParsedArgs == null)
                                    funcExp.RuntimeParsedArgs = Util.ExtractTypesFromArglist(funcExp.Arguments, env);
                                MlType[] fargs = (MlType[])funcExp.RuntimeParsedArgs;

                                //var fargs = Util.ExtractTypesFromArglist(funcExp.Arguments, env);

                                if (funcExp.Arguments.Length != types.Length)
                                {
                                    env.ExitClassContext();
                                    continue;
                                }
                                if (!fargs.ASequenceEqual(types))
                                {
                                    bool br = false;
                                    for (int i1 = 0; i1 < fargs.Length; i1++)
                                        if (!Util.CanImplicitTypeConvert(args[i1], fargs[i1], env))
                                        {
                                            br = true;
                                            break;
                                        }
                                    if (!br)
                                        impList.Add(cobj); // add it to the "implicit list"
                                    env.ExitClassContext();
                                    continue; // but continue because we may find a better match
                                }
                            } else if(!(cobj.Func is ConstructorExpression)) // the only other possibility now is it being a constructor call, in which case we proceed
                            {
                                natList.Add(cobj);
                                env.ExitClassContext();
                                continue; // Prioritize Native Functions the least
                            }
                            //env.ExitClassContext();
                            //env.EnterClassContext(cobj.From);
                            MlObject res = Call(cobj, env, args);
                            env.ExitClassContext();
                            return res;
                        }
                    }
                    MlObject r = null;

                    bool imp(MlType[] expArglist)
                    {
                        //var expArglist = Util.ExtractTypesFromArglist(fexp.Arguments);
                        if (expArglist.Length != args.Length)
                            return false;
                        bool br = false;
                        for (int i = 0; i < args.Length; i++)
                            if (!Util.CanImplicitTypeConvert(args[i], expArglist[i], env))
                            {
                                br = true;
                                break;
                            }
                        if (br)
                            return false;
                        return true;
                    }

                    bool procFunc(MlObjectCallable cobj, bool nat)
                    {
                        if(!nat) env.EnterClassContext(cobj.From);
                        if (!nat)
                        {
                            if (!imp(/*Util.ExtractTypesFromArglist(cobj.Func.Arguments, env)*/(MlType[])cobj.Func.RuntimeParsedArgs))
                            {
                                env.ExitClassContext();
                                return false;
                            }
                                
                        }
                        /*if (!nat) env.ExitClassContext();
                        if(!nat) env.EnterClassContext(cobj.From);*/
                        r = Call(cobj, env, args);
                        if(!nat)
                            env.ExitClassContext();
                        return true;
                    }

                    for (int i1 = 0; i1 < impList.Count; i1++)
                    {
                        MlObjectCallable cobj = impList[i1];
                        if (procFunc(cobj, false))
                            return r;
                    }

                    for (int i1 = 0; i1 < natList.Count; i1++)
                    {
                        MlObjectCallable cobj = natList[i1];
                        if (procFunc(cobj, true))
                            return r;
                    }

                    // Functionality for calling-operators in custom types (get-only)
                    // e.g. void get operator this()
                    // then you can use () on the object to "call" it (executing this method)
                    MlStoredObject mltoFirst = objList[0];
                    List<OperatorFunctionExpression> nonCOpList = mltoFirst.Value.FindOperatorFunctions("this");
                    foreach (OperatorFunctionExpression opexp in nonCOpList)
                    {
                        if (opexp.BraceType == Lexing.BraceType.ROUND &&
                                ((opexp.OpModifier == OperatorFunctionExpression.OperatorModifiers.NORMAL))
                                && opexp.Arguments.Length == args.Length)
                        {
                            env.EnterClassContext(mltoFirst.Value);

                            if (opexp.RuntimeParsedArgs == null)
                                opexp.RuntimeParsedArgs = Util.ExtractTypesFromArglist(opexp.Arguments, env);


                            if (!imp(/*Util.ExtractTypesFromArglist(opexp.Arguments, env)*/(MlType[])opexp.RuntimeParsedArgs))
                            {
                                env.ExitClassContext(); 
                                continue;
                            }
                            //env.ExitClassContext();
                            //env.EnterClassContext(mltoFirst.Value);
                            r = Call(opexp, env, args);
                            env.ExitClassContext();
                            return r;
                        }
                    }

                    throw new RuntimeException(exp.Origin, $"Couldn't find matching function definition for {exp.Subject} and arguments [{string.Join<MlType>(",", types)}]");
                case Lexing.BraceType.CURLY:
                    throw new RuntimeException(exp.Origin, "Invalid curly brace expression");
                case Lexing.BraceType.SQUARE: // for now only arrays support this
                    return ExtractValue(env.ResolveObjectPath(exp), env);
                default:
                    throw new NotImplementedException();
            }
        }

        internal static MlObject ExecuteInternal(BinaryOperatorExpression exp, Environment env, bool allowConstAssignment = false)
        {
            if(Modlang.Util.IsAssignmentOperator(exp.Operator)) // Assigment
            {
                if(exp.Operator.Length == 1) // just assignment
                {
                    // This is so that 'operator this[...]' inside classes works in "set" mode
                    if(exp.Left is BraceExpression brexp && brexp.Kind == Lexing.BraceType.SQUARE) 
                    {
                        MlStoredObject mlto = env.ResolveObjectPath(brexp.Subject);

                        if(!(mlto.Value is MlArray))// Arrays are handled internally already (kinda a shit system i know but whatever THIS IS MY PROJECT)
                        {
                            var args1 = FetchObjects(brexp.Arguments, env);
                            MlObject[] args = new MlObject[args1.Length + 1];
                            MlStoredObject mltoRight = env.ResolveObjectPath(exp.Right);

                            MlObject objl = mlto.Value;

                            args[0] = mltoRight.Value;
                            args1.CopyTo(args, 1);
                            env.EnterClassContext(objl);
                            List<OperatorFunctionExpression> nonCastingThisOperators = objl.FindOperatorFunctions("this");
                            foreach (OperatorFunctionExpression opexp in nonCastingThisOperators)
                            {
                                if (opexp.BraceType == Lexing.BraceType.SQUARE &&
                                    ((opexp.OpModifier == OperatorFunctionExpression.OperatorModifiers.SET))
                                    && opexp.Arguments.Length == args.Length)
                                {
                                    if (opexp.RuntimeParsedArgs == null)
                                        opexp.RuntimeParsedArgs = Util.ExtractTypesFromArglist(opexp.Arguments, env);
                                    MlType[] expArglist = (MlType[])opexp.RuntimeParsedArgs;
                                    //var expArglist = Util.ExtractTypesFromArglist(opexp.Arguments, env);
                                    bool br = false;
                                    for (int i = 0; i < args.Length; i++)
                                        if (!Util.CanImplicitTypeConvert(args[i], expArglist[i], env))
                                        {
                                            br = true;
                                            break;
                                        }
                                    if (br)
                                        continue;
                                    //env.ExitClassContext(true);
                                    //env.EnterClassContext(objl);
                                    MlObject re = Call(opexp, env, args);
                                    env.ExitClassContext();
                                    return re;
                                }
                            }

                            throw new RuntimeException(brexp.Origin, $"Couldn't find matching indexing operator definition in '{objl.UnderlyingType}' with arguments [{string.Join(", ", args.Select(_ => _.WorkingType.ToString()))}]");
                        }
                    }

                    {
                        
                        List<MlStoredObject> msos = env.ResolveObjectsFromPath(exp.Left, executeAttributeFunction: false);
                        
                        bool allFalse = true;
                        if(msos.Count > 0)
                        {
                            foreach (MlStoredObject mso in msos)
                                if (mso.Value is MlObjectCallable mloc && mloc.Func is AttributeFunctionExpression afex)
                                {
                                    allFalse = false;
                                    if (afex.IsSetter)
                                    {
                                        // Dirty fix (make it a call on the fly)
                                        return new BraceExpression(exp.Left, new Expression[] { exp.Right }, Lexing.BraceType.ROUND).Execute(env);//Call(afex, env, new MlObject[] { right });
                                    }
                                }
                            if (allFalse)
                            {
                                MlObject right = exp.Right.Execute(env);
                                env.SetValue(msos[0], right, allowConstAssignment);
                                return right;
                            }
                        }

                        //env.SetValue(exp.Left, right, allowConstAssignment);
                        //left.ChangeValue(right, env, allowConstAssignment);
                        throw new RuntimeException(exp.Left.Origin, $"Couldn't resolve {exp.Left}");
                    }
                }
                else
                {
                    string opPart = exp.Operator.Substring(0, exp.Operator.Length - 1);
                    //MlObject val = ExecuteInternal(new BinaryOperatorExpression(exp.Left, exp.Right, opPart), env);
                    //env.SetValue(exp.Left, val, allowConstAssignment);
                    //return val;

                    return new BinaryOperatorExpression(exp.Left, new BinaryOperatorExpression(exp.Left, exp.Right, opPart), Constants.AssignmentOperatorStr).Execute(env);
                }
            }

            if(exp.Operator == ".")
            {
                return ExtractValue(env.ResolveObjectPath(exp), env);
            }

            MlStoredObject mso1 = env.ResolveObjectPath(exp.Left);
            MlStoredObject oth = env.ResolveObjectPath(exp.Right);
            if ((mso1.Value.UnderlyingType.IsNull() || oth.Value.UnderlyingType.IsNull() ||
                CoreLib.Constants.NativeOpTypes.InArray(mso1.Value.UnderlyingType.TypeStr)
                ) && !mso1.Value.UnderlyingType.IsArray && !oth.Value.UnderlyingType.IsArray)
            {
                // Functionality so that the && and || logical boolean operators don't execute the second one
                // If the condition cannot even change after the first one
                bool isLogAnd = exp.Operator == CoreLib.Constants.SPECIAL_OP_LOG_AND,
                    isLogOr = exp.Operator == CoreLib.Constants.SPECIAL_OP_LOG_OR;
                if ((isLogAnd || isLogOr)
                    && mso1.Value.NativeValue.Value != null && mso1.Value.NativeValue.Value is bool boolean &&
                    ((isLogAnd && !boolean) || (isLogOr && boolean)))
                {

                    return mso1.Value; // Just return this boolean value (no need to copy it)
                }

                MlObject g;
                
                if ((oth.Value.UnderlyingType.IsNull() || CoreLib.Constants.NativeOpTypes.InArray(oth.Value.UnderlyingType.TypeStr)) && // Only if the other is also of native type
                    (g = EnvironmentInitializer.NativeOperatorOperation(env, exp.Operator, false, false, mso1.Value, oth.Value)) != null)
                    return g;
            }

            MlObject OpFind(MlStoredObject mso, Expression other)
            {
                List<OperatorFunctionExpression> opFuncList = mso.Value.FindOperatorFunctions(exp.Operator);
                List<NativeOperatorFunction> natOpList = new List<NativeOperatorFunction>();
                var args = FetchObjects(new[] { other }, env);
                foreach (OperatorFunctionExpression opFunc in opFuncList)
                {
                    if(opFunc is NativeOperatorFunction nof && nof.OpModifier == OperatorFunctionExpression.OperatorModifiers.NORMAL)
                    {
                        natOpList.Add(nof);
                        continue;
                    }
                    if (opFunc != null && opFunc.Arguments.Length == 1 && opFunc.OpModifier == OperatorFunctionExpression.OperatorModifiers.NORMAL)
                    {
                        if(opFunc.RuntimeParsedArgs == null)
                            opFunc.RuntimeParsedArgs = Util.ExtractTypesFromArglist(opFunc.Arguments, env);
                        MlType[] expectTypes = (MlType[])opFunc.RuntimeParsedArgs;
                        //var expectTypes = Util.ExtractTypesFromArglist(opFunc.Arguments, env);
                        bool br = false;
                        for (int i = 0; i < args.Length; i++)
                        {
                            MlType expect = expectTypes[i];
                            if (!Util.TypeCheck(args[i], expect) && !Util.CanImplicitTypeConvert(args[i], expect, env))
                            {
                                br = true;
                                break;
                            }
                        }

                        if (br)
                            continue;
                        env.EnterClassContext(mso.Value);
                        MlObject res = Call(opFunc, env, args);
                        env.ExitClassContext();
                        return res;
                    }
                }
                if(natOpList.Count > 0)
                {
                    env.EnterClassContext(mso.Value);
                    MlObject res = Call(natOpList[0], env, args);
                    env.ExitClassContext();
                    return res;
                }
                    
                return null;
            }

            
            MlObject r;
            if ((r = OpFind(mso1, exp.Right)) != null)
                return r;
            MlStoredObject mso2;
            // switching the operands is not a good idea ...
            if ((r = OpFind(mso2 = env.ResolveObjectPath(exp.Right), exp.Left)) != null)
                return r;

            throw new RuntimeException(exp.OpExp == null ? exp.Origin : exp.OpExp.Origin, $"Found no matching overloaded binary operator for '{exp.Operator}' in type '{mso1.Value.UnderlyingType}' or in type '{mso2.Value.UnderlyingType}'");
        }
        #endregion

        #region Literals

        internal static MlObject ExecuteInternal(BoolLiteralExpression exp, Environment env)
        {
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_BOOL);
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_BOOL), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
        }

        internal static MlObject ExecuteInternal(CharLiteralExpression exp, Environment env)
        {
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_CHAR), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_CHAR);
        }

        internal static MlObject ExecuteInternal(DoubleLiteralExpression exp, Environment env)
        {
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_DOUBLE), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_DOUBLE);
        }

        internal static MlObject ExecuteInternal(FloatLiteralExpression exp, Environment env)
        {
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_FLOAT), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_FLOAT);
        }

        internal static MlObject ExecuteInternal(IntegerLiteralExpression exp, Environment env)
        {
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_INT), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_INT);
        }

        internal static MlObject ExecuteInternal(LongIntegerLiteralExpression exp, Environment env)
        {
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_LONG), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_LONG);
        }

        internal static MlObject ExecuteInternal(OperatorLiteralExpression exp, Environment env)
        {
            throw new InvalidOperationException();
        }

        internal static MlObject ExecuteInternal(StringLiteralExpression exp, Environment env)
        {
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_STRING);
            /*
            MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_STRING), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
        }

        internal static MlObject ExecuteInternal(UIntegerLiteralExpression exp, Environment env)
        {
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_UINT);
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_UINT), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
        }

        internal static MlObject ExecuteInternal(ULongIntegerLiteralExpression exp, Environment env)
        {
            return EnvironmentInitializer.CreateNativeObject(env, exp.Value, CoreLib.Constants.TYPE_ULONG);
            /*MlObject res = MlObject.FromClass(env.FindClass(CoreLib.Constants.TYPE_ULONG), env);
            res.NativeValue.Value = exp.Value;
            return res;*/
        }

        #endregion

        #region ControlStructures

        internal static MlObject ExecuteInternal(ForExpression exp, Environment env)
        {
            env.EnterStackFrame(true);
            foreach (Expression sexp in exp.StartingExpressions)
                sexp?.Execute(env);

            while(true)
            {
                MlObject obj = exp.Condition?.Execute(env);
                {
                    if(obj != null && !(bool)CastObject(obj, CoreLib.Constants.TYPE_BOOL.GetNonArrayType(), env, true).NativeValue.Value)
                    {
                        break;
                    }
                    bool cb = exp.Inner is CodeBlock;
                    if(!cb) // Optimization: Only enter new stackframe if the inner block is not a codeblock already, which enters no matter what
                        env.EnterStackFrame(true);
                    exp.Inner.Execute(env);
                    if(!cb)
                        env.ExitStackFrame(true);

                    if (env.CurrentStackFrame.DoesBreak)
                    {
                        env.CurrentStackFrame.DoesBreak = false;
                        break;
                    }
                    if (env.CurrentStackFrame.DoesContinue)
                    {
                        env.CurrentStackFrame.DoesContinue = false;
                    }
                    if(env.CurrentStackFrame.DoesReturn)
                    {
                        break;
                    }

                    foreach (Expression rexp in exp.RoundExpressions)
                        rexp?.Execute(env);
                }
            }
            env.ExitStackFrame(true);
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(IfExpression exp, Environment env)
        {
            MlObject obj = exp.Condition.Execute(env);
            {
                if ((bool)CastObject(obj, CoreLib.Constants.TYPE_BOOL.GetNonArrayType(), env, true).NativeValue.Value)
                {
                    env.EnterStackFrame(true);
                    exp.Inner.Execute(env);
                    env.ExitStackFrame(true);
                }
                else if (exp.ElseInner != null)
                {
                    env.EnterStackFrame(true);
                    exp.ElseInner.Execute(env);
                    env.ExitStackFrame(true);
                }
            }
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(SwitchExpression exp, Environment env)
        {
            MlObject subj = exp.Subject.Execute(env);
            bool enteredCase = false;
            bool breakAll = false;
            env.EnterStackFrame(true);
            foreach(SwitchCase c in exp.Cases)
            {
                if (c.IsDefault)
                    continue;
                if(!enteredCase && !c.IsDefault)
                {
                    MlObject match = c.Match.Execute(env);
                    env.EnterClassContext(subj);
                    MlObject obj = Call(subj.FindFunctions(CoreLib.Constants.OBJ_FUNC_EQUALS)[0], env, match);
                    env.ExitClassContext();
                    {
                        if ((bool)CastObject(obj, CoreLib.Constants.TYPE_BOOL.GetNonArrayType(), env, true).NativeValue.Value)
                        {
                            enteredCase = true;
                            foreach (Expression iexp in c.Inner)
                            {
                                if(iexp is BreakExpression)
                                {
                                    breakAll = true;
                                    break;
                                }
                                iexp.Execute(env);
                            }
                            if (breakAll)
                                break;
                        }
                    }
                    continue;
                }
                foreach (Expression iexp in c.Inner)
                {
                    if (iexp is BreakExpression)
                    {
                        breakAll = true;
                        break;
                    }
                    iexp.Execute(env);
                }
                if (breakAll)
                    break;
            }

            if(!enteredCase)
                if(exp.Cases.Any(_ => _.IsDefault))
                    foreach (Expression iexp in exp.Cases.First(_ => _.IsDefault).Inner)
                    {
                        if (iexp is BreakExpression)
                        {
                            breakAll = true;
                            break;
                        }
                        iexp.Execute(env);
                    }
            env.ExitStackFrame(true);
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        internal static MlObject ExecuteInternal(WhileExpression exp, Environment env)
        {
            while (true)
            {
                env.EnterStackFrame(true);
                MlObject obj = exp.Condition.Execute(env);
                {
                    if (!(bool)CastObject(obj, CoreLib.Constants.TYPE_BOOL.GetNonArrayType(), env, true).NativeValue.Value)
                    {
                        break;
                    }
                    exp.Inner.Execute(env);

                    if (env.CurrentStackFrame.DoesBreak)
                    {
                        env.CurrentStackFrame.DoesBreak = false;
                        break;
                    }
                    if (env.CurrentStackFrame.DoesContinue)
                    {
                        env.CurrentStackFrame.DoesContinue = false;
                    }
                }
                env.ExitStackFrame(true);
            }
            return env.ExecutionOptions.PassOutMlNull ? MlObject.MlNull : null;
        }

        #endregion
    }
}
