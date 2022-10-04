using Modlang.Abstract;
using Modlang.Abstract.Literals;
using Modlang.Runtime.CoreLib;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlObject
    {
        public MlObject BaseObject = null;
        public MlTypeObject TypeObject = null;

        /*public static MlObject MlNull(Environment env)
        {
            MlObject r = new MlObject() { UnderlyingType = MlType.NULLTYPE, WorkingType = MlType.NULLTYPE };
            r.InitFromObjectOnly(env);
            return r;
        }*/

        //public static MlStoredObject MlNullStored(Environment env) => new MlStoredObject() { Value = MlNull(env), Access = MlObjectAccess.LOCAL, Type = MlType.NULLTYPE };

        public static MlObject MlNull => new MlObject() { UnderlyingType = MlType.NULLTYPE, WorkingType = MlType.NULLTYPE };
        public static MlStoredObject MlNullStored => new MlStoredObject() { Value = MlNull, Access = MlObjectAccess.LOCAL, Type = MlType.NULLTYPE };

        public MlObjectNative NativeValue;

        public Dictionary<string, MlStoredObject> Attributes = new Dictionary<string, MlStoredObject>();
        public MlType UnderlyingType;
        public MlType WorkingType;
        public MlType TmpType;

        public List<FunctionExpression> Functions = new List<FunctionExpression>();
        //public Dictionary<string, List<OperatorFunctionExpression>> Operators = new Dictionary<string, List<OperatorFunctionExpression>>();
        public List<OperatorFunctionExpression> OperatorsList = new List<OperatorFunctionExpression>();

        public bool IsPrimitive;
        public bool IsInitialized = true;
        internal Environment _env;

        public virtual string ToString(Environment env) => ToString();

        public virtual MlObject CastClone(Environment env, MlType to = null)
        {
            if(this is MlArray mar)
            {
                return new MlCastedArray(mar, to ?? WorkingType, env);
            }
            else
            {
                return new MlCastedObject(this, to ?? WorkingType, env);
            }
        }

        [Obsolete]
        public virtual MlObject Clone(Environment env)
        {
            MlObject newObj = new MlObject
            {
                BaseObject = BaseObject?.Clone(env),
                TypeObject = TypeObject,
                NativeValue = (MlObjectNative)NativeValue.Clone(env),
                UnderlyingType = UnderlyingType,
                WorkingType = WorkingType,
                _env = env,
                IsInitialized = IsInitialized,
                Functions = new List<FunctionExpression>(Functions),
                //Operators = new Dictionary<string, List<OperatorFunctionExpression>>(Operators),
                IsPrimitive = IsPrimitive,
                OperatorsList = OperatorsList
            };

            //newObj.Attributes = new Dictionary<string, MlStoredObject>(Attributes);

            foreach (KeyValuePair<string, MlStoredObject> mso in Attributes)
                newObj.Attributes.Add(mso.Key, mso.Value.Clone(env));

            return newObj;
        }

        public MlObject()
        {
            NativeValue = new MlObjectNative();
        }

        internal void InitFromObjectOnly(Environment env)
        {
            IsInitialized = true;
            InheritFrom(this, env.FindNativeClass(CoreLib.Constants.TYPE_OBJECT), env);
        }

        public void Init(Environment env, params MlObject[] args)
        {
            //env.EnterClassContext(this, true);
            InitCore(env, args);
            //env.ExitClassContext(true);
        }

        private void InitCore(Environment env, params MlObject[] args)
        {
            IsInitialized = true;
            ClassExpression cexp = TypeObject.Class;
            if (cexp.InheritorExpressions != null && cexp.InheritorExpressions.Length == 1)
            {
                Expression inh = cexp.InheritorExpressions[0];
                MlStoredObject mlo = env.ResolveObjectPath(inh);
                if (mlo.Value is MlTypeObject mltoi)
                    InheritFrom(this, mltoi, env); // TODO: Base Constructor Args
                else
                    throw new RuntimeException(inh.Origin, "Invalid typename");
            }
            else if (cexp.Name != CoreLib.Constants.TYPE_OBJECT)
                InheritFrom(this, env.FindNativeClass(CoreLib.Constants.TYPE_OBJECT), env);

            for (int i1 = 0; i1 < cexp.NonStaticFunctions.Length; i1++)
            {
                FunctionExpression fexp = cexp.NonStaticFunctions[i1];
                if(fexp.RuntimeParsedArgs == null)
                    fexp.RuntimeParsedArgs = Util.ExtractTypesFromArglist(fexp.Arguments, env);
                MlType[] fexpArgs = (MlType[])fexp.RuntimeParsedArgs;
                //var fexpArgs = Util.ExtractTypesFromArglist(fexp.Arguments, env);
                if (this.BaseObject != null)
                {
                    // Replace overwritten methods in the base object with the new definition
                    bool pred(FunctionExpression _) => _.Name == fexp.Name && fexpArgs.ASequenceEqual((MlType[])_.RuntimeParsedArgs ?? (MlType[])(_.RuntimeParsedArgs = Util.ExtractTypesFromArglist(_.Arguments, env)));
                    var iter = this.BaseObject.Functions;
                    for (int i = 0; i < iter.Count; i++)
                    {
                        if (pred(iter[i]))
                            iter[i] = fexp;
                    }
                }
                this.Functions.Add(fexp);
            }
            for (int i1 = 0; i1 < cexp.OperatorFunctions.Length; i1++)
            {
                OperatorFunctionExpression opexp = cexp.OperatorFunctions[i1];
                if(opexp.RuntimeParsedArgs == null)
                    opexp.RuntimeParsedArgs = Util.ExtractTypesFromArglist(opexp.Arguments, env);
                MlType[] opexpArgs = (MlType[])opexp.RuntimeParsedArgs;
                //string str = opexp.GetHashString();
                //var opexpArgs = Util.ExtractTypesFromArglist(opexp.Arguments, env);
                if (this.BaseObject != null)
                {
                    // Replace overwritten operator-functions in the base object with the new definition
                    bool pred(OperatorFunctionExpression _) => _.Name.Operator == opexp.Name.Operator && opexpArgs.ASequenceEqual((MlType[])_.RuntimeParsedArgs ?? (MlType[])(_.RuntimeParsedArgs = Util.ExtractTypesFromArglist(_.Arguments, env)));
                    var iter = this.BaseObject.OperatorsList;
                    /*if(iter.ContainsKey(str))
                    {
                        List<OperatorFunctionExpression> replList = iter[str];
                        for (int i = 0; i < replList.Count; i++)
                        {
                            OperatorFunctionExpression opexpr = replList[i];
                            if (pred(opexpr))
                                replList[i] = opexp;
                        }
                    }*/
                    for (int i = 0; i < iter.Count; i++)
                    {
                        if (pred(iter[i]))
                            iter[i] = opexp;
                    }
                }
                /*if(this.Operators.ContainsKey(str))
                    this.Operators[str].Add(opexp);
                else
                    this.Operators.Add(str, new List<OperatorFunctionExpression>() { opexp });*/
                this.OperatorsList.Add(opexp);
            }
            for (int i = 0; i < cexp.NonStaticAttributes.Length; i++)
            {
                DeclarationExpression dexp = cexp.NonStaticAttributes[i];
                MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype, env);

                for (int i1 = 0; i1 < dexp.Assignments.Length; i1++)
                {
                    Expression aexp = dexp.Assignments[i1];
                    if (aexp is IdentifierExpression idexp)
                    {
                        this.Attributes.Add(idexp.Identifier, new MlStoredObject()
                        {
                            Value = MlNull,
                            Access = dexp.Modifier.ToStoredObjectAccess(),
                            Type = Util.DatatypeExpressionToMlType(dexp.Datatype, env)
                        });
                        continue;
                    }
                    BinaryOperatorExpression boexp = aexp as BinaryOperatorExpression;
                    string ident = (boexp.Left as IdentifierExpression).Identifier;
                    env.EnterClassContext(this);
                    MlObject right = boexp.Right.Execute(env);

                    // temporary
                    right = Execution.CastObject(right, datatype, env, true);

                    env.ExitClassContext();
                    if (right.WorkingType != datatype)
                        throw new InternalRuntimeException($"Datatype mismatch. Expected {datatype}; got {right.WorkingType}");
                    this.Attributes.Add(ident, new MlStoredObject()
                    {
                        Value = right,
                        Access = dexp.Modifier.ToStoredObjectAccess(),
                        Type = Util.DatatypeExpressionToMlType(dexp.Datatype, env)
                    });
                }
            }

            /*foreach (Expression exp in cexp.Inner)
            {
                if(exp is FunctionExpression fexp && !fexp.IsStatic)
                {
                    var fexpArgs = Util.ExtractTypesFromArglist(fexp.Arguments);
                    if (this.BaseObject != null)
                    {
                        // Replace overwritten methods in the base object with the new definition
                        bool pred(FunctionExpression _) => _.Name == fexp.Name && fexpArgs.ASequenceEqual(Util.ExtractTypesFromArglist(_.Arguments));
                        var iter = this.BaseObject.Functions;
                        for (int i = 0; i < iter.Count; i++)
                        {
                            if (pred(iter[i]))
                                iter[i] = fexp;
                        }
                    }
                    this.Functions.Add(fexp);
                } else if(exp is OperatorFunctionExpression opexp && !opexp.IsStatic)
                {
                    var opexpArgs = Util.ExtractTypesFromArglist(opexp.Arguments);
                    if (this.BaseObject != null)
                    {
                        // Replace overwritten operator-functions in the base object with the new definition
                        bool pred(OperatorFunctionExpression _) => _.Name.Operator == opexp.Name.Operator && opexpArgs.ASequenceEqual(Util.ExtractTypesFromArglist(_.Arguments));
                        var iter = this.BaseObject.Operators;
                        for (int i = 0; i < iter.Count; i++)
                        {
                            if (pred(iter[i]))
                                iter[i] = opexp;
                        }
                    }
                    this.Operators.Add(opexp);
                } else if (exp is DeclarationExpression dexp && !(dexp.IsStatic || dexp.IsConst))
                {
                    MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                    foreach (Expression aexp in dexp.Assignments)
                    {
                        if (aexp is IdentifierExpression idexp)
                        {
                            this.Attributes.Add(idexp.Identifier, new MlStoredObject()
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
                        this.Attributes.Add(ident, new MlStoredObject()
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
                if (exp is DeclarationExpression dexp && !(dexp.IsStatic || dexp.IsConst))
                {
                    MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                    foreach (Expression aexp in dexp.Assignments)
                    {
                        if(aexp is IdentifierExpression idexp)
                        {
                            this.Attributes.Add(idexp.Identifier, new MlStoredObject()
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
                        this.Attributes.Add(ident, new MlStoredObject()
                        {
                             Value = right,
                             Access = dexp.Modifier.ToStoredObjectAccess(),
                             Type = Util.DatatypeExpressionToMlType(dexp.Datatype)
                        });
                    }
                }
            }*/

            List<ConstructorExpression> impList = new List<ConstructorExpression>();
            
            for (int i1 = 0; i1 < cexp.Constructors.Length; i1++)
            {
                ConstructorExpression coexp = cexp.Constructors[i1];
                if (coexp.Arguments.Length == args.Length
                    && env.CanAccess(this, coexp.Modifier.ToStoredObjectAccess(), env.CurrentClassContextObject))
                {
                    env.EnterClassContext(this);
                    bool br = false;
                    for (int i = 0; i < args.Length; i++)
                    {
                        Expression arg = coexp.Arguments[i];
                        if (arg is DeclarationExpression dexp)
                        {
                            MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype, env);
                            if (datatype != args[i].WorkingType)
                            {
                                if (Util.CanImplicitTypeConvert(args[i], datatype, env)) impList.Add(coexp);
                                br = true;
                                break;
                            }
                        }
                    }
                    env.ExitClassContext();
                    if (br)
                        continue;
                    env.EnterClassContext(this);
                    env.AllowCallCtor++;
                    coexp.Call(env, args);
                    env.AllowCallCtor--;
                    env.ExitClassContext();
                    return;// obj;
                }
            }
            if (impList.Count > 0)
            {
                env.EnterClassContext(this);
                impList[0].Call(env, args);
                env.ExitClassContext();
                return;// obj;
            }
            //env.ExitClassContext();
            /*foreach(ConstructorExpression coexp in conList)
            {
                bool br = false;
                for (int i = 0; i < args.Length; i++)
                {
                    Expression arg = coexp.Arguments[i];
                    if (arg is DeclarationExpression dexp)
                    {
                        MlType datatype = Util.DatatypeExpressionToMlType(dexp.Datatype);
                        if (datatype != args[i].WorkingType && !Util.CanImplicitTypeConvert(args[i], datatype))
                        {
                            br = true;
                            break;
                        }
                    }
                }
                if (br)
                    continue;
                env.EnterClassContext(obj);
                coexp.Call(env, args);
                env.ExitClassContext();
                return obj;
            }*/
            if (cexp.Constructors.Length == 0 && args.Length == 0)
                return;// obj;

            /*IEnumerable<ConstructorExpression> constructors = cexp.Inner.OfType<ConstructorExpression>();

            if (constructors.Count() == 0 && args.Length == 0)
                return obj;*/

            /*foreach (ConstructorExpression coexp in constructors)
            {
                if(coexp.Arguments.Length == args.Length)
                {
                    for(int i = 0; i < args.Length; i++)
                    {
                        Expression arg = coexp.Arguments[i];
                        if(arg is DeclarationExpression dexp)
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
            }*/

            throw new InternalRuntimeException("Could not create instance of class (Found no matching constructor)");
        }

        public bool IsTypeInHierarchy(MlType type)
        {
            return (type.IsArray == UnderlyingType.IsArray || type.TypeStr == CoreLib.Constants.TYPE_OBJECT) && IsTypeInHierarchy(type.TypeStr);
        }

        public bool IsTypeInHierarchy(string typename)
        {
            if (typename == CoreLib.Constants.TYPE_OBJECT
                && (CoreLib.Constants.SpecialNativeTypes.InArray(UnderlyingType.TypeStr) || this is MlArray))
                return true;
            if (!IsInitialized)
                Init(_env);
            return UnderlyingType.TypeStr == typename || (BaseObject != null && BaseObject.IsTypeInHierarchy(typename));
        }

        public static bool CanAccessFromContext(MlObjectAccess access, MlObjectAccess context)
        {
            switch (context)
            {
                case MlObjectAccess.CLASS_CONTEXT_PRIVATE:
                    return true;
                case MlObjectAccess.CLASS_CONTEXT_PROTECTED:
                    return access == MlObjectAccess.CLASS_CONTEXT_PUBLIC || access == MlObjectAccess.CLASS_CONTEXT_PROTECTED;
                case MlObjectAccess.CLASS_CONTEXT_PUBLIC:
                    return access == MlObjectAccess.CLASS_CONTEXT_PUBLIC;
                default:
                    return false;
            }
        }

        /* if context is
         * PRIVATE you can access all attributes
         * PROTECTED you can access PUBLIC AND PROTECTED
         * PUBLIC only PUBLIC
         * I should use a different enum i know but i'm lazy
         */
        public MlStoredObject FindAttribute(string name, MlObjectAccess context = MlObjectAccess.CLASS_CONTEXT_PRIVATE)
        {
            if (!IsInitialized)
                Init(_env);
            if (UnderlyingType == WorkingType)
            {
                MlStoredObject mso;
                if(Attributes.ContainsKey(name) && CanAccessFromContext((mso = Attributes[name]).Access, context))
                    return mso;
                if(BaseObject != null && (mso = BaseObject.FindAttribute(name, MlObjectAccess.CLASS_CONTEXT_PROTECTED)) != null)
                    return mso;
                if (TypeObject != null && (mso = TypeObject.FindAttribute(name, context)) != null)
                    return mso;
                return null;
            }

            if (IsTypeInHierarchy(WorkingType.TypeStr))
                return BaseObject.FindAttribute(name, MlObjectAccess.CLASS_CONTEXT_PROTECTED);
            return null;
        }

        public List<FunctionExpression> FindFunctions(string name)
        {
            if (!IsInitialized)
                Init(_env);
            if (this is MlTypeObject)
                return Functions.FindAll(_ => _.Name == name);

            if (UnderlyingType == WorkingType)
            {
                List<FunctionExpression> fexpl = Functions.FindAll(_ => _.Name == name);
                if (BaseObject != null) fexpl.AddRange(BaseObject.FindFunctions(name));
                if (TypeObject != null) fexpl.AddRange(TypeObject.FindFunctions(name));
                return fexpl;
            }

            if (IsTypeInHierarchy(WorkingType.TypeStr))
                return BaseObject.FindFunctions(name);
            return new List<FunctionExpression>();
        }

        public List<OperatorFunctionExpression> FindOperatorFunctions(string op)
        {
            if (!IsInitialized)
                Init(_env);
            if (UnderlyingType == WorkingType)
            {
                //string[] searchers = Util.GetOperatorExpressionHashStrings(op);
                List<OperatorFunctionExpression> fexpl = /*Operators.ContainsKey(opHashStr) ? Operators[opHashStr] : new List<OperatorFunctionExpression>();*/OperatorsList.FindAll(_ => _.Name.Operator == op);
                /*for (int i = 0; i < searchers.Length; i++)
                    if (Operators.ContainsKey(searchers[i]))
                        fexpl.AddRange(Operators[searchers[i]]);*/
                if (BaseObject != null) 
                    fexpl.AddRange(BaseObject.FindOperatorFunctions(op));
                return fexpl;
            }

            if (IsTypeInHierarchy(WorkingType.TypeStr))
                return BaseObject.FindOperatorFunctions(op);
            return new List<OperatorFunctionExpression>();
        }

        protected static void InheritFrom(MlObject obj, MlTypeObject from, Environment env, params MlObject[] ctorArgs)
        {
            // TODO: Proper Inheritance
            MlObject intermediate = FromClass(from, env, ctorArgs);
            /*foreach (KeyValuePair<string, MlStoredObject> pair in intermediate.Attributes)
                obj.Attributes.Add(pair.Key, pair.Value);

            foreach (FunctionExpression fexp in intermediate.Functions)
                obj.Functions.Add(fexp);*/
            obj.BaseObject = intermediate;
        }

        //[Obsolete]
        public static MlObject FromClassNonInit(MlTypeObject mlto, Environment env, params MlObject[] args)
        {
            //ClassExpression cexp = mlto.Class;
            MlObject obj = new MlObject
            {
                UnderlyingType = mlto.GetClassTypeStr().GetNonArrayType(),
                TypeObject = mlto
            };
            obj._env = env;
            obj.WorkingType = obj.UnderlyingType;
            obj.IsPrimitive = mlto.Class.IsPrimitive;
            //obj.Init(env, args);
            obj.IsInitialized = false;
            return obj;
        }

        public static MlObject FromClassInheritObjectOnly(MlTypeObject mlto, Environment env)
        {
            //ClassExpression cexp = mlto.Class;
            MlObject obj = new MlObject
            {
                UnderlyingType = mlto.GetClassTypeStr().GetNonArrayType(),
                TypeObject = mlto
            };
            obj._env = env;
            obj.WorkingType = obj.UnderlyingType;
            obj.IsPrimitive = mlto.Class.IsPrimitive;
            obj.InitFromObjectOnly(env);
            return obj;
        }

        public static MlObject FromClass(MlTypeObject mlto, Environment env, params MlObject[] args)
        {
            //ClassExpression cexp = mlto.Class;
            MlObject obj = new MlObject
            {
                UnderlyingType = mlto.GetClassTypeStr().GetNonArrayType(),
                TypeObject = mlto
            };
            obj._env = env;
            obj.WorkingType = obj.UnderlyingType;
            obj.IsPrimitive = mlto.Class.IsPrimitive;
            obj.Init(env, args);
            return obj;
        }

        public override string ToString()
        {
            return $"{WorkingType}:MlObject ({UnderlyingType})";
        }
    }
}
