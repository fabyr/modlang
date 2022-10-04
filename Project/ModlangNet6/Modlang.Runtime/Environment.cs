using Modlang.Abstract;
using Modlang.Parsing;
using Modlang.Runtime.CoreLib;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modlang.Runtime
{
    public class Environment
    {
        public class EnvironmentExecutionOptions
        {
            public bool NoTypeAssert = false;
            public bool PassOutMlNull = false;
            public bool InitializeArraysWithMlNull = false;
            public bool InitializeNativeTypeArraysWithDefault = true;
        }

        internal static ExpressionExecutor GlobalExpressionExecutor = new ExpressionExecutor();

        public class StackFrame
        {
            public bool DoesBreak = false;
            public bool DoesContinue = false;
            public bool DoesReturn = false;

            public bool DoesAny => DoesReturn
                || DoesBreak
                || DoesContinue;

            public MlObject ReturnValue = null;

            public void Add(string key, MlStoredObject value) => VariableStack.Last.Value.Add(key, value);
            public void Remove(string key) => VariableStack.Last.Value.Remove(key);

            public LinkedList<Dictionary<string, MlStoredObject>> VariableStack = new LinkedList<Dictionary<string, MlStoredObject>>();
        }

        public int AllowCallCtor = 0;

        public Dictionary<string, MlStoredObject> NativeFunctions = new Dictionary<string, MlStoredObject>();

        public EnvironmentExecutionOptions ExecutionOptions = new EnvironmentExecutionOptions();

        public Dictionary<string, MlStoredObject> GlobalVariables = new Dictionary<string, MlStoredObject>();
        public LinkedList<MlStoredObject> GlobalFunctions = new LinkedList<MlStoredObject>();
        public List<MlStoredObject> GlobalTypes = new List<MlStoredObject>();
        public Dictionary<string, MlStoredObject> NativeTypes = new Dictionary<string, MlStoredObject>();

        public Stack<StackFrame> StackFrames = new Stack<StackFrame>();

        private readonly Stack<MlObject> _contextStack = new Stack<MlObject>();

        public MlObject CurrentClassContextObject = null;

        public StackFrame CurrentStackFrame = null;

        public bool InStackFrame = false;

        public MlTypeObject AddGlobalType(MlTypeObject mlto)
        {
            MlStoredObject mso = new MlStoredObject() { Value = mlto, Access = mlto.Class.Modifier.ToStoredObjectAccess(), IsConst = true, Type = CoreLib.Constants.TYPE_TYPE.GetNonArrayType() };
            if (CoreLib.Constants.NativeTypes.InArray(mlto.Class.Name))
            {
                NativeTypes.Add(mlto.Class.Name, mso);
            }
            else
            {
                GlobalTypes.Add(mso);
            }
            return mlto;
        }

        public MlTypeObject AddGlobalType(ClassExpression exp)
        {
            MlTypeObject mlto = new MlTypeObject(exp, this);
            MlStoredObject mso = new MlStoredObject() { Value = mlto, Access = exp.Modifier.ToStoredObjectAccess(), IsConst = true, Type = CoreLib.Constants.TYPE_TYPE.GetNonArrayType() };
            if (CoreLib.Constants.NativeTypes.InArray(exp.Name))
            {
                NativeTypes.Add(exp.Name, mso); 
            } else
            {
                GlobalTypes.Add(mso);
            }
            return mlto;
        }

        public MlStoredObject AddGlobalFunction(FunctionExpression exp)
        {
            MlStoredObject mso = new MlStoredObject() { Value = new MlObjectCallable(this) { Func = exp }, Access = MlObjectAccess.GLOBAL, Type = CoreLib.Constants.TYPE_FUNC.GetNonArrayType() };
            GlobalFunctions.AddLast(mso);
            return mso;
        }

        public Environment()
        { }

        public void Init(EnvironmentInitializer init)
        {
            SfInit();
            init.Init(this);
        }

        private void SfInit()
        {
            StackFrame sf = new StackFrame();
            StackFrames.Push(sf);
            CurrentStackFrame = sf;
            CurrentStackFrame.VariableStack.AddLast(GlobalVariables);
        }

        public void TraverseFunctions(string name, Action<MlStoredObject> action)
        {
            var current = GlobalFunctions.First;
            while (current != null)
            {
                FunctionExpression fexp = (current.Value.Value as MlObjectCallable).Func as FunctionExpression;
                if (fexp.Name == name)
                    action(current.Value);
                current = current.Next;
            }
        }

        public FunctionExpression FindFunction(string name, bool includeNative = false)
        {
            if (includeNative)
            {
                return NativeFunctions.ContainsKey(name) ? ((NativeFunctions[name].Value as MlObjectCallable).Func as FunctionExpression) : null;
            }
            var current = GlobalFunctions.First;
            while(current != null)
            {
                current = current.Next;
                FunctionExpression fexp = (current.Value.Value as MlObjectCallable).Func as FunctionExpression;
                if (fexp.Name == name)
                    return fexp;
            }

            return null;
        }

        public MlTypeObject FindNativeClass(string name)
        {
            /*for (int i = 0; i < NativeTypes.Count; i++)
            {
                MlTypeObject mlto = (NativeTypes[i].Value as MlTypeObject);
                if (mlto.GetClassTypeStr() == name)
                    return mlto;
            }*/
            //if (NativeTypes.ContainsKey(name)) // We don't need to check, if it is missing, core.mdl is wrong
                return NativeTypes[name].Value as MlTypeObject;

            //return null;
        }

        public MlTypeObject FindClass(string name)
        {
            for (int i = 0; i < GlobalTypes.Count; i++)
            {
                MlTypeObject mlto = (GlobalTypes[i].Value as MlTypeObject);
                if (mlto.GetClassTypeStr() == name)
                    return mlto;
            }

            return null;
        }

        public bool CanAccess(MlObject ml, MlObjectAccess access, MlObject CurrentClassContextObject)
        {
            switch (access)
            {
                case MlObjectAccess.CLASS_CONTEXT_PRIVATE:
                    bool a = true;
                    if(CurrentClassContextObject is MlTypeObject mlto)
                    {
                        a = ml.WorkingType.TypeStr != mlto.GetClassTypeStr();
                    }
                    if (CurrentClassContextObject == null || (ml.WorkingType != CurrentClassContextObject.WorkingType && a))
                        return false;
                    break;
                case MlObjectAccess.CLASS_CONTEXT_PROTECTED:
                    if (CurrentClassContextObject == null || !ml.IsTypeInHierarchy(CurrentClassContextObject.WorkingType)) // TODO: Check if this is correct
                        return false;
                    break;
                default:
                    break;
            }
            return true;
        }

        /*public List<MlStoredObject> ResolveObjectsFromPath(Expression exp, bool typesOnly = false)
        {
            return ResolveObjectsFromPathCore(exp, CurrentClassContextObjects[0], typesOnly, true);

            // This once-feature is now obsolete
            /*if (CurrentClassContextObjects[1] == null)
                return ResolveObjectsFromPathCore(exp, CurrentClassContextObjects[0], typesOnly, true);
            List<MlStoredObject> list = new List<MlStoredObject>();
            for(int i = 0; i < 2; i++)
            {
                var l = ResolveObjectsFromPathCore(exp, CurrentClassContextObjects[i], typesOnly, false);
                if (l == null)
                    continue;
                list.AddRange(l);
            }
            if (list.Count == 0)
                throw new RuntimeException(exp.Origin, $"Could not resolve '{exp}'");
            return list;*
        }*/

        public List<MlStoredObject> ResolveObjectsFromPath(Expression exp, bool typesOnly = false, bool doThrow = true, bool executeAttributeFunction = true)
        {
            List<MlStoredObject> res = new List<MlStoredObject>();
            if (exp is IdentifierExpression idexp) // Lookup
            {
                string name = idexp.Identifier;

                if(name.StartsWith(CoreLib.Constants.FUNC_N_PRFIX))
                {
                    res.Add(NativeFunctions[name]);
                    return res;
                }

                if (CoreLib.Constants.NativeTypes.InArray(name))
                {
                    //if(NativeTypes.ContainsKey(name)) // We don't need to check, if it is missing, core.mdl is wrong
                    res.Add(NativeTypes[name]);
                    return res;
                }

                if (CurrentClassContextObject != null &&
                AllowCallCtor > 0 &&
                name == CoreLib.Constants.FUNC_OTHER_CTOR)
                {
                    for (int i = 0; i < CurrentClassContextObject.TypeObject.Class.Constructors.Length; i++)
                    {
                        ConstructorExpression ctorexp = CurrentClassContextObject.TypeObject.Class.Constructors[i];
                        res.Add(new MlStoredObject() { Value = new MlObjectCallable(this) { Func = ctorexp, From = CurrentClassContextObject }, Access = ctorexp.Modifier.ToStoredObjectAccess(), Type = MlType.FUNC });
                    }
                    return res;
                }

                if(CurrentClassContextObject != null && (CurrentClassContextObject is MlTypeObject cccomlto && name == cccomlto.Class.Name))
                {
                    res.Add(new MlStoredObject() { Value = cccomlto, Access = MlObjectAccess.LOCAL, IsConst = true, Type = cccomlto.WorkingType });
                    return res;
                }

                if (CurrentClassContextObject != null && (CurrentClassContextObject.TypeObject != null &&
                    name == CurrentClassContextObject.TypeObject.Class.Name))
                {
                    res.Add(new MlStoredObject() { Value = CurrentClassContextObject.TypeObject, Access = MlObjectAccess.LOCAL, IsConst = true, Type = CurrentClassContextObject.TypeObject.WorkingType });
                    return res;
                }

                if (!typesOnly)
                {
                    if (name == "null")
                    {
                        res.Add(MlObject.MlNullStored);
                        return res;
                    }

                    if (CurrentClassContextObject != null && name == CoreLib.Constants.CLASS_PROPERTY_NATIVE_VALUE)
                    {
                        res.Add(CurrentClassContextObject.NativeValue);
                        return res;
                    }

                    if (CurrentClassContextObject != null && (name == CoreLib.Constants.CLASS_PROPERTY_TYPE))
                    {
                        //MlObject resobj = EnvironmentInitializer.CreateNativeObject(this, CurrentClassContextObject.WorkingType.ToString(), CoreLib.Constants.TYPE_STRING);// = MlObject.FromNativeClass(FindClass(CoreLib.Constants.TYPE_STRING), this);
                        //resobj.NativeValue.Value = CurrentClassContextObject.WorkingType.ToString();
                        //res.Add(new MlStoredObject() { Value = resobj, Access = MlObjectAccess.LOCAL, IsConst = true, Type = resobj.WorkingType });
                        //
                        res.Add(new MlStoredObject() { Value = CurrentClassContextObject.TypeObject, Access = MlObjectAccess.LOCAL, IsConst = true, Type = CurrentClassContextObject.TypeObject.WorkingType });
                        return res;
                    }

                    if (name == "this")
                        if (CurrentClassContextObject == null)
                            throw new RuntimeException(idexp.Origin, "The Keyword 'this' is invalid outside of a class context.");
                        else
                        {
                            res.Add(new MlStoredObject() { Value = CurrentClassContextObject, Access = MlObjectAccess.LOCAL, Type = CurrentClassContextObject.WorkingType, IsConst = true }); // This kinda is a workaround
                            return res;
                        }

                    var currentNode = CurrentStackFrame.VariableStack.Last;
                    while ((currentNode != null))
                    {
                        if (currentNode.Value.ContainsKey(name))
                        {
                            res.Add(currentNode.Value[name]);
                            return res;
                        }
                        currentNode = currentNode.Previous;
                    }

                    if (CurrentClassContextObject != null)
                    {
                        MlStoredObject attrib;
                        if ((attrib = CurrentClassContextObject.FindAttribute(name)) != null)
                        {
                            res.Add(attrib);
                            return res;
                        }

                        var funcList = CurrentClassContextObject.FindFunctions(name);
                        bool one = false;
                        foreach (FunctionExpression fexp in funcList)
                        {
                            one = true;
                            if (executeAttributeFunction && fexp is AttributeFunctionExpression afex && !afex.IsSetter)
                            {
                                MlObject ret = new BraceExpression(exp, new Expression[0], Lexing.BraceType.ROUND).Execute(this);//;Execution.Call(afex, this);
                                res.Add(new MlStoredObject() { Value = ret, Access = MlObjectAccess.LOCAL, Type = ret.UnderlyingType });
                            }
                            else
                                res.Add(new MlStoredObject() { Value = new MlObjectCallable(this) { Func = fexp, From = CurrentClassContextObject }, Access = fexp.Modifier.ToStoredObjectAccess(), Type = MlType.FUNC });
                        }
                        if (one)
                            return res;
                    }

                    if (GlobalVariables.ContainsKey(name))
                    {
                        res.Add(GlobalVariables[name]);
                        return res;
                    }
                    {
                        TraverseFunctions(name, (fexpml) =>
                        {
                            res.Add(fexpml);
                        });
                        if (res.Count != 0)
                            return res;
                    }
                }

                foreach(MlStoredObject globtype in GlobalTypes)
                {
                    if (((MlTypeObject)globtype.Value).Class.Name == name)
                    {
                        res.Add(globtype);
                        return res;
                    }
                }

                if (CurrentClassContextObject != null && CurrentClassContextObject.TypeObject != null)
                {
                    MlStoredObject mlto;
                    if((mlto = CurrentClassContextObject.TypeObject.FindNestedType(name)) != null)
                    {
                        res.Add(mlto);
                        return res;
                    }
                }
                /*foreach (MlStoredObject ntype in NativeTypes)
                {
                    if (((MlTypeObject)ntype.Value).Class.Name == name)
                    {
                        res.Add(ntype);
                        return res;
                    }
                }*/

                if (doThrow)
                    throw new RuntimeException(idexp.Origin, $"Cannot find identifier '{name}'");
                else
                    return null;
            }
            if (exp is BinaryOperatorExpression boexp && boexp.Operator == ".")
            {
                if (!(boexp.Right is IdentifierExpression))
                    throw new RuntimeException(boexp.Right.Origin, "Invalid identifying expression");
                MlStoredObject left = ResolveObjectPath(boexp.Left);
                string rightIdent = (boexp.Right as IdentifierExpression).Identifier;

                bool CanAccessL(MlObjectAccess access)
                {
                    return this.CanAccess(left.Value, access, CurrentClassContextObject);
                }

                if(!typesOnly)
                {
                    if (rightIdent == CoreLib.Constants.CLASS_PROPERTY_NATIVE_VALUE)
                    {
                        if (CanAccessL(left.Value.NativeValue.Access))
                        {
                            res.Add(left.Value.NativeValue);
                            return res;
                        }
                    }

                    if (rightIdent == CoreLib.Constants.CLASS_PROPERTY_TYPE)
                    {
                        if (CanAccessL(MlObjectAccess.CLASS_CONTEXT_PRIVATE))
                        {
                            MlObject resobj = MlObject.FromClass(FindClass(CoreLib.Constants.TYPE_STRING), this);
                            resobj.NativeValue.Value = CurrentClassContextObject.WorkingType.ToString();
                            res.Add(new MlStoredObject() { Value = resobj, Access = MlObjectAccess.LOCAL, IsConst = true, Type = resobj.WorkingType });
                            return res;
                        }
                    }

                    MlStoredObject attrib;
                    if ((attrib = left.Value.FindAttribute(rightIdent)) != null)
                    {
                        if (CanAccessL(attrib.Access))
                        {
                            res.Add(attrib);
                            return res;
                        }
                    }

                    var foundFunc = left.Value.FindFunctions(rightIdent);
                    bool one = false;
                    foreach (FunctionExpression fexp in foundFunc)
                    {
                        if (CanAccessL(fexp.Modifier.ToStoredObjectAccess()))
                        {
                            one = true;
                            //if (Util.ExtractTypesFromArglist(fexp.Arguments).SequenceEqual(funcArgTypes))
                            if (executeAttributeFunction && fexp is AttributeFunctionExpression afex && !afex.IsSetter)
                            {
                                MlObject ret = new BraceExpression(boexp, new Expression[0], Lexing.BraceType.ROUND).Execute(this);//;Execution.Call(afex, this);
                                res.Add(new MlStoredObject() { Value = ret, Access = MlObjectAccess.LOCAL, Type = ret.UnderlyingType });
                            }
                            else
                                res.Add(new MlStoredObject() { Value = new MlObjectCallable(this) { Func = fexp, From = left.Value }, Access = fexp.Modifier.ToStoredObjectAccess(), Type = MlType.FUNC });
                        }
                    }
                    if (one)
                        return res;
                }

                if(left.Value is MlTypeObject mlto)
                {
                    MlStoredObject mso;
                    if((mso = mlto.FindNestedType(rightIdent)) != null && CanAccessL(mso.Access))
                    {
                        res.Add(mso);
                        return res;
                    }
                }
                if (doThrow)
                    throw new RuntimeException(boexp.Right.Origin, $"Cannot find identifier '{rightIdent}'");
                else
                    return null;
            }
            if(exp is BraceExpression brexp && brexp.Kind == Lexing.BraceType.SQUARE)
            {
                
                MlStoredObject mso = ResolveObjectPath(brexp.Subject);
                
                if (mso.Value is MlArray ar)
                {
                    if (brexp.Arguments.Length != 1) throw new RuntimeException(brexp.Origin, "Invalid argument count. Expected exactly 1 for array-indexing operators.");
                    MlObject idx = brexp.Arguments[0].Execute(this);
                    try
                    {
                        idx = Execution.CastObject(idx, CoreLib.Constants.TYPE_LONG.GetNonArrayType(), this, true);
                    } catch (InternalRuntimeException ex) 
                    {
                        throw new RuntimeException(brexp.Arguments[0].Origin, ex.Message);
                    }
                    long nat = (long)idx.NativeValue.Value;
                    if (nat < 0 || ar.Collection.Length <= nat)
                        throw new RuntimeException(brexp.Arguments[0].Origin, $"The Array index is too small or too big. Array Size: {ar.Collection.Length}, Index: {nat}");
                    res.Add(ar.Collection[nat]);
                    return res;
                } else
                {
                    // Functionality for indexing-operators in custom types (get functionality for indexing operators, set functionality is in BinaryOperatorExpression-Execution Method)
                    // e.g. void get operator this[int value]
                    // then you can use () on the object to "call" it (executing this method)
                    List<OperatorFunctionExpression> nonCOpList = mso.Value.FindOperatorFunctions("this");
                    var args = Execution.FetchObjects(brexp.Arguments, this);
                    //EnterClassContext(mso.Value, true);
                    foreach (OperatorFunctionExpression opexp in nonCOpList)
                    {
                        if (opexp.BraceType == Lexing.BraceType.SQUARE &&
                                ((opexp.OpModifier == OperatorFunctionExpression.OperatorModifiers.GET))
                                && opexp.Arguments.Length == args.Length)
                        {
                            if (!CanAccess(mso.Value, opexp.Modifier.ToStoredObjectAccess(), CurrentClassContextObject))
                                continue;
                            
                            var expArglist = Util.ExtractTypesFromArglist(opexp.Arguments, this);
                            bool br = false;
                            for (int i = 0; i < args.Length; i++)
                                if (!Util.CanImplicitTypeConvert(args[i], expArglist[i], this))
                                {
                                    br = true;
                                    break;
                                }
                            if (br)
                                continue;
                            //ExitClassContext(true);
                            EnterClassContext(mso.Value);
                            MlObject r = Execution.Call(opexp, this, args);
                            res.Add(new MlStoredObject() { Value = r, Access = opexp.Modifier.ToStoredObjectAccess(), Type = r.WorkingType });
                            ExitClassContext();
                            return res;
                        }
                    }

                    if (doThrow)
                        throw new RuntimeException(brexp.Origin, $"Found no definition of an indexing-operator function in '{mso.Value.UnderlyingType}' with arguments [{string.Join(", ", args.Select(_ => _.WorkingType.ToString()))}]");
                    else
                        return null;
                }

                //ExitClassContext();
            }
            if (Util.IsLiteralExpression(exp))
            {
                MlObject mlo = exp.Execute(this);
                res.Add(new MlStoredObject() { Value = mlo, Access = MlObjectAccess.LOCAL, Type = mlo.WorkingType });
                return res;
            }
            MlObject general = exp.Execute(this);
            res.Add(new MlStoredObject() { Value = general, Access = MlObjectAccess.LOCAL, Type = general.WorkingType });
            return res;
        }

        public void ReturnToGlobalScope()
        {
            while (_contextStack.Count > 0)
                ExitClassContext(true);
            while (StackFrames.Count > 1)
                ExitStackFrame(false);

            _stackFrameLayer = 0;
            InStackFrame = false;

            //SfInit();
        }

        public MlStoredObject ResolveObjectPath(Expression exp, bool typesOnly = false, bool executeAttributeFunction = true) // assumes it is a pure identifier expression
        {
            return ResolveObjectsFromPath(exp, typesOnly, executeAttributeFunction: executeAttributeFunction)[0];
        }

        public MlStoredObject ResolveObjectPath(string name, bool typesOnly = false, bool executeAttributeFunction = true) // assumes it is a pure identifier expression
        {
            return ResolveObjectPath(new IdentifierExpression(name), typesOnly, executeAttributeFunction);
            //return ResolveObjectsFromPath(new IdentifierExpression(name), typesOnly)[0];
        }

        public void SetValue(MlStoredObject mso, MlObject value, bool allowConstAssignment = false)
        {
            try
            {
                mso.ChangeValue(value, this, allowConstAssignment);
            }
            catch (InternalRuntimeException ex)
            {
                throw new RuntimeException(null, ex.Message);
            }

        }

        public void SetValue(Expression exp, MlObject value, bool allowConstAssignment = false)
        {
            MlStoredObject mso = ResolveObjectPath(exp);
            try
            {
                mso.ChangeValue(value, this, allowConstAssignment);
            } catch (InternalRuntimeException ex) 
            {
                throw new RuntimeException(exp.Origin, ex.Message);
            }
            
        }

        public void Execute(ParseResult pr)
        {
            foreach (Expression exp in pr)
                exp.Execute(this);
        }

        public MlObject SingleExecute(Expression exp)
        {
            MlObject res = exp.Execute(this);
            return res;
        }

        public void Execute(params Expression[] expressions)
        {
            for (int i = 0; i < expressions.Length; i++)
            {
                Expression exp = expressions[i];
                exp.Execute(this);
            }
        }

        public void EnterClassContext(MlObject of, bool specialSoft = false)
        {
            // Set the working type back to the underlying type so that methods inside that class can actually
            // then access their properties otherwise not found in a lower type
            if(of != null)
            {
                of.TmpType = of.WorkingType;
                of.WorkingType = of.UnderlyingType;
            }

            //SoftContextStack.Push(CurrentClassContextObjects[1]);
            if (!specialSoft)
            {
                //CurrentClassContextObjects[1] = null;
                EnterStackFrame(false);
            }
            else
            {
                //CurrentClassContextObjects[1] = CurrentClassContextObjects[0];
            }
                
            _contextStack.Push(of);
            CurrentClassContextObject = of;
        }

        public void ExitClassContext(bool specialSoft = false)
        {
            MlObject mlo = _contextStack.Pop();

            // Revert back to the original working type outside of the class context now
            if(mlo != null)
                mlo.WorkingType = mlo.TmpType;

            CurrentClassContextObject = _contextStack.Count == 0 ? null : _contextStack.Peek();
            //CurrentClassContextObjects[1] = SoftContextStack.Count == 0 ? null :  SoftContextStack.Pop();

            if (!specialSoft)
                ExitStackFrame(false);
            else
            {
                
            }
                
        }

        public void EnterStackFrame(bool soft) // if soft, then all the previous stackframes will still be available
        {
            if (!soft)
            {
                StackFrame sf = new StackFrame();
                StackFrames.Push(sf);
                CurrentStackFrame = sf;
            }
            CurrentStackFrame.VariableStack.AddLast(new Dictionary<string, MlStoredObject>());
            InStackFrame = true;
            _stackFrameLayer++;
        }

        private int _stackFrameLayer = 0;

        public void ExitStackFrame(bool soft)
        {
            CurrentStackFrame.VariableStack.RemoveLast();
            if (!soft)
            {
                StackFrames.Pop();
                if (StackFrames.Count != 0)
                    CurrentStackFrame = StackFrames.Peek();
                else
                    CurrentStackFrame = null;
            }

            _stackFrameLayer--;
            if (_stackFrameLayer <= 0)
                InStackFrame = false;
            //if (!(StackFrames.Count > 1 || CurrentStackFrame?.VariableStack.Count > 1))  InStackFrame = false;
        }
    }
}
